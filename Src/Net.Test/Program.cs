using Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        static void Main(string[] args)
        {

            byte[] aaa = new byte[62000000 + 4700000];


            Net.Boot.Service.ServiceApplication.Run();
            //Net.Boot._Application.Run();

            //var t= ContainerSingleton.Instance.Resolve<Itest>();
            //t.write("hello world");

            //ContainerSingleton.Instance.Resolve<mmmm>().test("hello word!");
            //new mmmm().test();

            Console.ReadKey();
        }
    }
}
