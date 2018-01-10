using System;
using Cicada;
using Cicada.Log;
using Cicada.Mq;
using Cicada.Boot.Service;

namespace LingYi.GetSingleChat
{
    public class Service : IService
    {
        private readonly IReceiverManager _receiverManager;
        private static readonly ILog Log = CicadaFacade.CreateLog<Service>();
        public Service(IReceiverManager receiverManager)
        {
            _receiverManager = receiverManager;
        }
        /// <summary>
        /// 当服务启动时执行此方法
        /// </summary>
        public void Start()
        {
            _receiverManager.Run();
            Log.Info("Rpc Server Run at :" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        /// 当服务停止时执行此方法
        /// </summary>
        public void Stop()
        {
            _receiverManager.Close();
            Log.Info("Rpc Server Stop at :" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
