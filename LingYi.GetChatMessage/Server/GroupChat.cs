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
    [ComponentAttribute("GROUPCHAT")]
    public class GroupChat : IAction
    {
        private readonly IXMPP _xmppOpenfire;
        private static List<string> _userList = new List<string>();
        private static readonly object _locker = new object();
        public GroupChat(IXMPP xmppOpenfire)
        {
            _xmppOpenfire = xmppOpenfire;
        }        
        public void Execute(tbl_Message Message)
        {
            var parent = Task.Factory.StartNew(() =>
            {
                var task1 = Task.Factory.StartNew(() =>
                {
                    //TODO 获得群成员列表
                });
                var task2 = Task.Factory.StartNew(() =>
                {
                    task1.Wait();
                    if (task1.IsCompleted)
                    {
                        while (_userList.Count > 0)
                        {
                            Message.MsgTo = GetUserItem();
                            SendMessage(Message);
                        }
                    }
                });
                var task3 = Task.Factory.StartNew(() =>
                {
                    task1.Wait();
                    if (task1.IsCompleted)
                    {
                        while (_userList.Count > 0)
                        {
                            Message.MsgTo = GetUserItem();
                            SendMessage(Message);
                        }
                    }
                });
                var task4 = Task.Factory.StartNew(() =>
                {
                    task1.Wait();
                    if (task1.IsCompleted)
                    {
                        while (_userList.Count > 0)
                        {
                            Message.MsgTo = GetUserItem();
                            SendMessage(Message);
                        }
                    }
                });
            });
            parent.Wait();
        }

        private string GetUserItem()
        {
            if (_userList.Count > 0)
            {
                lock (_locker)
                {
                    if (_userList.Count > 0)
                    {
                        var userID = _userList[0];
                        _userList.RemoveAt(0);
                        return userID;
                    }
                }
            }
            return string.Empty;
        }

        private void SendMessage(tbl_Message Message)
        {
            var jid = _xmppOpenfire.GetJid(Message.MsgTo, "PC");
            _xmppOpenfire.SendMessage(jid, Message);
            jid = _xmppOpenfire.GetJid(Message.MsgTo, "MOBILE");
            _xmppOpenfire.SendMessage(jid, Message);
        }
    }
}
