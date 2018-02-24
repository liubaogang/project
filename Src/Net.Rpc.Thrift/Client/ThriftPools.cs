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
        public int MinCount { get; set; } = 3;

        private object objectLock = new object();
        private readonly List<ThriftClient> RpcClients = new List<ThriftClient>();
        public ThriftPools()
        {
            if (RpcClients.Count == 0)
            {
                for (int i = 0; i < MinCount;)
                {
                    if (CreateClient() != null)
                    {
                        i++;
                    }
                    else
                    {
                        if (i > 0)
                            i--;
                    }
                }
            }
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Recalculation:
                    var _RpcClients = RpcClients.Where(w => w.IsUse == false);
                    if (RpcClients.Count < MaxCount && _RpcClients.Count() < MinCount)
                    {
                        if (CreateClient() == null)
                            goto Recalculation;
                    }
                    Thread.Sleep(500);
                }
            });
        }
        public ThriftClient GetRpcClient()
        {
            lock (objectLock)
            {
                var RpcClient = RpcClients.Where(w => w.IsUse == false).ToList()[0];
                RpcClient.IsUse = true;
                Console.WriteLine("正在使用第{0}几个链接", RpcClients.IndexOf(RpcClient));
                return RpcClient;
            }
        }
        

        private ThriftClient CreateClient()
        {
            TSocket socket = new TSocket("127.0.0.1", 9527, 3000);
            try
            {
                new Action(socket.Open).TryDo(2, 500, new object[0]);
                var thriftClient = new ThriftClient(socket);
                RpcClients.Add(thriftClient);
                return thriftClient;
            }
            catch
            {
                //日 志
                return null;
            }
        }
    }
}
