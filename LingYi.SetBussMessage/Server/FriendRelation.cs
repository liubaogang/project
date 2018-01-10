using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cicada.DI;
using Newtonsoft.Json;
using LingYi.SetBussMessage.IServer;
using LingYi.DataTransfer.Models.Message;
using Im.Rpc.Redis;

namespace LingYi.SetBussMessage.Server
{
    [ComponentAttribute("FRIENDRELATION")]
    public class FriendRelation : BussBase, IAction
    {
        private readonly IXMPP _xmppOpenfire;
        private readonly RedisService.Iface _redisDB;
        public FriendRelation(IXMPP xmppOpenfire, RedisService.Iface redisDB)
        {
            _redisDB = redisDB;
            _xmppOpenfire = xmppOpenfire;
        }
        public void Execute(tbl_Message Message, string Key)
        {
            //var GetData = Message.MsgBody[Key];
            //switch (Key.Split('-')[2])
            //{
            //    case "REQUEST":
            //        {
            //            var Relation = JsonConvert.DeserializeObject<tbl_FriendRelation>(GetData);
            //            Relation.RelationID = GetDatePrefix + Guid.NewGuid().ToPureString();
            //            Relation.FromUserID = Message.MsgTo;
            //            Relation.ToUserID = Message.MsgFrom;
            //            Relation.RelationStatus = 0;
            //            GetData = JsonConvert.SerializeObject(Relation);
            //            if (_redisDB.FriendRelationIns(GetData))
            //            {
            //                Message.MsgBody[Key] = GetData;
            //                var jid = _xmppOpenfire.GetJid(Message.MsgTo, "PC");
            //                _xmppOpenfire.SendMessage(jid, Message);
            //                jid = _xmppOpenfire.GetJid(Message.MsgTo, "MOBILE");
            //                _xmppOpenfire.SendMessage(jid, Message);
            //            }
            //        }; break;
            //    case "AGREE":
            //        {
            //            var Relation = JsonConvert.DeserializeObject<tbl_FriendRelation>(GetData);
            //            Relation.RelationStatus = 1;
            //            //回答自己的添加好友同意请求
            //            GetData = JsonConvert.SerializeObject(Relation);
            //            if (_redisDB.FriendRelationUpd(GetData))
            //            {
            //                Message.MsgBody[Key] = GetData;
            //                var jid = _xmppOpenfire.GetJid(Message.MsgFrom, "PC");
            //                _xmppOpenfire.SendMessage(jid, Message);
            //                jid = _xmppOpenfire.GetJid(Message.MsgFrom, "MOBILE");
            //                _xmppOpenfire.SendMessage(jid, Message);
            //            }
            //            //给对方的两个设备端发送消息
            //            Relation.RelationID = GetDatePrefix + Guid.NewGuid().ToPureString();
            //            Relation.FromUserID = Message.MsgTo;
            //            Relation.ToUserID = Message.MsgFrom;
            //            GetData = JsonConvert.SerializeObject(Relation);
            //            if (_redisDB.FriendRelationIns(GetData))
            //            {
            //                Message.MsgFrom = Relation.FromUserID;
            //                Message.MsgTo = Relation.ToUserID;
            //                Message.MsgBody[Key] = GetData;
            //                var jid = _xmppOpenfire.GetJid(Message.MsgFrom, "PC");
            //                _xmppOpenfire.SendMessage(jid, Message);
            //                jid = _xmppOpenfire.GetJid(Message.MsgFrom, "MOBILE");
            //                _xmppOpenfire.SendMessage(jid, Message);
            //            }
            //        }; break;
            //    case "REFUSE":
            //        {
            //            var Relation = JsonConvert.DeserializeObject<tbl_FriendRelation>(GetData);
            //            Relation.RelationStatus = 2;
            //            //回答自己的添加好友同意请求
            //            GetData = JsonConvert.SerializeObject(Relation);
            //            if (_redisDB.FriendRelationUpd(GetData))
            //            {
            //                Message.MsgBody[Key] = GetData;
            //                var jid = _xmppOpenfire.GetJid(Message.MsgFrom, "PC");
            //                _xmppOpenfire.SendMessage(jid, Message);
            //                jid = _xmppOpenfire.GetJid(Message.MsgFrom, "MOBILE");
            //                _xmppOpenfire.SendMessage(jid, Message);
            //            }
            //        }; break;
            //}
        }
    }
}
