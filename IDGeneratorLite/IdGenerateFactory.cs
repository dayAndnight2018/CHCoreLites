using System;
using System.Collections.Generic;
using System.Text;

namespace IDGeneratorLite
{
    /// <summary>
    /// ID生成器
    /// </summary>
    public class IDGenerateFactory
    {
        private static IDGenerator generator = new IDGenerator(sequenceBits:20);
       
        /// <summary>
        /// 生成id
        /// </summary>
        /// <returns>获取到的id</returns>
        public static long NewID()
        {
            return generator.NewSequenceId();
        }
    }
}
