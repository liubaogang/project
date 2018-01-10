namespace LingYi.SetBussMessage
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Cicada.DI;
    using Cicada.Mq;
    using Newtonsoft.Json;
    using LingYi.DataTransfer.Models.Message;
    using IServer;

    public class Receives : IReceiver<tbl_Message>
    {
        private readonly ISender _rabbitDB;
        public Receives(ISender rabbitDB)
        {
            _rabbitDB = rabbitDB;
        }
        public void Receive(tbl_Message Message)
        {
            //var GetData= Message.MsgBody.Where(w => w.Key.IndexOf("DATA") >= 0).FirstOrDefault();
            //var KeyName = GetData.Key.Split('-');
            //var Action = Cicada.CicadaFacade.Container.Resolve<IAction>(KeyName[1].ToUpper());
            //if (Action != null)
            //    Action.Execute(Message, GetData.Key);
            //Console.WriteLine("LingYi.SetBussMessage:" + DateTime.Now.ToString());
        }
    }
}
