#************************************************************************
#
#                             Im-Redis-Service                                 
#                                                                        
#************************************************************************
namespace csharp Im.Rpc.Redis

service RedisService {

	bool SetUnreadNumber(1:string userid,2:i32 count)			#设置未读数量
    #==================================================================
    bool FriendGroupIns(1:string friendgroup)  					#好友分组添加
	bool FriendGroupUpd(1:string friendgroup)  					#好友分组修改
	bool FriendGroupDel(1:string friendgroup)					#好友分组删除
	string GetFriendGroupList(1:string userid)					#获得个人分组
	#==================================================================
	bool FriendRelationIns(1:string friendrelation)				#好友关系添加
	bool FriendRelationUpd(1:string friendrelation)				#好友关系修改
	bool FriendRelationDel(1:string friendrelation)				#好友关系删除
	string GetFriendRelationList(1:string userid)				#获得好友关系
	#==================================================================

}
