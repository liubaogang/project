service UserSessionService {		#用户会话服务接口 
	CreateSessionRet CreateSession (1:string userId)			#创建会话
	GetUserIdRet GetUserId(1:string sessionId)				#根据会话编码获取用户编码
	RemoveSessionStatus RemoveSession (1:string sessionId)		#删除会话
} 

struct	CreateSessionRet{			#创建会话返回类型
	1: CreateSessionStatus status,	#状态
	2: string sessionId				#会话编码
}

enum CreateSessionStatus {
   Success,							#成功
   InvalidUserId,					#无效的用户编码
}

struct	GetUserIdRet{				#根据会话编码获取用户编码返回类型
	1: GetUserIdStatus status		#状态
	2: string userId				#用户编码
}

enum GetUserIdStatus {
   Success,							#成功
   InvalidSessionId,				#无效的会话编码
   NotExit,							#不存在
   Expired,							#已过期
}

enum RemoveSessionStatus {
   Success,							#成功 
   InvalidSessionId,				#无效的会话编码
}