namespace LingYi.GetPushMessage.Server
{
    using System;
    using Cicada.DI;
    using PushSharp.Apple;
    using Cicada.Configuration;
    using Newtonsoft.Json.Linq;
    using LingYi.GetPushMessage.IServer;
    using System.Threading;
    using System.Threading.Tasks;

    [ComponentAttribute("FeedBackCheck", Lifetime = Lifetime.Singleton)]
    public class MessageCheck_FeedBack : IMessagePush
    {
        private Task task;
        private FeedbackService fbs;
        private bool IsStop = false;
        public void Begin()
        {
            var cicada = Cicada.CicadaFacade.Container.Resolve<IConfigurationDataRespository>();
            string p12Path = AppDomain.CurrentDomain.BaseDirectory + "PushDll";
            string ProductType = cicada.Get("Message.ApnsServer.Environment");
            var config = new ApnsConfiguration((ProductType == "Production"
                                                                ? ApnsConfiguration.ApnsServerEnvironment.Production
                                                                : ApnsConfiguration.ApnsServerEnvironment.Sandbox)
                                            , (ProductType == "Production"
                                                                ? p12Path += "\\Production.cer"
                                                                : p12Path += "\\Sandbox.cer")
                                            , "1");
            fbs = new FeedbackService(config);
            fbs.FeedbackReceived += (string deviceToken, DateTime timestamp) =>
            {
                Console.WriteLine("无效token：" + deviceToken + ":" + timestamp);
            };
            Push();
        }

        public void End()
        {
            IsStop = true;
            task.Wait();
        }

        public void Push(string deviceToken = null, string pushBody = null)
        {
            task = Task.Factory.StartNew(() =>
            {
                do
                {
                    if (fbs != null)
                    {
                        //fbs.Check();
                        Thread.Sleep(5000);
                    }
                } while (!IsStop);
            });
        }
    }
}
