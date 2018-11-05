using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //模拟数据变化
            //Task.Factory.StartNew(() => 
            //{
            //    System.Threading.Thread.Sleep(2000);
            //    WeightedRoundRobin.Change();
            //});
            //正是测试开始
            Dictionary<string, int> dic = new Dictionary<string, int>();
            Server s;
            for (int j = 0; j < 100; j++)
            {
                s = WeightedRoundRobin.GetServer();
                Console.WriteLine("{0},weight:{1}", s.IP, s.Weight);

                if (!dic.Keys.Contains("服务器" + s.IP + ",权重:" + s.Weight))
                    dic.Add("服务器" + s.IP + ",权重:" + s.Weight, 0);
                dic["服务器" + s.IP + ",权重:" + s.Weight]++;
                System.Threading.Thread.Sleep(100);
            }

            foreach (var i1 in dic)
            {
                Console.WriteLine("{0}，共处理请求{1}次", i1.Key, i1.Value);
            }

            Console.ReadLine();
        }
    }

    /// <summary>
    /// 权重轮询算法
    /// </summary>
    public static class WeightedRoundRobin
    {

        public static void Change()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(2000);
                s.Add(new Server()
                {
                    IP = "192.168.0.105",
                    Weight = 30
                });
                System.Threading.Thread.Sleep(4000);
                s.RemoveAt(s.Count - 1);

                System.Threading.Thread.Sleep(40000000);

            }
        }

        private static List<Server> s = new List<Server>()
                {
                    new Server()
                        {
                            IP = "192.168.0.100",
                            Weight = 10
                        },
                    new Server()
                        {
                            IP = "192.168.0.101",
                            Weight = 2
                        },
                    new Server()
                        {
                            IP = "192.168.0.102",
                            Weight = 2
                        },
                    new Server()
                        {
                            IP = "192.168.0.103",
                            Weight = 1
                        },
                    new Server()
                        {
                            IP = "192.168.0.104",
                            Weight = 1
                        },
                }.OrderBy(a => a.Weight).ToList();

        private static int i = -1;//代表上一次选择的服务器
        //private static int gcd = GetGcd(s);//表示集合S中所有服务器权值的最大公约数
        private static int cw = 0;//当前调度的权值
        //private static int max = GetMaxWeight(s);
        //private static int n = s.Count;//服务器个数


        /**
         * 算法流程：
         * 假设有一组服务器 S = {S0, S1, …, Sn-1} ，有相应的权重，变量I表示上次选择的服务器，1每次步长
         * 权值cw初始化为0，i初始化为-1 ，当第一次的时候 权值取最大的那个服务器，
         * 通过权重的不断递减 寻找 适合的服务器返回，直到轮询结束，权值返回为0 
         */
        public static Server GetServer()
        {
            while (true)
            {
                i = (i + 1) % s.Count;
                if (i == 0)
                {
                    cw = cw - GetGcd(s);
                    if (cw <= 0)
                    {
                        cw = GetMaxWeight(s);
                        if (cw == 0)
                            return s[s.Count - 1];
                    }
                }
                if (s[i].Weight >= cw)
                {
                    return s[i];
                }
            }
        }


        /// <summary>
        /// 获取服务器所有权值的最大公约数
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        private static int GetGcd(List<Server> servers)
        {
            return 1;
        }
        /// <summary>
        /// 获取最大的权值
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        private static int GetMaxWeight(List<Server> servers)
        {
            int max = 0;
            foreach (var s in servers)
            {
                if (s.Weight > max)
                    max = s.Weight;
            }
            return max;
        }
    }
    /// <summary>
    /// 服务器结构
    /// </summary>
    public class Server
    {
        public string IP;
        public int Weight;
    }
}
