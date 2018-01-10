namespace LingYi.GetPushMessage.Server
{
    using System;
    using Cicada.DI;
    using PushSharp.Apple;
    using Cicada.Configuration;
    using Newtonsoft.Json.Linq;
    using LingYi.GetPushMessage.IServer;

    [ComponentAttribute("ApplePush", Lifetime = Lifetime.Singleton)]
    public class MessagePush_Apple : IMessagePush
    {
        /// <summary>
        /// 定义推送代理对象
        /// </summary>
        private ApnsServiceBroker apnsBroker;
        public void Begin()
        {
            Begin(null, null);
        }

        private void Begin(string deviceToken = null, string pushBody = null)
        {
            var cicada = Cicada.CicadaFacade.Container.Resolve<IConfigurationDataRespository>();
            string p12Path = AppDomain.CurrentDomain.BaseDirectory + "PushDll";
            string ProductType = cicada.Get("Message.ApnsServer.Environment");
            var config = new ApnsConfiguration((ProductType == "Production"
                                                                ? ApnsConfiguration.ApnsServerEnvironment.Production
                                                                : ApnsConfiguration.ApnsServerEnvironment.Sandbox)
                                            , (ProductType == "Production"
                                                                ? p12Path += "\\Production.p12"
                                                                : p12Path += "\\Sandbox.p12")
                                            , "1");
            apnsBroker = new ApnsServiceBroker(config);
            apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {
                aggregateEx.Handle(ex =>
                {
                    if (ex is ApnsNotificationException)
                    {
                        var notificationException = (ApnsNotificationException)ex;
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;
                        Console.WriteLine("Apple Notification Failed: ID={0}, Code={1}", apnsNotification.Identifier, statusCode);
                    }
                    else
                    {
                        Console.WriteLine("Apple Notification Failed for some unknown reason : {0}", ex.InnerException);
                    }
                    return true;
                });
            };
            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
                Console.WriteLine("Apple Notification Succeeded Sent!");
            };
            apnsBroker.Start();
            if (deviceToken != null && pushBody != null)
            {
                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = deviceToken,
                    Payload = JObject.Parse(pushBody)
                });
            }
        }

        /// <summary>
        /// 苹果离线推送
        /// </summary>
        /// <param name="pushBody"></param>
        public void Push(string deviceToken, string pushBody)
        {
            try
            {
                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = deviceToken,
                    Payload = JObject.Parse(pushBody)
                });
            }
            catch
            {
                Begin(deviceToken, pushBody);
            }
        }
        /// <summary>
        /// 停止推送服务
        /// </summary>
        public void End()
        {
            apnsBroker.Stop();
        }
    }
}
