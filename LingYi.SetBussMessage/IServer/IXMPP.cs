using Sharp.Xmpp;
using LingYi.DataTransfer.Models.Message;

namespace LingYi.SetBussMessage.IServer
{
    public interface IXMPP
    {
        void XmppConnection(Jid to = null, tbl_Message message = null);
        bool SendMessage(Jid to, tbl_Message message);
        Jid GetJid(string userid, string resource);
        void XmppClosed();
    }
}
