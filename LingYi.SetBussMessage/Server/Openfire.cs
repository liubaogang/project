using System;
using Cicada.DI;
using Sharp.Xmpp;
using Sharp.Xmpp.Im;
using Newtonsoft.Json;
using Sharp.Xmpp.Client;
using LingYi.SetBussMessage.IServer;
using LingYi.DataTransfer.Models.Message;


namespace LingYi.SetBussMessage.Server
{
    [ComponentAttribute(Lifetime = Lifetime.Singleton)]
    public class Openfire : IXMPP
    {
        private int Port { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        public string HostName { get; set; }
        private XmppClient Client { get; set; }
        private int Times = 0;
        public Openfire()
        {
            Port = 5333;
            UserName = "admin";
            Password = "admin";
            HostName = "csxmpp.lingyi365.com";
        }
        public void XmppConnection(Jid to = null, tbl_Message message = null)
        {
            do
            {
                try
                {
                    Client = new XmppClient(HostName);
                    Client.Username = UserName;
                    Client.Password = Password;
                    Client.Port = Port;
                    Client.Connect(Guid.NewGuid().ToPureString());
                    if (null != Client && Client.Connected)
                    {
                        if (null != to & null != message)
                        {
                            SendMessage(to, message);
                            Times = 0; break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if ((Times++) > 4)
                        throw new Exception(ex.Message);
                }
            } while (Times > 0 && Times < 5);
        }
        public bool SendMessage(Jid to, tbl_Message message)
        {
            try
            {
                var body = JsonConvert.SerializeObject(message);
                Client.SendMessage(to, body, null, null, MessageType.Chat);
            }
            catch
            {
                Client = null;
                XmppConnection(to, message);
            }
            return true;
        }
        public Jid GetJid(string userid, string resource)
        {
            return new Jid(HostName, userid, resource);
        }

        public void XmppClosed()
        {
            try
            {
                if (null != Client)
                {
                    if (Client.Connected)
                        Client.Close();
                }
            }
            catch { }
        }
    }
}
