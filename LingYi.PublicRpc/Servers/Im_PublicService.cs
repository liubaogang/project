namespace LingYi.PublicRpc.Servers
{
    using System;
    using Dapper;
    using Im.Rpc.Public;
    using Newtonsoft.Json;
    using LingYi.DataTransfer.Models.Message;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Im_PublicService : PublicService.Iface
    {
        public string GetGroupsInfoByOA(string GroupID)
        {
            using (var conn = MysqlDB.GetOpenConnection())
            {
                var GroupInfo = conn.QueryFirst<dynamic>(@"SELECT * FROM  tbl_GroupManage WHERE GroupID=@groupID;",
                                new { groupID = GroupID });
                var GroupProp = conn.Query<string>(@"SELECT PropertyVal FROM  tbl_GroupProperty 
                                                     WHERE PropertyGroupID=@groupID and PropertyKey in ('groupImage', 'groupDescribe')
                                                     ORDER BY PropertyVal DESC ;",
                                new { groupID = GroupID }).ToList();
                var GroupUser = conn.Query<string>(@"SELECT MemberID FROM  tbl_GroupMembers WHERE GroupID=@groupID and IsAgree=1 and IsAdminAgree=1;",
                                new { groupID = GroupID }).ToList();
                return JsonConvert.SerializeObject(new
                {
                    GroupID = GroupID,
                    GroupName = GroupInfo.GroupName,
                    GroupImage = GroupProp.Count > 0 ? GroupProp[0] : "",
                    GroupDescribe = GroupProp.Count > 1 ? GroupProp[1] : "",
                    GroupMember = GroupUser,
                    GroupDate = GroupInfo.CreateDate
                });
            }
        }
    }
}
