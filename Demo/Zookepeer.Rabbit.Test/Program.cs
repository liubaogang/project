using org.apache.zookeeper;
using Rabbit.Zookeeper;
using Rabbit.Zookeeper.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zookepeer.Rabbit.Test
{
    class Program
    {
        static void Main(string[] args)
        {
           
            IZookeeperClient client = new ZookeeperClient(new ZookeeperClientOptions("192.168.1.167:2181")
            {
                BasePath = "/", //default value
                ConnectionTimeout = TimeSpan.FromSeconds(10), //default value
                SessionTimeout = TimeSpan.FromSeconds(20), //default value
                OperatingTimeout = TimeSpan.FromSeconds(60), //default value
                ReadOnly = false, //default value
                SessionId = 0, //default value
                SessionPasswd = null ,//default value
                EnableEphemeralNodeRestore = true //default value
            });
            

            
            client.SubscribeChildrenChange("/microsoft", (ct, args2) =>
            {
                IEnumerable<string> currentChildrens = args2.CurrentChildrens;
                string path = args2.Path;
                Console.WriteLine("SubscribeChildrenChange:" + path);
                Watcher.Event.EventType eventType = args2.Type;
                return Task.CompletedTask;
            });

            client.SubscribeDataChange("/year", (ct, args1) =>
            {
                
                IEnumerable<byte> currentData = args1.CurrentData;
                string path = args1.Path;
                Console.WriteLine("SubscribeDataChange:" + path+"*****"+args1.Type.ToString()+":"+DateTime.Now.ToString());
                Watcher.Event.EventType eventType = args1.Type;
                return Task.CompletedTask;
            });

            var data = Encoding.UTF8.GetBytes("2016");

            //Fast create temporary nodes
             client.CreateEphemeralAsync("/year", data);
             client.CreateEphemeralAsync("/year", data, ZooDefs.Ids.OPEN_ACL_UNSAFE);

            //Fast create permanent nodes
             client.CreatePersistentAsync("/year", data);
             client.CreatePersistentAsync("/year", data, ZooDefs.Ids.OPEN_ACL_UNSAFE);

            //Full call
             client.CreateAsync("/year", data, ZooDefs.Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT_SEQUENTIAL);

            //Recursively created
             client.CreateRecursiveAsync("/microsoft/netcore/aspnet", data);
            Console.WriteLine("start .....");
            while (true)
            {
                Console.WriteLine("请输入内容！");
                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            client.SetDataAsync("/microsoft", Encoding.UTF8.GetBytes("2018"));
                        };break;
                    case "2":
                        {
                            var abc = client.SetDataAsync("/year", Encoding.UTF8.GetBytes("2018"));
                        }; break;
                }
            
            }
            Console.ReadKey();
        }
    }
}
