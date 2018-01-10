namespace LingYi.SetSaveMessage
{
    using Cicada;
    using Cicada.DI;
    using Cicada.Mq;
    using Im.Rpc.Mysql;
    using System;
    using System.Text;

    public class Receives : IReceiver<string>
    {
        private readonly ISender _rabbitMqDB;
        private readonly MysqlService.Iface _mysqlDB;        
        public Receives(MysqlService.Iface mysqlDB)
        {
            _mysqlDB = mysqlDB;
            _rabbitMqDB = CicadaFacade.Container.Resolve<ISender>();
        }
        public void Receive(string Message)
        {
            byte[] bytes = Encoding.Default.GetBytes(Message);
            _mysqlDB.MessageInsert(Convert.ToBase64String(bytes));
            if (Message.Contains("<code>2000</code>"))
            {
                _rabbitMqDB.Send("oamessagereceive", Message);
            }
        }
    }
}
