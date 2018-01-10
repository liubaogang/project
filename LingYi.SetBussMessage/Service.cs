using System;
using Cicada;
using Cicada.Log;
using Cicada.Mq;
using Cicada.Boot.Service;

namespace LingYi.SetBussMessage
{
    public class Service : IService
    {
        private readonly IReceiverManager _receiverManager;
        private static readonly ILog Log = CicadaFacade.CreateLog<Service>();
        private readonly string Info = "RabbitMq BussMessage Server {0} Status :{1}";
        public Service(IReceiverManager receiverManager)
        {
            _receiverManager = receiverManager;
        }
        /// <summary>
        /// 当服务启动时执行此方法
        /// </summary>
        public void Start()
        {
            try
            {
                _receiverManager.Run();
                Log.Info(Info, "Run", "Successful");
            }
            catch(Exception ex)
            {
                Log.Error(Info, "Start Error", ex.Message);
            }
        }

        /// <summary>
        /// 当服务停止时执行此方法
        /// </summary>
        public void Stop()
        {
            try
            {
                _receiverManager.Close();
                Log.Info(Info, "Stop", "Successful");
            }
            catch(Exception ex)
            {
                Log.Error(Info, "Stop Error", ex.Message);
            }
        }
    }
}
