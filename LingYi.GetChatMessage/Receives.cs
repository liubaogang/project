namespace LingYi.GetChatMessage
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Cicada.DI;
    using Cicada.Mq;
    using Newtonsoft.Json;
    using LingYi.DataTransfer.Models.Message;
    using LingYi.GetChatMessage.IServer;
    using System.Xml;
    using Cicada;

    public class Receives : IReceiver<CustomParams>
    {
        private readonly ISender _rabbitDB;
        public Receives(ISender rabbitDB)
        {
            _rabbitDB = rabbitDB;
        }
        public void Receive(CustomParams XmlMessage)
        {
            if (XmlMessage.ParamsData.ToString().IndexOf("thread") < 0)
            {
                var parent = Task.Factory.StartNew(() =>
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(XmlMessage.ParamsData.ToString());
                    XmlNode msgXml = xmlDoc.SelectSingleNode("message");
                    if (msgXml.Attributes["from"].Value.IndexOf("admin") < 0)
                    {
                        var XmlBody = msgXml.SelectSingleNode("body").InnerText;
                        var Message = JsonConvert.DeserializeObject<tbl_Message>(XmlBody);
                        Task.Factory.StartNew(() =>
                        {
                            //if (Message.MsgBody.Where(w => w.Key.IndexOf("CHAT") >= 0).Count() > 0)
                            //{
                            //    //1:保存消息记录
                            //    _rabbitDB.Send("lingyi.setsavemessage", Message);
                            //}
                        });
                        Task.Factory.StartNew(() =>
                        {
                            //if (Message.MsgBody.Where(w => w.Key.IndexOf("DATA") >= 0).Count() > 0)
                            //{
                            //    //2:业务数据处理
                            //    _rabbitDB.Send("lingyi.setbussmessage", Message);
                            //}
                        });
                        Task.Factory.StartNew(() =>
                        {
                            //3:发送消息通知
                            var action = CicadaFacade.Container.Resolve<IAction>(Message.MsgType.ToUpper());
                            action.Execute(Message);
                        });
                    }
                });
                parent.Wait();
                Console.WriteLine("LingYi.GetChatMessage:" + DateTime.Now.ToString());
            }            
        }
    }

    public class CustomParams
    {
        /// <summary>
        /// 管道类型
        /// </summary>
        public string ParamsType { get; set; }
        /// <summary>
        /// 原数据源
        /// </summary>
        public object ParamsData { get; set; }
    }
}
