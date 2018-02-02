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
        static Semaphore sema = new Semaphore(5, 5);
        const int cycleNum = 9;
        static void Main(string[] args)
        {

            for (int i = 0; i < cycleNum; i++)
            {
                Thread td = new Thread(new ParameterizedThreadStart(testFun));
                td.Name = string.Format("编号{0}", i.ToString());
                td.Start(td.Name);
            }
            Console.ReadKey();


            Net.Boot.Service.ServiceApplication.Run();
            

            //var t= ContainerSingleton.Instance.Resolve<Itest>();
            //t.write("hello world");

            //ContainerSingleton.Instance.Resolve<mmmm>().test("hello word!");
            //new mmmm().test();

            Console.ReadKey();
        }

        static void testFun(object obj)
        {
            sema.WaitOne();
            Console.WriteLine(obj.ToString() + "进洗手间：" + DateTime.Now.ToString());
            Thread.Sleep(2000);
            Console.WriteLine(obj.ToString() + "出洗手间：" + DateTime.Now.ToString());
            sema.Release();
        }
    }
}
