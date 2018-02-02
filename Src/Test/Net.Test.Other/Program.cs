using Castle.DynamicProxy;
using Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Test.Other
{

    interface ITest
    {
        string arg { get; set; }
        void GetUser(string a);
        void GetUser();
    }


    class Test : ITest
    {
        public string arg { get; set; }

        public void GetUser()
        {
            Console.WriteLine(arg);
        }

        public virtual void GetUser(string a)
        {
            Console.WriteLine(a);
        }
    }

    class tttt
    {

    }

    class TestInterceptor : IInterceptor
    {
        private string _arg;
        public TestInterceptor(string arg)
        {
            _arg = arg;
        }
        public  void Intercept(IInvocation invocation)
        {
            //invocation.SetArgumentValue(0, _arg);
            Console.WriteLine("===========处理前");
            //invocation.Proceed();
            Console.WriteLine("===========处理后");
        }
    }


    class yyyy
    {
        interface ITest
        {
            void aaa();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            

            var proxy = new ProxyGenerator();

            //方法一 注意 被代理的方法必须为虚方法
            //var t= proxy.CreateClassProxy<Test>(new TestInterceptor());
            //方法二
            //var t= proxy.CreateInterfaceProxyWithTarget<ITest>(new Test(), new IInterceptor[] { new TestInterceptor() });
            //方法三
            string arg = "处理中*****************************";
            var _type = Type.GetType("Net.Test.Other.ITest,Net.Test.Other", false);

            var t = (ITest)proxy.CreateInterfaceProxyWithoutTarget(_type, new TestInterceptor(arg));
            t.arg = arg;
            t.GetUser();

            Console.ReadKey();
        }
    }
}
