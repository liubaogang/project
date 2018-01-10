namespace LingYi.RedisRpc.Message
{
    using System;
    using Im.Rpc.Redis;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using LingYi.NetToolClass.Redis;
    using LingYi.DataTransfer.Models.Message;
    using StackExchange.Redis;
    using Cicada.Configuration;
    using Cicada.DI;
    using Cicada;

    public class Im_RedisService : RedisService.Iface
    {
        private string UserPrefixKey = "im.user.";
        private string GroupPrefixKey = "im.friendgroup.";
        private string RelationPrefixKey = "im.friendlist.";


        public Im_RedisService()
        {
            var config = CicadaFacade.Container.Resolve<IConfigurationDataRespository>();
            RedisDB._redisIP = config.Get("Cicada.Cache.Redis.Server");
        }

        private string GetDatePart(string id)
        {
            return id.Substring(0, 6);
        }
        public bool SetUnreadNumber(string userid, int count)
        {
            return RedisDB.HashSet<int>("UnreadMsgNumber", userid, count);
        }
        public bool FriendGroupDel(string friendgroup)
        {
            var FriendGroup = JsonConvert.DeserializeObject<tbl_FriendGroups>(friendgroup);
            var RedisKey = GroupPrefixKey + GetDatePart(FriendGroup.GroupID);
            if (RedisDB.HashDelete(RedisKey, FriendGroup.GroupID))
            {
                var GroupIds = new List<string>();
                if (RedisDB.HashExists(UserPrefixKey + FriendGroup.GroupUserID, "FriendGroup"))
                {
                    GroupIds = RedisDB.HashGet<List<string>>(UserPrefixKey + FriendGroup.GroupUserID, "FriendGroup");
                }
                GroupIds.Remove(FriendGroup.GroupID);
                return RedisDB.HashSet(UserPrefixKey + FriendGroup.GroupUserID, "FriendGroup", GroupIds);
            }
            return false;
        }

        public bool FriendGroupIns(string friendgroup)
        {
            var FriendGroup = JsonConvert.DeserializeObject<tbl_FriendGroups>(friendgroup);
            var RedisKey = GroupPrefixKey + GetDatePart(FriendGroup.GroupID);
            if (RedisDB.HashSet<tbl_FriendGroups>(RedisKey, FriendGroup.GroupID, FriendGroup))
            {
                var GroupIds = new List<string>();
                if (RedisDB.HashExists(UserPrefixKey + FriendGroup.GroupUserID, "FriendGroup"))
                {
                    GroupIds = RedisDB.HashGet<List<string>>(UserPrefixKey + FriendGroup.GroupUserID, "FriendGroup");
                }
                GroupIds.Add(FriendGroup.GroupID);
                return RedisDB.HashSet(UserPrefixKey + FriendGroup.GroupUserID, "FriendGroup", GroupIds);
            }
            return false;
        }

        public bool FriendGroupUpd(string friendgroup)
        {
            var FriendGroup = JsonConvert.DeserializeObject<tbl_FriendGroups>(friendgroup);
            var RedisKey = GroupPrefixKey + GetDatePart(FriendGroup.GroupID);
            if (RedisDB.HashExists(RedisKey, FriendGroup.GroupID))
            {
                return RedisDB.HashSet<tbl_FriendGroups>(RedisKey, FriendGroup.GroupID, FriendGroup);
            }
            return false;
        }

        public string GetFriendGroupList(string userid)
        {
            var FriendGroup = RedisDB.HashGet<List<string>>(UserPrefixKey + userid, "FriendGroup");
            if (null != FriendGroup && FriendGroup.Count > 0)
            {
                var FriendGroupList = new List<tbl_FriendGroups>();
                foreach (var GroupItem in FriendGroup)
                {
                    var RedisKey = GroupPrefixKey + GetDatePart(GroupItem);
                    FriendGroupList.Add(RedisDB.HashGet<tbl_FriendGroups>(RedisKey, GroupItem));
                }
                return JsonConvert.SerializeObject(FriendGroupList);
            }
            return string.Empty;
        }

        public bool FriendRelationDel(string friendrelation)
        {
            var FriendRelation = JsonConvert.DeserializeObject<tbl_FriendRelation>(friendrelation);
            var RedisKey = RelationPrefixKey + GetDatePart(FriendRelation.RelationID);
            if (RedisDB.HashDelete(RedisKey, FriendRelation.RelationID))
            {
                var FriendIds = new List<string>();
                if (RedisDB.HashExists(UserPrefixKey + FriendRelation.FromUserID, "FriendList"))
                {
                    FriendIds = RedisDB.HashGet<List<string>>(UserPrefixKey + FriendRelation.FromUserID, "FriendList");
                }
                FriendIds.Remove(FriendRelation.RelationID);
                return RedisDB.HashSet(UserPrefixKey + FriendRelation.FromUserID, "FriendList", FriendIds);
            }
            return false;
        }

        public bool FriendRelationIns(string friendrelation)
        {
            var FriendRelation = JsonConvert.DeserializeObject<tbl_FriendRelation>(friendrelation);
            var RedisKey = RelationPrefixKey + GetDatePart(FriendRelation.RelationID);
            if (RedisDB.HashSet<tbl_FriendRelation>(RedisKey, FriendRelation.RelationID, FriendRelation))
            {
                var FriendIds = new List<string>();
                if (RedisDB.HashExists(UserPrefixKey + FriendRelation.FromUserID, "FriendList"))
                {
                    FriendIds = RedisDB.HashGet<List<string>>(UserPrefixKey + FriendRelation.FromUserID, "FriendList");
                }
                FriendIds.Add(FriendRelation.ToUserID);
                return RedisDB.HashSet(UserPrefixKey + FriendRelation.FromUserID, "FriendList", FriendIds);
            }
            return false;
        }

        public bool FriendRelationUpd(string friendrelation)
        {
            var FriendRelation = JsonConvert.DeserializeObject<tbl_FriendRelation>(friendrelation);
            var RedisKey = RelationPrefixKey + GetDatePart(FriendRelation.RelationID);
            if (RedisDB.HashExists(RedisKey, FriendRelation.RelationID))
            {
                return RedisDB.HashSet<tbl_FriendRelation>(RedisKey, FriendRelation.RelationID, FriendRelation);
            }
            return false;
        }

        public string GetFriendRelationList(string userid)
        {
            var FriendList = RedisDB.HashGet<List<string>>(UserPrefixKey + userid, "FriendList");
            if (null != FriendList && FriendList.Count > 0)
            {
                var FriendRelationList = new List<tbl_FriendRelation>();
                foreach (var RelationItem in FriendList)
                {
                    var RedisKey = RelationPrefixKey + GetDatePart(RelationItem);
                    FriendRelationList.Add(RedisDB.HashGet<tbl_FriendRelation>(RedisKey, RelationItem));
                }
                return JsonConvert.SerializeObject(FriendRelationList);
            }
            return string.Empty;
        }
    }
}
