using Net.Boot.Configs;
using Net.Boot.Module;
using Net.Boot.Register;
using Net.Core;
using System.Collections.Generic;
using System.Diagnostics;

namespace Net.Boot
{
    public static class _Application
    {
        private static bool _inited;
        private static readonly object LockObject = new object();
        /// <summary>
        /// 1:加载配置文件
        /// 2:初始依赖注入
        /// 3:初始模块数据
        /// </summary>
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
                        ContainerSingleton.SetContainer(new Container());
                        List<IBootStrategy> listStrategy = new List<IBootStrategy> {
                            new ConfigsLoad(),
                            new RegisterLoad(),
                            new ModulesLoad()
                        };
                        using (IEnumerator<IBootStrategy> enumerator = listStrategy.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                enumerator.Current.Run();
                            }
                        }
                        stopwatch.Stop();
                        _inited = true;
                    }
                }
            }
        }
    }
}
