#************************************************************************
#
#                             Im-Mysql-Service                                 
#                                                                        
#************************************************************************
namespace csharp Im.Rpc.Public

service PublicService {						              # 定义服务接口
     string GetGroupsInfoByOA(1:string GroupID)			  # 获得群组信息用于OA板块

} 