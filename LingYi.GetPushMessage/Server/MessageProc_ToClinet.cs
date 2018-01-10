namespace LingYi.GetPushMessage.Server
{
    using System;
    using Cicada.DI;
    using System.Threading;
    using System.Threading.Tasks;
    using LingYi.NetToolClass.Redis;
    using LingYi.GetPushMessage.IServer;
    using Cicada;
    using System.Xml;
    using Cicada.Configuration;
    using Newtonsoft.Json;

    [ComponentAttribute(Lifetime = Lifetime.Singleton)]
    public class MessageProc_ToClinet : IMessageProc
    {
        private bool IsStop = false;
        private const int TaskCount = 3;
        private Task[] ParnetTask = new Task[TaskCount];
        private readonly UserInfoService.Iface _userService;

        public MessageProc_ToClinet(UserInfoService.Iface userService)
        {
            _userService = userService;
            var cicada = CicadaFacade.Container.Resolve<IConfigurationDataRespository>();
            RedisDB._redisIP = cicada.Get("Cicada.Cache.Redis.Server");
        }
        public void Begin()
        {
            for (int i = 0; i < TaskCount; i++)
            {
                var task = Task.Factory.StartNew(() =>
                 {
                     do
                     {
                         var Message = string.Empty;
                         try
                         {
                             Message = RedisDB.ListRightPop<string>("OffLinePush");
                             if (null != Message)
                             {
                                 if (Message.IndexOf("<received") < 0)
                                 {
                                     XmlDocument xmlDoc = new XmlDocument();
                                     xmlDoc.LoadXml(Message);
                                     XmlNode msgXml = xmlDoc.SelectSingleNode("message");
                                     string ToUserID = msgXml.Attributes["to"].Value;
                                     XmlNode Body = msgXml.SelectSingleNode("body");
                                     if (msgXml.Attributes["ToUser"] != null)
                                         ToUserID = msgXml.Attributes["ToUser"].Value;
                                     string BodyContent = Body.InnerText;
                                     ToUserID = ToUserID.Split('@')[0];
                                     PushMessage(ToUserID, BodyContent);
                                 }
                                 
                             }
                             else
                             {
                                 Thread.Sleep(1000 * 10);
                             }
                         }
                         catch(Exception ex)
                         {
                             RedisDB.ListLeftPush("OfflineMsgException", "[" + ex.Message + "]-" + Message);
                         }
                     }
                     while (!IsStop);
                 });
                ParnetTask[i] = task;
            }
        }

        public void PushMessage(string toUserID, string bodyContent)
        {
            var UserInfo = _userService.GetDeviceToken(toUserID);
            if (UserInfo.Status == GetDeviceTokenStatus.Success)
            {
                if (!string.IsNullOrWhiteSpace(UserInfo.DeviceToken))
                {
                    IMessagePush action = null;
                    switch (UserInfo.DeviceType)
                    {
                        case DeviceTypeEnum.iOS:
                            {
                                action = CicadaFacade.Container.Resolve<IMessagePush>("ApplePush");
                                int UnreadMsgNumber = 1;
                                if (RedisDB.HashExists("UnreadMsgNumber", UserInfo.UserBaseId))
                                    UnreadMsgNumber += RedisDB.HashGet<int>("UnreadMsgNumber", UserInfo.UserBaseId);
                                RedisDB.HashSet<int>("UnreadMsgNumber", UserInfo.UserBaseId, UnreadMsgNumber);
                                bodyContent = JsonConvert.SerializeObject(new
                                {
                                    aps = new
                                    {
                                        alert = "您收到一条消息",
                                        badge = UnreadMsgNumber,
                                        sound = "default"
                                    }
                                });
                            }; break;
                        case DeviceTypeEnum.Android:
                            {
                                var deviceNumber = UserInfo.DeviceNumber.ToLower();
                                if (deviceNumber.IndexOf("huawei") >= 0)
                                {
                                    action = CicadaFacade.Container.Resolve<IMessagePush>("HuaWeiPush");
                                }
                                if (deviceNumber.IndexOf("xiaomi") >= 0)
                                {
                                    action = CicadaFacade.Container.Resolve<IMessagePush>("XiaoMiPush");
                                }
                            }; break;
                    }
                    if (action != null)
                        action.Push(UserInfo.DeviceToken, bodyContent);
                }
            }
        }
        public void End()
        {
            IsStop = true;
            Task.WaitAll(ParnetTask);
        }
    }
}
