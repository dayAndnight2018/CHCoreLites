﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SqlLite.Dapper
{
    public static partial class SqlMapper
    {
        private static Task<DbDataReader> ExecuteReaderWithFlagsFallbackAsync(DbCommand cmd, bool wasClosed, CommandBehavior behavior, CancellationToken cancellationToken)
        {
            var task = cmd.ExecuteReaderAsync(GetBehavior(wasClosed, behavior), cancellationToken);
            if (task.Status == TaskStatus.Faulted && Settings.DisableCommandBehaviorOptimizations(behavior, task.Exception.InnerException))
            { // we can retry; this time it will have different flags
                return cmd.ExecuteReaderAsync(GetBehavior(wasClosed, behavior), cancellationToken);
            }
            return task;
        }

        /// <summary>
        /// Attempts to open a connection asynchronously, with a better error message for unsupported usages.
        /// </summary>
        private static Task TryOpenAsync(this IDbConnection cnn, CancellationToken cancel)
        {
            if (cnn is DbConnection dbConn)
            {
                return dbConn.OpenAsync(cancel);
            }
            else
            {
                throw new InvalidOperationException("Async operations require use of a DbConnection or an already-open IDbConnection");
            }
        }

        /// <summary>
        /// Attempts setup a <see cref="DbCommand"/> on a <see cref="DbConnection"/>, with a better error message for unsupported usages.
        /// </summary>
        private static DbCommand TrySetupAsyncCommand(this CommandDefinition command, IDbConnection cnn, Action<IDbCommand, object> paramReader)
        {
            if (command.SetupCommand(cnn, paramReader) is DbCommand dbCommand)
            {
                return dbCommand;
            }
            else
            {
                throw new InvalidOperationException("Async operations require use of a DbConnection or an IDbConnection where .CreateCommand() returns a DbCommand");
            }
        }

        internal static async Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection cnn, Type effectiveType, CommandDefinition command)
        {
            object param = command.Parameters;
            var identity = new Identity(command.CommandText, command.CommandType, cnn, effectiveType, param?.GetType(), null);
            var info = GetCacheInfo(identity, param, command.AddToCache);
            bool wasClosed = cnn.State == ConnectionState.Closed;
            var cancel = command.CancellationToken;
            using (var cmd = command.TrySetupAsyncCommand(cnn, info.ParamReader))
            {
                DbDataReader reader = null;
                try
                {
                    if (wasClosed) await cnn.TryOpenAsync(cancel).ConfigureAwait(false);
                    reader = await ExecuteReaderWithFlagsFallbackAsync(cmd, wasClosed, CommandBehavior.SequentialAccess | CommandBehavior.SingleResult, cancel).ConfigureAwait(false);

                    var tuple = info.Deserializer;
                    int hash = GetColumnHash(reader);
                    if (tuple.Func == null || tuple.Hash != hash)
                    {
                        if (reader.FieldCount == 0)
                            return Enumerable.Empty<T>();
                        tuple = info.Deserializer = new DeserializerState(hash, GetDeserializer(effectiveType, reader, 0, -1, false));
                        if (command.AddToCache) SetQueryCache(identity, info);
                    }

                    var func = tuple.Func;

                    if (command.Buffered)
                    {
                        var buffer = new List<T>();
                        var convertToType = Nullable.GetUnderlyingType(effectiveType) ?? effectiveType;
                        while (await reader.ReadAsync(cancel).ConfigureAwait(false))
                        {
                            object val = func(reader);
                            if (val == null || val is T)
                            {
                                buffer.Add((T)val);
                            }
                            else
                            {
                                buffer.Add((T)Convert.ChangeType(val, convertToType, CultureInfo.InvariantCulture));
                            }
                        }
                        while (await reader.NextResultAsync(cancel).ConfigureAwait(false)) { /* ignore subsequent result sets */ }
                        command.OnCompleted();
                        return buffer;
                    }
                    else
                    {
                        // can't use ReadAsync / cancellation; but this will have to do
                        wasClosed = false; // don't close if handing back an open reader; rely on the command-behavior
                        var deferred = ExecuteReaderSync<T>(reader, func, command.Parameters);
                        reader = null; // to prevent it being disposed before the caller gets to see it
                        return deferred;
                    }
                }
                finally
                {
                    using (reader) { /* dispose if non-null */ }
                    if (wasClosed) cnn.Close();
                }
            }
        }

        internal static async Task<T> QueryRowAsync<T>(this IDbConnection cnn, Row row, Type effectiveType, CommandDefinition command)
        {
            object param = command.Parameters;
            var identity = new Identity(command.CommandText, command.CommandType, cnn, effectiveType, param?.GetType(), null);
            var info = GetCacheInfo(identity, param, command.AddToCache);
            bool wasClosed = cnn.State == ConnectionState.Closed;
            var cancel = command.CancellationToken;
            using (var cmd = command.TrySetupAsyncCommand(cnn, info.ParamReader))
            {
                DbDataReader reader = null;
                try
                {
                    if (wasClosed) await cnn.TryOpenAsync(cancel).ConfigureAwait(false);
                    reader = await ExecuteReaderWithFlagsFallbackAsync(cmd, wasClosed, (row & Row.Single) != 0
                    ? CommandBehavior.SequentialAccess | CommandBehavior.SingleResult // need to allow multiple rows, to check fail condition
                    : CommandBehavior.SequentialAccess | CommandBehavior.SingleResult | CommandBehavior.SingleRow, cancel).ConfigureAwait(false);

                    T result = default(T);
                    if (await reader.ReadAsync(cancel).ConfigureAwait(false) && reader.FieldCount != 0)
                    {
                        var tuple = info.Deserializer;
                        int hash = GetColumnHash(reader);
                        if (tuple.Func == null || tuple.Hash != hash)
                        {
                            tuple = info.Deserializer = new DeserializerState(hash, GetDeserializer(effectiveType, reader, 0, -1, false));
                            if (command.AddToCache) SetQueryCache(identity, info);
                        }

                        var func = tuple.Func;

                        object val = func(reader);
                        if (val == null || val is T)
                        {
                            result = (T)val;
                        }
                        else
                        {
                            var convertToType = Nullable.GetUnderlyingType(effectiveType) ?? effectiveType;
                            result = (T)Convert.ChangeType(val, convertToType, CultureInfo.InvariantCulture);
                        }
                        if ((row & Row.Single) != 0 && await reader.ReadAsync(cancel).ConfigureAwait(false)) ThrowMultipleRows(row);
                        while (await reader.ReadAsync(cancel).ConfigureAwait(false)) { /* ignore rows after the first */ }
                    }
                    else if ((row & Row.FirstOrDefault) == 0) // demanding a row, and don't have one
                    {
                        ThrowZeroRows(row);
                    }
                    while (await reader.NextResultAsync(cancel).ConfigureAwait(false)) { /* ignore result sets after the first */ }
                    return result;
                }
                finally
                {
                    using (reader) { /* dispose if non-null */ }
                    if (wasClosed) cnn.Close();
                }
            }
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="cnn">The connection to query on.</param>
        /// <param name="command">The command to execute for this query.</param>
        internal static async Task<GridReader> QueryMultipleAsync(this IDbConnection cnn, CommandDefinition command)
        {
            object param = command.Parameters;
            var identity = new Identity(command.CommandText, command.CommandType, cnn, typeof(GridReader), param?.GetType(), null);
            CacheInfo info = GetCacheInfo(identity, param, command.AddToCache);

            DbCommand cmd = null;
            IDataReader reader = null;
            bool wasClosed = cnn.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) await cnn.TryOpenAsync(command.CancellationToken).ConfigureAwait(false);
                cmd = command.TrySetupAsyncCommand(cnn, info.ParamReader);
                reader = await ExecuteReaderWithFlagsFallbackAsync(cmd, wasClosed, CommandBehavior.SequentialAccess, command.CancellationToken).ConfigureAwait(false);

                var result = new GridReader(cmd, reader, identity, command.Parameters as DynamicParameters, command.AddToCache, command.CancellationToken);
                wasClosed = false; // *if* the connection was closed and we got this far, then we now have a reader
                // with the CloseConnection flag, so the reader will deal with the connection; we
                // still need something in the "finally" to ensure that broken SQL still results
                // in the connection closing itself
                return result;
            }
            catch
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        try { cmd.Cancel(); }
                        catch
                        { /* don't spoil the existing exception */
                        }
                    }
                    reader.Dispose();
                }
                cmd?.Dispose();
                if (wasClosed) cnn.Close();
                throw;
            }
        }

        private struct AsyncExecState
        {
            public readonly DbCommand Command;
            public readonly Task<int> Task;
            public AsyncExecState(DbCommand command, Task<int> task)
            {
                Command = command;
                Task = task;
            }
        }

        internal static async Task<int> ExecuteMultiImplAsync(IDbConnection cnn, CommandDefinition command, IEnumerable multiExec)
        {
            bool isFirst = true;
            int total = 0;
            bool wasClosed = cnn.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) await cnn.TryOpenAsync(command.CancellationToken).ConfigureAwait(false);

                CacheInfo info = null;
                string masterSql = null;
                if ((command.Flags & CommandFlags.Pipelined) != 0)
                {
                    const int MAX_PENDING = 100;
                    var pending = new Queue<AsyncExecState>(MAX_PENDING);
                    DbCommand cmd = null;
                    try
                    {
                        foreach (var obj in multiExec)
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                                cmd = command.TrySetupAsyncCommand(cnn, null);
                                masterSql = cmd.CommandText;
                                var identity = new Identity(command.CommandText, cmd.CommandType, cnn, null, obj.GetType(), null);
                                info = GetCacheInfo(identity, obj, command.AddToCache);
                            }
                            else if (pending.Count >= MAX_PENDING)
                            {
                                var recycled = pending.Dequeue();
                                total += await recycled.Task.ConfigureAwait(false);
                                cmd = recycled.Command;
                                cmd.CommandText = masterSql; // because we do magic replaces on "in" etc
                                cmd.Parameters.Clear(); // current code is Add-tastic
                            }
                            else
                            {
                                cmd = command.TrySetupAsyncCommand(cnn, null);
                            }
                            info.ParamReader(cmd, obj);

                            var task = cmd.ExecuteNonQueryAsync(command.CancellationToken);
                            pending.Enqueue(new AsyncExecState(cmd, task));
                            cmd = null; // note the using in the finally: this avoids a double-dispose
                        }
                        while (pending.Count != 0)
                        {
                            var pair = pending.Dequeue();
                            using (pair.Command) { /* dispose commands */ }
                            total += await pair.Task.ConfigureAwait(false);
                        }
                    }
                    finally
                    {
                        // this only has interesting work to do if there are failures
                        using (cmd) { /* dispose commands */ }
                        while (pending.Count != 0)
                        { // dispose tasks even in failure
                            using (pending.Dequeue().Command) { /* dispose commands */ }
                        }
                    }
                }
                else
                {
                    using (var cmd = command.TrySetupAsyncCommand(cnn, null))
                    {
                        foreach (var obj in multiExec)
                        {
                            if (isFirst)
                            {
                                masterSql = cmd.CommandText;
                                isFirst = false;
                                var identity = new Identity(command.CommandText, cmd.CommandType, cnn, null, obj.GetType(), null);
                                info = GetCacheInfo(identity, obj, command.AddToCache);
                            }
                            else
                            {
                                cmd.CommandText = masterSql; // because we do magic replaces on "in" etc
                                cmd.Parameters.Clear(); // current code is Add-tastic
                            }
                            info.ParamReader(cmd, obj);
                            total += await cmd.ExecuteNonQueryAsync(command.CancellationToken).ConfigureAwait(false);
                        }
                    }
                }

                command.OnCompleted();
            }
            finally
            {
                if (wasClosed) cnn.Close();
            }
            return total;
        }

        internal static async Task<int> ExecuteImplAsync(IDbConnection cnn, CommandDefinition command, object param)
        {
            var identity = new Identity(command.CommandText, command.CommandType, cnn, null, param?.GetType(), null);
            var info = GetCacheInfo(identity, param, command.AddToCache);
            bool wasClosed = cnn.State == ConnectionState.Closed;
            using (var cmd = command.TrySetupAsyncCommand(cnn, info.ParamReader))
            {
                try
                {
                    if (wasClosed) await cnn.TryOpenAsync(command.CancellationToken).ConfigureAwait(false);
                    var result = await cmd.ExecuteNonQueryAsync(command.CancellationToken).ConfigureAwait(false);
                    command.OnCompleted();
                    return result;
                }
                finally
                {
                    if (wasClosed) cnn.Close();
                }
            }
        }

        internal static async Task<IEnumerable<TReturn>> MultiMapAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDbConnection cnn, CommandDefinition command, Delegate map, string splitOn)
        {
            object param = command.Parameters;
            var identity = new Identity(command.CommandText, command.CommandType, cnn, typeof(TFirst), param?.GetType(), new[] { typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth), typeof(TFifth), typeof(TSixth), typeof(TSeventh) });
            var info = GetCacheInfo(identity, param, command.AddToCache);
            bool wasClosed = cnn.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) await cnn.TryOpenAsync(command.CancellationToken).ConfigureAwait(false);
                using (var cmd = command.TrySetupAsyncCommand(cnn, info.ParamReader))
                using (var reader = await ExecuteReaderWithFlagsFallbackAsync(cmd, wasClosed, CommandBehavior.SequentialAccess | CommandBehavior.SingleResult, command.CancellationToken).ConfigureAwait(false))
                {
                    if (!command.Buffered) wasClosed = false; // handing back open reader; rely on command-behavior
                    var results = MultiMapImpl<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(null, CommandDefinition.ForCallback(command.Parameters), map, splitOn, reader, identity, true);
                    return command.Buffered ? results.ToList() : results;
                }
            }
            finally
            {
                if (wasClosed) cnn.Close();
            }
        }

        internal static async Task<IEnumerable<TReturn>> MultiMapAsync<TReturn>(this IDbConnection cnn, CommandDefinition command, Type[] types, Func<object[], TReturn> map, string splitOn)
        {
            if (types.Length < 1)
            {
                throw new ArgumentException("you must provide at least one type to deserialize");
            }

            object param = command.Parameters;
            var identity = new Identity(command.CommandText, command.CommandType, cnn, types[0], param?.GetType(), types);
            var info = GetCacheInfo(identity, param, command.AddToCache);
            bool wasClosed = cnn.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) await cnn.TryOpenAsync(command.CancellationToken).ConfigureAwait(false);
                using (var cmd = command.TrySetupAsyncCommand(cnn, info.ParamReader))
                using (var reader = await ExecuteReaderWithFlagsFallbackAsync(cmd, wasClosed, CommandBehavior.SequentialAccess | CommandBehavior.SingleResult, command.CancellationToken).ConfigureAwait(false))
                {
                    var results = MultiMapImpl(null, default(CommandDefinition), types, map, splitOn, reader, identity, true);
                    return command.Buffered ? results.ToList() : results;
                }
            }
            finally
            {
                if (wasClosed) cnn.Close();
            }
        }

        private static IEnumerable<T> ExecuteReaderSync<T>(IDataReader reader, Func<IDataReader, object> func, object parameters)
        {
            using (reader)
            {
                while (reader.Read())
                {
                    yield return (T)func(reader);
                }
                while (reader.NextResult()) { /* ignore subsequent result sets */ }
                (parameters as IParameterCallbacks)?.OnCompleted();
            }
        }

        internal static async Task<IDataReader> ExecuteReaderImplAsync(IDbConnection cnn, CommandDefinition command, CommandBehavior commandBehavior)
        {
            Action<IDbCommand, object> paramReader = GetParameterReader(cnn, ref command);

            DbCommand cmd = null;
            bool wasClosed = cnn.State == ConnectionState.Closed;
            try
            {
                cmd = command.TrySetupAsyncCommand(cnn, paramReader);
                if (wasClosed) await cnn.TryOpenAsync(command.CancellationToken).ConfigureAwait(false);
                var reader = await ExecuteReaderWithFlagsFallbackAsync(cmd, wasClosed, commandBehavior, command.CancellationToken).ConfigureAwait(false);
                wasClosed = false;
                return reader;
            }
            finally
            {
                if (wasClosed) cnn.Close();
                cmd?.Dispose();
            }
        }

        internal static async Task<T> ExecuteScalarImplAsync<T>(IDbConnection cnn, CommandDefinition command)
        {
            Action<IDbCommand, object> paramReader = null;
            object param = command.Parameters;
            if (param != null)
            {
                var identity = new Identity(command.CommandText, command.CommandType, cnn, null, param.GetType(), null);
                paramReader = GetCacheInfo(identity, command.Parameters, command.AddToCache).ParamReader;
            }

            DbCommand cmd = null;
            bool wasClosed = cnn.State == ConnectionState.Closed;
            object result;
            try
            {
                cmd = command.TrySetupAsyncCommand(cnn, paramReader);
                if (wasClosed) await cnn.TryOpenAsync(command.CancellationToken).ConfigureAwait(false);
                result = await cmd.ExecuteScalarAsync(command.CancellationToken).ConfigureAwait(false);
                command.OnCompleted();
            }
            finally
            {
                if (wasClosed) cnn.Close();
                cmd?.Dispose();
            }
            return Parse<T>(result);
        }
    }
}
