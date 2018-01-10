#****************************************
#           用户模块RPC服务接口定义
#该接口通过RPC方式调用
#提供的功能有：
#	1.根据用户编码获取信息
#	2.根据用户编码获取额外信息
#	3.根据用户登录名获取信息
#	4.根据用户编码列表获取信息列表
#	5.修改用户的在线状态
#	6.根据用户编码修改信息
#	7.根据经纬度查找距离内的用户列表
#	8.根据主键获取收货地址
#	9.根据用户编码设备类型获取Token
#	10.根据查询条件检索用户列表
#	11.添加虚拟用户（大V用户）
#	12.搜索关联（有推荐人）用户信息
#	13.获取当前登录人的推荐信息
#	14.获取默认收货地址
#	15.通过账号更改密码
#	16.通过用户编码比对当前输入密码是否正确
#	19.获取我的市场的人员列表
#	20.设置代理商类别
#	25.通过账号查询用户数据
#	29.获取全部用户
#	30.用户总条数
#	31.根据账号、真实姓名、昵称获取用户
#	32.发送校验码变更密码、初始账号信息
#	33.反写会员等级
#	34.修复会员升级日志
#****************************************





#=================用户模块RPC服务接口=================
service UserInfoService {
	GetUserInfoRet GetUserInfo(1:string userId)							#根据用户编码获取信息
	GetUserExtendInfoRet GetUserExtendInfo(1:string userId)				#根据用户编码获取额外信息
	GetUserPartInfoByAccountRet GetUserPartInfo(1:string accountId)		#根据用户登录名获取信息	    
	GetUserInfosRet GetUserInfos(1:list<string> userIds)                #根据用户编码列表获取信息列表
	PutAccountStateStatus PutAccountState(1:string userId, 2:AccountStateEnum accountState) #修改用户的在线状态
	PutUserInfoStatus  PutUserInfo(1:string userId, 2:UserInfoParam userInfo)				#根据用户编码修改信息
	GetUserInfosByPositionRet GetUserInfosByPosition(1:GetUserInfosByPosiParam param) 		#根据经纬度距离获取这个范围内的用户列表
	GetReceiveAddressRet GetReceiveAddressById(1:string id)									#根据主键获取收货地址
	GetDeviceTokenRet GetDeviceToken(1:string userBaseId)									#根据用户编码设备类型获取Token
	GetUserInfosBySearchRet GetUserInfosBySearch(1:SearchParam param)					#用户模块RPC服务接口
	PostVirtualInfoStatus PostVirtualInfo(1: VirtualParam param)						#添加虚拟用户（大V用户）
	GetDefaultReceiveAddressRet GetDefaultReceiveAddress(1:string userBaseId)			#获取默认收货地址
	PutPwdByAccountStatus PutPwdByAccount(1:string accountId,2:string newPwd)			#通过账号更改密码
	GetUserPwdIsTrueRet GetUserPwdIsTrue(1:string userBaseId,2:string userPwd)			#通过用户编码比对当前输入密码是否正确
	GetLoginRet GetLogin(1:string userName,2:string password)							#登录接口
	GetRegisterRet Register(1:string userName,2:string password)						#注册接口
	GetUsersOfMyMarketRet GetUsersOfMyMarket(1:marketParam param)						#获取我的市场的人员列表
	PutAgentTypeStatus PutAgentType(1:PutAgentParam param)								#设置代理商类别
	GetIsUserReferrerEmptyRet GetIsUserReferrerEmpty(1:string userBaseId)				#是否有推荐人
	GetUserInfoBySearchRet GetUserInfoBySearch(1:string accountId)						#通过账号查询用户数据
	GetUserInfosRet GetAllUserInfo(1:i32 pageIndex,2:i32 pageSize,3:string searchContent="")                		#获取信息列表
	GetTotalUserCountRet GetTotalUserCount()													#用户总条数
	GetUserInfosByRet GetUserInfosBy(1:string searchContent,2:i32 pageIndex,3:i32 pageSize)		#根据账号、真实姓名、昵称获取用户
	InitAccountOfficialRet InitAccountOfficial(1:string phone)							#发送校验码变更密码、初始账号信息
	RemoveDirtyDataRet RemoveDirtyData(1:string userBaseId)								#移除测试脏数据
	UpdateMemberLevelRet UpdateMemberLevel(1:string userBaseId,2:i32 memberLevel)		#设置会员等级
	UpdateLoginAccRet UpdateLoginAcc(1:string userBaseId,2:string newLoginAcc)			#后台修改用户账户
	RepaireMemberLevelRet RepaireMemberLevel(1:RepaireMemberLevelParam param)			#修复会员升级日志
}
#=================用户模块RPC服务接口=================





#===================修复会员升级日志===================
struct RepaireMemberLevelParam{
	1:string userBaseId,
	2:i32 fromLevel,
	3:i32 toLevel,
	4:string dateTime,
	5:RepaireTypeEnum repaireType,
}

enum RepaireTypeEnum{
	ToBeMember,
	UpdateMemberLevel,
}

enum RepaireMemberLevelRet{
	Success,
	InvalidUserId,
	InvalidFromLevel,
	InvalidToLevel,
	InvalidDateTime,
}
#===================修复会员升级日志===================




#=================后台修改用户账户=================
enum UpdateLoginAccRet{
	Success,
	InvalidUserId,
	InvalidAccount,
	ExistLoginAcc,
}
#=================后台修改用户账户=================



#=================反写会员等级=================
enum UpdateMemberLevelRet{
	Success,
	InvalidUserId,
	InvalidMemberLevel,
}
#=================反写会员等级=================




#=================移除测试脏数据=================
enum RemoveDirtyDataRet{
	Success,
	InvalidUserId,
	Exception,
}
#=================移除测试脏数据=================


#=================发送校验码变更密码、初始账号信息=================
enum InitAccountOfficialRet{
	Success,
	InvalidPhone,
}
#=================发送校验码变更密码、初始账号信息=================




#=================根据账号、真实姓名、昵称获取用户=================
struct GetUserInfosByRet{
	1: GetUserInfosByStatus status,			#状态
	2: list<UserInfo> userInfos				#用户信息
}

enum GetUserInfosByStatus{
	Success,								#成功 
	InvalidSearchContext,					#无效的用户编码
	NoExist,								#超出了参数的最大长度，最大长度为50
}

#=================根据账号、真实姓名、昵称获取用户=================


#=================用户总条数=================
struct GetTotalUserCountRet{
	1: i64 totalCount,
}
#=================用户总条数=================

 
 

 



#=================通过账号查询用户数据=================
struct GetUserInfoBySearchRet{
	1: GetUserInfoBySearchStatus status,
	2: GetUserInfoSearchData data,
}
enum GetUserInfoBySearchStatus{
	Success,
	InvalidAccount,
	NoExist,
}
struct GetUserInfoSearchData{
	1: string userBaseId,
	2: string nickName,
	3: string trueName,
	4: string phone,
	5: string loginAccount,
}
#=================通过账号查询用户数据=================



#=================是否有推荐人=================
struct GetIsUserReferrerEmptyRet{
	1:GetIsUserReferrerEmptyStatus status,
	2:bool exist,
}
enum GetIsUserReferrerEmptyStatus{
	Success,
	InvalidUserId
}
#=================是否有推荐人=================


 



#=================设置代理商类别=================
struct PutAgentParam{
	1:string userBaseId,
	2:AgentType agentType,
	3:string referrerId="",
	4:string loginAccount="",
	5:string nickName=""
}
enum PutAgentTypeStatus{
	Success,				#成功
	InvalidUserId,          #无效的用户编码
	InvalidAgentType,		#无效的代理类型
	InvalidReferrerId,		#无效的推荐人用户编码
	InvalidAccount,			#无效的账号
	LoginAccExist,			#账号已存在
	InvalidNickName,		#无效的昵称
}
#=================设置代理商类别=================




#=================获取我的市场的人员列表=================
enum CanShareType{
	All,					#我的市场（全部用户）
	IdentityCardAuth		#我的市场（认证过身份证的用户）
}
enum MarketType{
	MarketOne,				#一级市场
	MarketTwo,				#二级市场
	MarketThree,			#三级市场
}
enum AgentType{
	CommonUser,				#普通用户
	AgentFirst,				#一级代理商
	AgentSecond,			#二级代理商
	AgentThird,				#三级代理商
	ServiceFirst,			#一级服务商
	ServiceSecond,			#二级服务商
	ServiceThird,           #三级服务商
}
enum AuthedType{
	WaitingComplete,		#尚未认证
	WaitingAuth,			#等待认证
	PartAuth,				#部分认证
	AllAuthed,				#全部认证
	All,					#全部用户
}
struct marketParam{
	1:string userBaseId,
	2:MarketType marketType,
	3:CanShareType canShareType,
	4:i32 pageIndex=1,
	5:i32 pageSize=50,
}
struct GetUsersOfMyMarketRet{
	1:GetUsersOfMyMarketStatus status,		#状态
	2:list<UserInfoStat> userInfoStat,		#统计用到的用户信息
	3:i32 totalCount,						#总页数
}
struct UserInfoStat{
	1: string id,							#用户编码
	2: string loginAccount,					#登录账号
	3: string nickName,						#昵称
	4: string registerDateTime,				#注册时间
	5: AuthedType authedType,				#认证类型
	6: string authedPercent,				#认证百分比
	7: AgentType agentType,					#代理商类别
	8: string marketOne,					#推荐人
	9: string userImage,					#用户图像
	10: SexTypeEnum sex,					#性别
	11: string sexString,					#性别字符串
	12: string trueName,					#真实姓名
}
enum GetUsersOfMyMarketStatus{
	Success,								#成功
	InvalidUserId,							#无效的用户编码
	InvalidMarketType,						#无效的市场类型
	InvalidPageIndex,						#索引页
	InvalidPageSize,						#页数
}
#=================获取我的市场的人员列表=================




#=================登录接口、注册接口=================
struct GetLoginRet{	
	1:GetLoginStatus status,		#状态
	2:string token					#登录之后返回的token
}

enum GetLoginStatus{		
	Success,						#成功
	InvalidUserName,				#无效的账号
	InvalidPassWord,				#无效的密码
	NoExist,						#账号不存在
	PasswordError,					#密码错误
	AccountCancel,					#账号注销
}

struct GetRegisterRet{			
	1:GetRegisterStatus status,		#状态
	2:string token                  #登录之后返回的token
}

enum GetRegisterStatus{
	Success,						#成功
	InvalidUserName,                #无效的账号
	InvalidPassWord                 #无效的密码
}
#=================登录接口、注册接口=================




#==================通过用户编码比对当前输入密码是否正确=====================
struct GetUserPwdIsTrueRet{
	1:GetUserPwdIsTrueStatus status,		#状态
	2:bool isTrue,							#是否正确
}

enum GetUserPwdIsTrueStatus{
	Success,					#成功
	InvalidUserId,				#无效的用户编码	
	InvalidPwd,					#无效密码
}
#==================通过用户编码比对当前输入密码是否正确=====================





#==================获取默认收货地址=====================
enum PutPwdByAccountStatus{
	Success,					#成功
	InvalidAccount,				#无效的账号
	NoExist,					#不存在
	InvalidNewPwd,				#无效的密码
}
#==================获取默认收货地址=====================




#==================获取默认收货地址=====================
struct GetDefaultReceiveAddressRet{
	1: GetDefaultReceiveAddressStatus status,		#状态
	2: ReceiveAddress receiveAddress				#用户收货地址详情
}

enum GetDefaultReceiveAddressStatus{
	Success,										#成功
	InvalidUserId,									#无效的用户编码
	NoExist,										#不存在
}
#==================获取默认收货地址=====================





#=================获取当前登录人的推荐信息=================
struct GetReferrerByUserIdRet{
	1: GetReferrerByUserIdStatus status,		#状态
	2: string referrer							#推荐人账号
}

enum GetReferrerByUserIdStatus{
	Success,							#成功
	IdentityUserBaseId,					#无效的用户编码
	NoExist,							#无记录
}
#=================获取当前登录人的推荐信息=================


#================添加虚拟用户（大V用户）====================
struct VirtualParam{
	1: string userImage			#用户图像
	2: string nickName			#昵称
	3: string trueName			#真实姓名
	4: SexTypeEnum sex			#性别
	5: i32 age					#年龄
	6: string nativePlace		#籍贯
}

enum PostVirtualInfoStatus{
	Success,					#成功
	InvalidUserImage,			#无效的用户图像
	InvalidNickName,			#无效的昵称
	InvalidTrueName,			#无效的真实姓名
	InvalidSex,					#无效的性别
	InvalidAge,					#无效的年龄
	InvalidNativePlace,			#无效的籍贯
}
#================添加虚拟用户（大V用户）====================





#=================根据查询条件检索用户列表=================
struct GetUserInfosBySearchRet{
	1: GetUserInfosBySearchStatus status,	#状态
	2: list<UserInfo> userInfos				#用户信息
}
enum GetUserInfosBySearchStatus{
	Success,									#成功 
	InvalidSex,									#无效的性别
	InvalidAge,									#无效的年龄
	InvalidProfession,							#无效的职业
	InvalidNativePlace,							#无效的籍贯
	InvalidBelife,								#无效的信仰
	InvalidYearOfBirth,							#无效的属相
	InvalidConstellation,						#无效的星座
	NoExist,									#无记录
	InvalidParam,								#无效的查询参数
	InvalidPageIndex,							#无效的起始页
	InvalidPageSize,							#无效的页数
	InvalidNickName,							#无效的昵称
}
struct SearchParam{
	1: SearchOptionEnum SearchOption				#选择查询的项
	2: SexTypeEnum sex								#性别
	3: i32 beginAge									#下限年龄
	4: string constellation							#星座
	5:string constellationString					#星座字符串
	6: string profession							#职业
	7: string professionString						#职业字符串
	8: string nativePlace							#籍贯
	9: string nativePlaceString						#籍贯字符串
	10: string belife								#信仰
	11: string belifeString							#信仰字符串
	12: string yearOfBirth							#属相
	13: string yearOfBirthString					#属相字符串
	14: i32 endAge									#上限年龄
	15: string nickName								#昵称
	16: i32 pageIndex								#起始页
	17: i32 pageSize								#页数
}
enum SearchOptionEnum{
	Sex = 1,					#用户的性别
	Age = 2,					#年龄	
	Profession = 4,				#职业	
	NativePlace = 8,			#籍贯
	Belife = 16,				#信仰
	YearOfBirth = 32,			#属相 
	Constellation = 64,			#星座
	NickName = 128,				#昵称
}
#=================根据查询条件检索用户列表=================






#===============根据用户编码设备类型获取Token============
enum DeviceTypeEnum{
	iOS=1,									#苹果设备
	Android=2								#Android设备
}

struct GetDeviceTokenRet{
	1: GetDeviceTokenStatus status,		#状态
	2: string deviceToken,				#设备Token
	3: string deviceNumber,				#设备型号
	4: DeviceTypeEnum deviceType,
	5: string userBaseId,
}

enum GetDeviceTokenStatus{
	Success,							#成功
	IdentityUserBaseId,					#无效的用户编码
}
#===============根据用户编码设备类型获取Token============





#===============根据主键获取收货地址详情============
struct GetReceiveAddressRet{
	1: GetReceiveAddressStatus status,			#状态
	2: ReceiveAddress receiveAddress			#用户收货地址详情
}

enum GetReceiveAddressStatus{
	Success,									#成功 
	InvalidId,									#无效的Id
}


struct ReceiveAddress{
	1: string id								#主键
	2: string userBaseId						#用户编码
	3: string consignee							#收货人
	4: string phone								#联系电话
	5: string areaInfo							#区域信息（省市区街道编号）
	6: string areaInfoString					#区域信息（省市区街道名称）
	7: string addressName						#收货人地址（具体地址）
	8: bool isDefaultAddress					#是否是默认配送地址
}


#===============根据主键获取收货地址详情============



#===============根据经纬度距离获取这个范围内的用户列表============

struct GetUserInfosByPosiParam{
	1: double positionX,
	2: double positionY,
	3: double instance,
	4: i32 pageIndex,
	5: i32 pageSize,
	6: SexTypeEnum sexType,
	7: i32 afterDay=7								#更新坐标在N天之内的
}


struct GetUserInfosByPositionRet{
	1: GetUserInfosByPositionStatus status,			#状态
	2: list<UserInfo> userInfos						#用户信息
}

enum GetUserInfosByPositionStatus{
	Success,								#成功 
	InvalidPositionX,						#无效的经度
	InvalidPositionY,						#无效的维度
	InvalidInstance,						#无效的距离值
	NoExist,								#不存在
	InvalidPageIndex,						#无效的起始页
	InvalidPageSize,						#无效的页数
}


#===============根据经纬度距离获取这个范围内的用户列表============



#=================根据用户编码获取信息=================
struct GetUserInfoRet{
	1: GetUserInfoStatus status,			#状态
	2: UserInfo userInfo					#用户信息
}

enum GetUserInfoStatus{
	Success,								#成功 
	InvalidUserId,							#无效的用户编码
	NoExist,								#不存在
}

struct UserInfo {   
	1: string userId						#用户ID
	2: string nickName						#昵称
    3: string trueName						#真实姓名			
    4: SexTypeEnum sex						#性别	
	5: string sexString						#性别字符串
	6: string birthday						#用户的出生年月
	7: string phone							#手机号
	8: string userImage						#用户的头像
	9: AccountStateEnum accountState		#用户信息状态
	10: string accountStateString			#用户信息状态字符串
	11: string remark						#备注信息
	12: double instance						#距离
	13: string OpenId						#OpenId
	14: string lastUpdateLocationDt			#最后一次更新经纬度时间
	15: AgentType agentType					#代理商类别
	16: AuthedType authedType,				#认证类型
	17: string authedPercent,				#认证百分比
	18: i32 age,							#年龄
	19: string profession,					#职业
	20: string professionString,			#职业字符串
	21: string nativePlace,					#籍贯
	22: string nativePlaceString,			#籍贯字符串
	23: string createDateTime,				#注册时间
	24: string loginAccount,				#账号
	25: i32 memberLevel,					#会员等级
	26: bool isCancel,						#账号是否注销
	27: string marketOne,					#一级市场
	28: string identityCard,				#证件号码
}
enum SexTypeEnum {		
	Unknown,							#未知
	Man,								#男
	Woman,								#女
	All									#全部
}

enum AccountStateEnum {		
	OffLineState,							#离线
	OnLineState,							#在线
}
#=================根据用户编码获取信息=================






#=================根据用户编码获取额外信息================= 
struct GetUserExtendInfoRet{
	1: GetUserInfoStatus status,			#状态
	2: UserExtendInfo userExtendInfo		#用户额外信息
}

struct UserExtendInfo {   
	1: string userId								#用户ID
	2: string yearOfBirth							#属相
	3: string constellation							#星座
	4: RegisterTypeEnum registerType				#注册类型
	5: string registerTypeString					#注册类型字符串
	6: string nowPlace								#当前所在地
	7: string nowPlaceString						#当前所在地字符串
	8: string profession							#职业
	9: string professionString						#职业字符串
	10: string nativePlace							#籍贯
	11: string nativePlaceString					#籍贯字符串
	12: string nation								#民族
	13: string nationString							#民族字符串
	14: string belife								#信仰
	15: string belifeString							#信仰字符串
	16: string description							#个人说明
	17: string identityCard							#身份证
	18: string passport								#护照
	19: string officersCard							#军官证
	20: CertificateTypeEnum certificateType			#证件类型
	21: string certificateValue						#证件类型值
	22: string email								#邮箱 
	23: string yearOfBirthString					#属相字符串
	24: string constellationString					#星座字符串
	25: i32 memberLevel								#会员等级
}

enum RegisterTypeEnum {								#注册类型
	Personal,										#个人
	PrivateEnterPrise,								#私企
	NationalEnterPrise,								#国企
	ForeignEnterPrise,								#外企
	Organization,									#团体组织
}

enum CertificateTypeEnum {							#证件类型
	IdentityCard,									#身份证
	Passport,										#护照
	OfficersCard,									#军官证
} 
#=================根据用户编码获取额外信息=================








#=================根据用户登录名获取信息================= 
struct GetUserPartInfoByAccountRet {
	1: GetUserPartInfoStatus status			#状态
	2: UserInfo userInfo					#用户信息
}

enum GetUserPartInfoStatus{
	Success,								#成功
	InvalidAccount,							#无效的登录账号
	NoExist,								#账号不存在
}
#=================根据用户登录名获取信息=================





#=================根据用户编码列表获取信息列表=================
struct GetUserInfosRet{
	1: GetUserInfosStatus status,			#状态
	2: list<UserInfo> userInfos,			#用户信息
	3: i32 totalCount
}

enum GetUserInfosStatus{
	Success,								#成功 
	InvalidUserId,							#无效的用户编码
	ListMaxCount,							#超出了参数的最大长度，最大长度为50
}
#=================根据用户编码列表获取信息列表=================









#=================修改用户的在线状态=================
enum PutAccountStateStatus{
	Success,								#成功 
	InvalidUserId,							#无效的用户编码
	InvalidAccountState,					#无效的在线状态
}
#=================修改用户的在线状态================= 







#=================根据用户编码修改信息=================
struct UserInfoParam{
	1: PutOptionEnum putOption						#修改配置
	2: string nickName								#昵称
	3: string trueName								#真实姓名	
	4: SexTypeEnum sex								#性别	
	5: string birthday								#用户的出生年月
	6: string phone									#手机号
	7: string email									#邮箱 
	8: string nowPlace								#当前所在地
	9: CertificateTypeEnum certificateType			#证件类型
	10: string certificateValue						#证件类型值
	11: string OpenId								#开放Id
}

enum  PutOptionEnum{				#修改配置
	NickName = 1,					#昵称
	TrueName = 2,					#真实姓名	
	Sex		= 4,					#性别	
	Birthday = 8,					#用户的出生年月
	Phone	= 16,					#手机号
	Email	= 32,					#邮箱 
	NowPlace	= 64,				#当前所在地
	Certificate	=128,				#证件
	OpenId=256,						#开放Id
}									


enum PutUserInfoStatus{
	Success,								#成功 
	InvalidUserId,							#无效的用户编码
	InfoIsNull,								#信息为空
	InvalidNickName,                        #无效的昵称
	InvalidTrueName,  						#无效的真实姓名	
	InvalidSex,		 						#无效的性别	
	InvalidBirthday,  						#无效的用户的出生年月
	InvalidPhone,	 						#无效的手机号
	InvalidEmail,	 						#无效的邮箱 
	InvalidNowPlace,						#无效的当前所在地
	InvalidCertificateType,					#无效的证件类型
	InvalidCertificateValue,				#无效的证件值
	InvalidOpenId,							#开放Id		
	
	AuthedTrueName,							#真实姓名已认证
	AuthedPhone,							#手机号已认证
	AuthedEmail,							#邮箱已认证
	AuthedIdentityCard,						#身份证已认证	
}
#=================根据用户编码修改信息================= 




