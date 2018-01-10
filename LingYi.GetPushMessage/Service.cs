using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cicada;
using Cicada.DI;
using Cicada.Boot.Service;
using Cicada.Log;
using LingYi.GetPushMessage.Server;
using LingYi.GetPushMessage.IServer;

namespace LingYi.GetPushMessage
{
    public class Service : IService
    {
        private readonly IMessageProc _Process;
        private readonly IMessagePush _ApplePush;
        private readonly IMessagePush _XiaoMiPush;
        private readonly IMessagePush _HuaWeiPush;
        private readonly IMessagePush _FeedBackCheck;
        private static readonly ILog Log = CicadaFacade.CreateLog<Service>();
        private readonly string Info = "Message Push Server {0} Status :{1}";

        public Service()
        {
            _Process= CicadaFacade.Container.Resolve<IMessageProc>();
            _ApplePush = CicadaFacade.Container.Resolve<IMessagePush>("ApplePush");
            _XiaoMiPush = CicadaFacade.Container.Resolve<IMessagePush>("XiaoMiPush");
            _HuaWeiPush = CicadaFacade.Container.Resolve<IMessagePush>("HuaWeiPush");
            //_FeedBackCheck= CicadaFacade.Container.Resolve<IMessagePush>("FeedBackCheck");
        }
        /// <summary>
        /// 当服务启动时执行此方法
        /// </summary>
        public void Start()
        {
            try
            {
                _ApplePush.Begin();
                _XiaoMiPush.Begin();
                _HuaWeiPush.Begin();
                _Process.Begin();
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
                _Process.End();
                Log.Info("1:_Process.End() Stop Successful");
                _ApplePush.End();
                Log.Info("2:_ApplePush.End() Stop Successful");
                _XiaoMiPush.End();
                Log.Info("3:_XiaoMiPush.End() Stop Successful");
                _HuaWeiPush.End();
                Log.Info("4:_HuaWeiPush.End() Stop Successful");
                Log.Info(Info, "Stop", "Successful");
            }
            catch(Exception ex)
            {
                Log.Error(Info, "Stop Error", ex.Message);
            }
        }
    }
}
