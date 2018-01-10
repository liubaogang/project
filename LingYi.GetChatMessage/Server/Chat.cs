using Cicada.DI;
using Newtonsoft.Json;
using LingYi.DataTransfer.Models.Message;
using LingYi.GetChatMessage.IServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingYi.GetChatMessage.Server
{
    [ComponentAttribute("CHAT")]
    public class Chat : IAction
    {
        private readonly IXMPP _xmppOpenfire;
        public Chat(IXMPP xmppOpenfire)
        {
            _xmppOpenfire = xmppOpenfire;
        }
        public void Execute(tbl_Message Message)
        {
            try
            {
                if (Message.MsgBody["RESOURCE"].ToUpper() == "MOBILE")
                {
                    var jid = _xmppOpenfire.GetJid(Message.MsgFrom, "PC");
                    _xmppOpenfire.SendMessage(jid, Message);
                    jid = _xmppOpenfire.GetJid(Message.MsgTo, "PC");
                    _xmppOpenfire.SendMessage(jid, Message);
                }
                else
                {
                    var jid = _xmppOpenfire.GetJid(Message.MsgFrom, "MOBILE");
                    _xmppOpenfire.SendMessage(jid, Message);
                    jid = _xmppOpenfire.GetJid(Message.MsgTo, "MOBILE");
                    _xmppOpenfire.SendMessage(jid, Message);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
