#************************************************************************
#
#                             Im-Mysql-Service                                 
#                                                                        
#************************************************************************
namespace csharp Im.Rpc.Mysql

service MysqlService {						              # 定义服务接口
    bool FileInsert(1:string filemanager)	              # 文件添加
	string GetFileByID(1:string fileid)	                  # 根据ID过得一条记录
	bool MessageInsert(1:string message)	              # 消息记录保存
	list<string> GetChatRecords(1:string fromid,2:string toid) # 获得聊天记录





	#老数据操作
	string GetFriendGroupList(1:string userid)		      #获得个人分组
	string GetFriendRelationList(1:string userid)	      #获得好友关系
	string GetGroupMembers(1:string groupid)		      #获得群组成员

} 