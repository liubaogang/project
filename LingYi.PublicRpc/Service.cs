using System;
using Cicada;
using Cicada.Boot.Service;
using Cicada.Log;
using Cicada.Rpc.Server;
using Im.Rpc.Public;

namespace LingYi.PublicRpc
{
    public class Service : IService
    {
        private readonly IRpcServer _rpcServer;
        private static readonly ILog Log = CicadaFacade.CreateLog<Service>();
        private readonly string Info = "LingYi.PublicRpc Server {0} Status :{1}";
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
                _rpcServer.Run<PublicService.Iface>();
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
