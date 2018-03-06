using Net.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Transport;
using Net.Rpc.Thrift.Endpoint;

namespace Net.Rpc.Thrift.Client
{
    internal class ThriftPools : IThriftPools
    {
        public int MaxCount { get; set; }
        public int MinCount { get; set; }
        private object objectLock = new object();
        private ClientEndpointInfo _endPointInfo;
        private  List<ThriftClient> RpcClients = new List<ThriftClient>();
        public void DataInit(ClientEndpointInfo endPointInfo)
        {
            MinCount = 5;
            MaxCount = endPointInfo.ClientMaxCollections;
            _endPointInfo = endPointInfo;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    //1:移除池中不可用链接
                    var _Availables = RpcClients.Where(w => w.IsAvailable == false);
                    foreach (var item in _Availables.ToList())
                        RpcClients.Remove(item);
                    //2:检查池中可用连接数
                    var _RpcClients = RpcClients.Where(w => w.IsUse == false && w.IsAvailable == true);
                    var _ObjArg = new object[] { RpcClients.Count, _RpcClients.Count() };
                    Console.WriteLine("共计连接数为 {0},空闲链接数为 {1}", _ObjArg);
                    if (_RpcClients.Count() < MinCount && RpcClients.Count < MaxCount)
                        CreateClient();
                    Thread.Sleep(500);
                }
            });
        }
        //public ThriftPools()
        //{
        //    Task.Factory.StartNew(() =>
        //    {
        //        while (true)
        //        {
        //            //1:移除池中不可用链接
        //            var _Availables = RpcClients.Where(w => w.IsAvailable == false);
        //            foreach (var item in _Availables.ToList())
        //                RpcClients.Remove(item);
        //            //2:检查池中可用连接数
        //            var _RpcClients = RpcClients.Where(w => w.IsUse == false && w.IsAvailable == true);
        //            var _ObjArg = new object[] { RpcClients.Count, _RpcClients.Count() };
        //            Console.WriteLine("共计连接数为 {0},空闲链接数为 {1}", _ObjArg);
        //            if (_RpcClients.Count() < MinCount && RpcClients.Count < MaxCount)
        //                CreateClient();
        //            Thread.Sleep(500);
        //        }
        //    });
        //}
        public ThriftClient GetRpcClient()
        {
            lock (objectLock)
            {
                ReGetRpcClient:
                if (RpcClients.Count > 0)
                {
                    var RpcClient = RpcClients.Where(w => w.IsUse == false).Where(w => w.IsAvailable == true).FirstOrDefault();
                    if (RpcClient == null)
                    {
                        if (RpcClients.Count < MaxCount)
                            CreateClient();
                        goto ReGetRpcClient;
                    }
                    RpcClient.IsUse = true;
                    Console.WriteLine("正在使用第{0}个链接", RpcClients.IndexOf(RpcClient) + 1);
                    var FreeCount = RpcClients.Where(w => w.IsUse == false).Where(w => w.IsAvailable == true);
                    Console.WriteLine("当前空闲链接数为{0}", FreeCount.Count());
                    return RpcClient;
                }
                throw new TimeoutException("没有可用的链接，发生超时异常！");
            }
        }


        private void CreateClient()
        {
            TSocket socket = new TSocket("172.18.104.151", 9527, 3000);
            try
            {
                new Action(socket.Open).TryDo(2, 500, new object[0]);
                RpcClients.Add(new ThriftClient(socket, _endPointInfo.ClientType));
            }
            catch (Exception ex)
            {
                var log = ex.Message;
            }
        }
    }
}
