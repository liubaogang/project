using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Base
{
    public enum LifeTime
    {
        /// <summary>
        /// 每次解析时创建
        /// </summary>
        PerResolve,

        /// <summary>
        /// 单例
        /// </summary>
        Singleton,
    }
}
