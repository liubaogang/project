using Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net.Test
{
    interface Itest
    {
        void write(string t);
    }

    class test : Itest
    {
        public void write(string t)
        {
            Console.WriteLine(t);
        }
    }


    class mmmm
    {
        private Itest _t;
        public mmmm(Itest t)
        {
            _t = t;
        }

        public void test(string a)
        {
            _t.write(a);
        }
    }

    class modeul : IModuleType
    {
        public void Execute(IConfigsType dictData)
        {
            var t = dictData.Get("Cicada.DI.AutoRegisterByProductName");
            Console.WriteLine(t);
        }
    }

    class server : Net.Boot.Service.IService
    {
        private readonly IConfigsType _config;
        public server(IConfigsType config)
        {
            _config = config;
        }
        public void Start()
        {
            Console.WriteLine(_config.Get("Cicada.Boot.Service.Description"));
        }

        public void Stop()
        {
        }
    }


    class Program
    {

        static async Task Async2(string str)
        {
            //return await Task.Run(() =>
            // {
            //     Console.WriteLine(" Async2 线程ID:" + Thread.CurrentThread.ManagedThreadId);
            //     Thread.Sleep(3000);
            //     Console.WriteLine("bbb");
            //     return "my is result" + str;
            // });
        }

        static Semaphore sema = new Semaphore(500, 500);
        const int cycleNum = 9;
        static void Main(string[] args)
        {
            Console.WriteLine("【Debug】主 线程ID:" + Thread.CurrentThread.ManagedThreadId);
            string yyyyy = await Program.Async2("=liubaogang");
            Console.WriteLine("*************************");
            //var mmm = yyyyy.Result;
            Console.WriteLine("输出结果："+ yyyyy);

            Console.ReadKey();




            Console.WriteLine("【Debug】主 线程ID:" + Thread.CurrentThread.ManagedThreadId);
            string str1 = string.Empty, str2 = string.Empty, str3 = string.Empty;
            var task1 = Task.Run(() =>
            {
                Thread.Sleep(500);
                str1 = "姓名：张三,";
                Console.WriteLine("【Debug】task1 线程ID:" + Thread.CurrentThread.ManagedThreadId);
            }).ContinueWith(t =>
            {
                Thread.Sleep(500);
                str2 = "年龄：25,";
                Console.WriteLine("【Debug】task2 线程ID:" + Thread.CurrentThread.ManagedThreadId);
            }).ContinueWith(t =>
            {
                Thread.Sleep(500);
                str3 = "爱好：妹子";
                Console.WriteLine("【Debug】task3 线程ID:" + Thread.CurrentThread.ManagedThreadId);
            });

            Thread.Sleep(2500);//其他逻辑代码

            task1.Wait();

            Console.WriteLine(str1 + str2 + str3);
            Console.WriteLine("【Debug】主 线程ID:" + Thread.CurrentThread.ManagedThreadId);

            Console.ReadKey();



            var ts = new List<Thread>();
            for (int i = 0; i < 5000; i++)
            {
                Thread td = new Thread(new ParameterizedThreadStart(testFun));
                td.Name = string.Format("编号{0}", i.ToString());
                ts.Add(td);
            }
            ts.ForEach(a => a.Start(a.Name));
            ts.ForEach(a => a.Join());


            //for (int i = 0; i < cycleNum; i++)
            //{
            //    Thread td = new Thread(new ParameterizedThreadStart(testFun));
            //    td.Name = string.Format("编号{0}", i.ToString());
            //    td.Start(td.Name);
            //}
            Console.ReadKey();


            Net.Boot.Service.ServiceApplication.Run();


            //var t= ContainerSingleton.Instance.Resolve<Itest>();
            //t.write("hello world");

            //ContainerSingleton.Instance.Resolve<mmmm>().test("hello word!");
            //new mmmm().test();

            Console.ReadKey();
        }
        object obj = new object();
        static void testFun(object obj)
        {
            //lock(obj)
            //{
            //    Console.WriteLine(obj.ToString() + "进洗手间：" + DateTime.Now.ToString());
            //    Thread.Sleep(2000);
            //    Console.WriteLine(obj.ToString() + "出洗手间：" + DateTime.Now.ToString());
            //}


            sema.WaitOne();
            Console.WriteLine(obj.ToString() + "进洗手间：" + DateTime.Now.ToString());
            Thread.Sleep(2000);
            Console.WriteLine(obj.ToString() + "出洗手间：" + DateTime.Now.ToString());
            sema.Release();
        }
    }
}
