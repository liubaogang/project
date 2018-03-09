using Net.Boot;
using Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift;
using Thrift.Protocol;
using Thrift.Transport;

namespace Net.TestRpc.Client
{
    class dddd
    {
        public dddd(string a)
        {
            Console.WriteLine(a);
        }
        public void test()
        {

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            
            //object[] arg = new object[] { "aaaa" };

            //var Client =Activator.CreateInstance(Type.GetType("ThriftCustomerService+Client"), null);
            //ThriftCustomerService.Client

            //var a = Type.GetType("ThriftCustomerService+Client");



            _Application.Run();

            /*
            TTransport transport = null;
            try
            {
                transport = new TSocket("127.0.0.1", 9527, 3000);
                // 协议要和服务端一致
                //TProtocol protocol = new TBinaryProtocol(transport);
                 TProtocol protocol = new TCompactProtocol(transport);
                // TProtocol protocol = new TJSONProtocol(transport);
                ThriftCustomerService.Client client = new ThriftCustomerService.Client(
                          protocol);
                transport.Open();
                var result = client.Add(new Customer());
                
            }
            catch (TTransportException e)
            { 

            }
            catch (TException e)
            {

            }
            finally
            {
                 
            }
            */
            
            var dbiface = ContainerSingleton.Instance.Resolve<ThriftCustomerService.Iface>();
            
            //int a= dbiface.Add(new Customer() {  Name="sdf", CustomerId=1});
            //Console.WriteLine("输出结果为："+a);
            //System.Threading.Thread.Sleep(5000);
            

            var ts = new List<Thread>();
            for (int i = 0; i < 1; i++)
            {
                ts.Add(new Thread(() =>
                {
                //Able:
                    for (int j = 0; j < 1; j++)
                    {
                        try
                        {
                            int a = dbiface.Add(new Customer() { Name = "sdf", CustomerId = 1 });
                            Console.WriteLine("输出结果为：" + a);
                            System.Threading.Thread.Sleep(1000);
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("*****************************************************"+ex.Message);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    //goto Able;
                }));
            }
            ts.ForEach(a => a.Start());
            ts.ForEach(a => a.Join());
            //Console.WriteLine("请按任意键，继续！");
            Console.ReadLine();



        }
    }
}
