using Cicada.Mq;
using Newtonsoft.Json;
using Cicada.Boot.Service;
using LingYi.DataTransfer.Models.Message;

namespace LingYi.GetSingleChat
{
    interface IAction
    {
        void Execute(string json);
    }

    class MessageReceive : IReceiver<string>
    {
        private readonly ISender _rabbitDB;

        public MessageReceive(ISender rabbitDB)
        {
            _rabbitDB = rabbitDB;
        }
        public void Receive(string jsonData)
        {
            var Im = JsonConvert.DeserializeObject<tbl_Message>(jsonData);
            foreach(var KeysDict in Im.MsgBody)
            {
                var KeysSplit = KeysDict.Key.Split('-');
                switch(KeysSplit[0])
                {
                    case "CHAT":
                        {
                            //1:保存消息记录
                            _rabbitDB.Send("Im-00001", jsonData);
                            //2:发送消息对方
                            _rabbitDB.Send("Im-00002", jsonData);
                        }; break;
                    case "DATA":
                        {
                            switch(KeysSplit[1])
                            {
                                default:
                                    {

                                    };break;
                            }
                        };break;
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ServiceApplication.Run();
        }
    }
}
