using System;
using Cicada;
using Cicada.Log;
using Cicada.Mq;
using Cicada.Boot.Service;
using LingYi.GetChatMessage.IServer;

namespace LingYi.GetChatMessage
{
    public class Service : IService
    {
        private readonly IXMPP _xmppOpenfire;
        private readonly IReceiverManager _receiverManager;
        private static readonly ILog Log = CicadaFacade.CreateLog<Service>();
        public Service(IXMPP xmppOpenfire,IReceiverManager receiverManager)
        {
            _xmppOpenfire = xmppOpenfire;
            _receiverManager = receiverManager;
        }
        /// <summary>
        /// 当服务启动时执行此方法
        /// </summary>
        public void Start()
        {
            _xmppOpenfire.XmppConnection();
            _receiverManager.Run();
            Log.Info("RabbitMq Server Run at :" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        /// 当服务停止时执行此方法
        /// </summary>
        public void Stop()
        {
            _receiverManager.Close();
            _xmppOpenfire.XmppClosed();
            Log.Info("RabbitMq Server Stop at :" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
