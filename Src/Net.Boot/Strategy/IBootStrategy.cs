using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Boot.Strategy
{
    /// <summary>
    /// 启动策略
    /// </summary>
    public interface IBootStrategy
    {
        void Run();
    }
}
