namespace LingYi.MysqlRpc.Servers
{
    using System;
    using Dapper;
    using Im.Rpc.Mysql;
    using Newtonsoft.Json;
    using LingYi.DataTransfer.Models.Message;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Im_MysqlService : MysqlService.Iface
    {
        public bool FileInsert(string filemanager)
        {
            var FileManager = JsonConvert.DeserializeObject<tbl_FileManager>(filemanager);
            using (var conn = MysqlDB.GetOpenConnection())
            {
                return conn.Execute(@"INSERT INTO tbl_ImFileManager
                                   (FileID, FileDirectory, FileDate)
                                   VALUES (@FileID,@FileDirectory,@FileDate)"
                             , FileManager) > 0;
            }
        }

        public string GetFileByID(string fileid)
        {
            using (var conn = MysqlDB.GetOpenConnection())
            {
                var Model = conn.QueryFirst(@"SELECT FileID, FileType, FileDirectory, FileDate 
                            FROM tbl_ImFileManager WHERE FileID=@FileID",
                            new tbl_FileManager { FileID = fileid });
                return JsonConvert.SerializeObject(new tbl_FileManager()
                {
                    FileID = Convert.ToString(Model.FileID),
                    FileDirectory = Model.FileDirectory,
                    FileType = Model.FileType,
                    FileDate = Convert.ToString(Model.FileDate)
                });
            }
        }

        public string GetFriendGroupList(string userid)
        {
            using (var conn = MysqlDB.GetOpenConnection())
            {
                var FriendGroups = conn.Query<tbl_FriendGroups>(@"
                        SELECT  GroupID as 'GroupID',
                                GroupCategory as 'GroupCategory',
                                GroupType as 'GroupType',
                                GroupName as 'GroupName',
                                GroupImage as 'GroupImage',
                                GroupMark as 'GroupRemark',
                                GroupUserID as 'GroupUserID',
                                GroupDate as 'GroupDate'
                        FROM  tbl_CustomGroup 
                        WHERE GroupUserID=@UserID;",
                    new { UserID = userid }).ToList();
                return JsonConvert.SerializeObject(FriendGroups);
            }
        }

        public string GetFriendRelationList(string userid)
        {
            using (var conn = MysqlDB.GetOpenConnection())
            {
                var FriendRelation = conn.Query<tbl_FriendRelation>(@"
                        SELECT  RelationId,
                        RelationFromUserId as 'FromUserID',
                        RelationToUserId as 'ToUserID',
                        RelationGroupId as 'FriendGroupID',
                        RelationSpaceGroupId as 'SpaceGroupID',
                        RelationMark  as 'RelationRemark',
                        RelationBGFileID as 'Background',
                        RelationCreateTime as 'RelationDate'
                        FROM tbl_FriendRelationShip 
                    WHERE RelationFromUserId=@UserID;",
                    new { UserID = userid }).ToList();
                return JsonConvert.SerializeObject(FriendRelation);
            }
        }

        public string GetGroupMembers(string groupid)
        {
            using (var conn = MysqlDB.GetOpenConnection())
            {
                var Model = conn.Query<string>(@"SELECT MemberID 
                                              FROM  tbl_GroupMembers 
                                              WHERE GroupID=@GroupID AND   
                                              IsAgree=1 AND IsAdminAgree=1;",
                            new { GroupID = groupid }).ToList();
                return JsonConvert.SerializeObject(Model);
            }
        }

        public bool MessageInsert(string message)
        {
            byte[] bytes = Convert.FromBase64String(message);
            var XmlMessage = Encoding.Default.GetString(bytes);
            try
            {
                var Message = new tbl_Message(XmlMessage);
                if (null != Message.MsgID)
                {
                    using (var conn = MysqlDB.GetOpenConnection())
                    {
                        switch (Message.MsgType)
                        {
                            case "chat":
                            case "groupchat":
                                {
                                    return conn.Execute(@"INSERT INTO tbl_Message
                                    (MsgID, MsgFrom, MsgTo,MsgType,MsgStanza,MsgDate)
                                        VALUES 
                                    (@MsgID, @MsgFrom, @MsgTo,@MsgType,@MsgStanza,@MsgDate)", Message) > 0;
                                };
                            case "chat-arrive-receipt":
                                {
                                    System.Threading.Thread.Sleep(300);
                                    return conn.Execute(@"UPDATE tbl_Message
                                                          SET MsgStatus=3 WHERE MsgID=@MsgID",
                                    new
                                    {
                                        MsgID = Message.MsgID
                                    }) > 0;
                                };
                            case "chat-viewed-receipt":
                                {
                                    System.Threading.Thread.Sleep(300);
                                    return conn.Execute(@"UPDATE tbl_Message 
                                                        SET MsgStatus=4 
                                                        WHERE MsgStatus=3 AND 
                                                              MsgFrom=@MsgFrom AND 
			                                                  MsgTo=@MsgTo",
                                    new
                                    {
                                        MsgFrom = Message.MsgTo,
                                        MsgTo = Message.MsgFrom
                                    }) > 0;
                                };
                            case "chat-recall-message":
                            case "groupchat-recall-message":
                                {
                                    System.Threading.Thread.Sleep(300);
                                    return conn.Execute(@"UPDATE tbl_Message
                                                          SET MsgStanza=@MsgStanza 
                                                          WHERE MsgID=@MsgID",
                                    new
                                    {
                                        MsgID = Message.MsgID,
                                        MsgStanza = Message.MsgStanza
                                    }) > 0;
                                };
                        }
                    };
                }
                return false;
            }
            catch (Exception ex)
            {
                AddExceptionData("tbl_Message", XmlMessage, ex.Message);
                return false;
            }
        }

        public List<string> GetChatRecords(string fromid, string toid)
        {
            using (var conn = MysqlDB.GetOpenConnection())
            {
                if (null != toid)
                {
                    return conn.Query<string>(@"SELECT MsgStanza FROM  tbl_Message
                                            WHERE (MsgFrom=@MsgFrom  AND MsgTo=@MsgTo)
                                                   OR
                                                  (MsgFrom=@MsgTo AND MsgTo=@MsgFrom);",
                                new { MsgFrom = fromid, MsgTo = toid }).ToList();
                }
                else
                {
                    return conn.Query<string>(@"SELECT MsgStanza FROM  tbl_Message
                                            WHERE MsgFrom=@MsgFrom;",
                                new { MsgFrom = fromid }).ToList();
                }
            }
        }

        private bool AddExceptionData(string tablename, string data, string error)
        {
            try
            {
                using (var conn = MysqlDB.GetOpenConnection())
                {
                    return conn.Execute(@"INSERT INTO tbl_TableException
                            (ID,TableName,Staza, Error) 
                            VALUES (@ID,@TableName,@Staza, @Error)", new
                    {
                        ID = Guid.NewGuid().ToString(),
                        TableName = tablename,
                        Staza = data,
                        Error = error
                    }) > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
