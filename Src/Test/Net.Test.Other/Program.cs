using Castle.DynamicProxy;
using Net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public virtual void GetUser()
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
        public void Intercept(IInvocation invocation)
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

    static class AAA
    {
        public static void ConAAA()
        {
        }
    }

   


    class kkkk
    {
        public static string test()
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    return "";
                }
                catch
                {
                    if (i == (10 - 1))
                    {
                        throw;
                    }
                }
            }
            return "";
        }
    }

    class testusing : IDisposable
    {
         
        public void Dispose()
        {
            Console.WriteLine("执行了-Dispose");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CodeTimer
            using (var tsd = new testusing())
            {
                Console.WriteLine("执行方法了=====");
            }

                kkkk.test();
            //Process p = new Process();
            //p.StartInfo.FileName = "cmd.exe";//设置启动的应用程序  
            //p.StartInfo.UseShellExecute = false;//禁止使用操作系统外壳程序启动进程  
            //p.StartInfo.RedirectStandardInput = true;//应用程序的输入从流中读取  
            //p.StartInfo.RedirectStandardOutput = true;//应用程序的输出写入流中  
            //p.StartInfo.RedirectStandardError = true;//将错误信息写入流  
            //p.StartInfo.CreateNoWindow = true;//是否在新窗口中启动进程  
            //p.Start();
            ////p.StandardInput.WriteLine(@"netstat -a -n>c:\port.txt");//将字符串写入文本流  
            ////p.StandardInput.WriteLine(@"netstat -a -n");

            //p.StandardInput.WriteLine(@"ping 172.18.104.120 -t");
            //string str;
            //while ((str = p.StandardOutput.ReadLine()) != null)
            //{
            //    Console.WriteLine(str);
            //}

            //Console.ReadKey();

            var proxy = new ProxyGenerator();

            //方法一 注意 被代理的方法必须为虚方法
            //var t = proxy.CreateClassProxy<Test>(new TestInterceptor("2"));
            //t.arg = 3.ToString();
            //t.GetUser();
            //方法二
            //var t = proxy.CreateInterfaceProxyWithTarget<ITest>(new Test(), new IInterceptor[] { new TestInterceptor("2") });
            //t.arg = "3";
            //t.GetUser();
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
