using System;
using System.Collections.Generic;
using System.Text;

namespace IDGeneratorLite.cs
{
    /// <summary>
    /// ID生成器
    /// </summary>
    public class IDGenerateFactory
    {
        private static IDGenerator generator;

        private static object LOCK = new object();

        private static IDGenerator GetInstance()
        {
            if(generator == null)
            {
                lock(LOCK)
                {
                    if(generator ==null)
                    {
                        generator = new IDGenerator(sequenceBits: 20);
                    }
                }
            }

            return generator;
        }

        /// <summary>
        /// 生成id
        /// </summary>
        /// <returns>获取到的id</returns>
        public static long NewID()
        {
            if (generator == null)
            {
                GetInstance();
            }
            return generator.NewSequenceId();
        }
    }
}
