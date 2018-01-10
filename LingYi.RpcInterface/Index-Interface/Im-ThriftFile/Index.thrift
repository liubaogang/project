#************************************************************************
#
#                             Im-Index-Service                                 
#                                                                        
#************************************************************************
namespace csharp Im.Rpc.Index

service IndexService {
    bool MsgInsert(1:string message)	#消息记录添加
} 