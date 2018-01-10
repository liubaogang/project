using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cicada.DI;
using Newtonsoft.Json;
using Im.Rpc.Redis;
using LingYi.SetBussMessage.IServer;
using LingYi.DataTransfer.Models.Message;

namespace LingYi.SetBussMessage.Server
{
    [ComponentAttribute("FRIENDGROUP")]
    public class FriendGroup : BussBase, IAction
    {
        private readonly IXMPP _xmppOpenfire;
        private readonly RedisService.Iface _redisDB;
        public FriendGroup(IXMPP xmppOpenfire, RedisService.Iface redisDB)
        {
            _redisDB = redisDB;
            _xmppOpenfire = xmppOpenfire;
        }
        public void Execute(tbl_Message Message, string Key)
        {
            //var GetData = Message.MsgBody[Key];
            //switch (Key.Split('-')[2])
            //{
            //    case "INSERT":
            //        {
            //            var Group = JsonConvert.DeserializeObject<tbl_FriendGroups>(GetData);
            //            Group.GroupID = GetDatePrefix + Guid.NewGuid().ToPureString();
            //            GetData = JsonConvert.SerializeObject(Group);
            //            if (_redisDB.FriendGroupIns(GetData))
            //                Message.MsgBody[Key] = GetData;
            //        };break;
            //    case "UPDATE":
            //        {
            //            _redisDB.FriendGroupUpd(GetData);
            //        };break;
            //    case "DELETE":
            //        {
            //            _redisDB.FriendGroupDel(GetData);
            //        };break;
            //}
            //var jid = _xmppOpenfire.GetJid(Message.MsgFrom, "PC");
            //_xmppOpenfire.SendMessage(jid, Message);
            //jid = _xmppOpenfire.GetJid(Message.MsgFrom, "MOBILE");
            //_xmppOpenfire.SendMessage(jid, Message);
        }
    }
}
