using System;
using Cicada;
using Cicada.Boot.Service;
using Cicada.Log;
using Cicada.Rpc.Server;
using Im.Rpc.Redis;


namespace LingYi.RedisRpc
{
    public class Service : IService
    {
        private readonly IRpcServer _rpcServer;
        private static readonly ILog Log = CicadaFacade.CreateLog<Service>();
        private readonly string Info = "LingYi.RedisRpc Server {0} Status :{1}";

        public Service(IRpcServer rpcServer)
        {
            _rpcServer = rpcServer;
        }
        /// <summary>
        /// 当服务启动时执行此方法
        /// </summary>
        public void Start()
        {
            try
            {
                _rpcServer.Run<RedisService.Iface>();
                Log.Info(Info, "Run", "Successful");
            }
            catch (Exception ex)
            {
                Log.Info(Info, "Stop Error", ex.Message);
            }
        }

        /// <summary>
        /// 当服务停止时执行此方法
        /// </summary>
        public void Stop()
        {
            try
            {
                _rpcServer.Close();
                Log.Info(Info, "Stop", "Successful");
            }
            catch (Exception ex)
            {
                Log.Info(Info, "Stop Error", ex.Message);
            }
        }
    }
}
