using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Boot
{
    public static class BootStrategy
    {
        private static bool _inited;
        private static readonly object LockObject = new object();
        
        public static void Run()
        {
            if (!_inited)
            {
                lock (LockObject)
                {
                    if (!_inited)
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();
                        //ContainerSingleton.SetContainer(container);
                        //1:加载配置文件

                        //2:初始依赖注入

                        //3:初始模块数据
                        stopwatch.Stop();                   
                        _inited = true;
                    }
                }
            }
        }
    }
}
