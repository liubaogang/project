using Net.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Transport;

namespace Net.Rpc.Thrift.Client
{
    internal class ThriftPools : IThriftPools
    {
        public int MaxCount { get; set; } = 100;
        public int MinCount { get; set; } = 5;

        private object objectLock = new object();
        private readonly List<ThriftClient> RpcClients = new List<ThriftClient>();
        public ThriftPools()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {          
                    var _RpcClients = RpcClients.Where(w => w.IsUse == false);
                    var _ObjArg = new object[] { RpcClients.Count, _RpcClients.Count() };
                    Console.WriteLine("共计连接数为 {0},空闲链接数为 {1}", _ObjArg);
                    if (_RpcClients.Count() < MinCount)
                        CreateClient();                    
                    Thread.Sleep(500);                    
                }
            });
        }
        public ThriftClient GetRpcClient()
        {
            lock (objectLock)
            {
                var RpcClientList = RpcClients.Where(w => w.IsUse == false).ToList();
                if (RpcClientList.Count > 0)
                {
                    var RpcClient = RpcClientList[0];
                    RpcClient.IsUse = true;
                    Console.WriteLine("正在使用第 {0} 个链接", RpcClients.IndexOf(RpcClient) + 1);
                    Console.WriteLine("当前空闲链接数为 {0}", RpcClientList.Count - 1);
                    return RpcClient;
                }
                return null;
            }
        }
        

        private void CreateClient()
        {
            if (RpcClients.Count == MaxCount)
                throw new Exception("连接池数已经达到上限,请修改上限数！");
            TSocket socket = new TSocket("127.0.0.1", 9527, 3000);
            try
            {
                new Action(socket.Open).TryDo(2, 500, new object[0]);
                var thriftClient = new ThriftClient(socket);
                RpcClients.Add(thriftClient);
            }
            catch(Exception ex)
            {
                //日 志
            }
        }
    }
}
