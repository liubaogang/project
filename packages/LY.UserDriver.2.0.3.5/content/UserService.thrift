#****************************************
#           �û�ģ��RPC����ӿڶ���
#�ýӿ�ͨ��RPC��ʽ����
#�ṩ�Ĺ����У�
#	1.�����û������ȡ��Ϣ
#	2.�����û������ȡ������Ϣ
#	3.�����û���¼����ȡ��Ϣ
#	4.�����û������б��ȡ��Ϣ�б�
#	5.�޸��û�������״̬
#	6.�����û������޸���Ϣ
#	7.���ݾ�γ�Ȳ��Ҿ����ڵ��û��б�
#	8.����������ȡ�ջ���ַ
#	9.�����û������豸���ͻ�ȡToken
#	10.���ݲ�ѯ���������û��б�
#	11.��������û�����V�û���
#	12.�������������Ƽ��ˣ��û���Ϣ
#	13.��ȡ��ǰ��¼�˵��Ƽ���Ϣ
#	14.��ȡĬ���ջ���ַ
#	15.ͨ���˺Ÿ�������
#	16.ͨ���û�����ȶԵ�ǰ���������Ƿ���ȷ
#	17.��¼�ӿ�
#	18.ע��ӿ�
#	19.��ȡ�ҵ��г�����Ա�б�
#	20.���ô��������
#	21.ͨ�����֤����ʵ������Ȩ��֤
#	22.��ȡ��֤�û��б�������֤���͵��û���
#	23.��֤���ͨ���ӿ�
#	24.��֤��˲��ؽӿ�
#	25.ͨ���˺Ų�ѯ�û�����
#	26.��ǰ�û��Ƿ���֤
#	27.��֤ģ��5����Ϣ�Ƿ�����д
#	28.��֤ģ��5�����ݷ�д�ӿ�
#	29.��ȡȫ���û�
#	30.�û�������
#	31.�����˺š���ʵ�������ǳƻ�ȡ�û�
#	32.����У���������롢��ʼ�˺���Ϣ
#	33.��д��Ա�ȼ�
#	34.�޸���Ա������־
#****************************************





#=================�û�ģ��RPC����ӿ�=================
service UserInfoService {
	GetUserInfoRet GetUserInfo(1:string userId)							#�����û������ȡ��Ϣ
	GetUserExtendInfoRet GetUserExtendInfo(1:string userId)				#�����û������ȡ������Ϣ
	GetUserPartInfoByAccountRet GetUserPartInfo(1:string accountId)		#�����û���¼����ȡ��Ϣ	    
	GetUserInfosRet GetUserInfos(1:list<string> userIds)                #�����û������б��ȡ��Ϣ�б�
	PutAccountStateStatus PutAccountState(1:string userId, 2:AccountStateEnum accountState) #�޸��û�������״̬
	PutUserInfoStatus  PutUserInfo(1:string userId, 2:UserInfoParam userInfo)				#�����û������޸���Ϣ
	GetUserInfosByPositionRet GetUserInfosByPosition(1:GetUserInfosByPosiParam param) 		#���ݾ�γ�Ⱦ����ȡ�����Χ�ڵ��û��б�
	GetReceiveAddressRet GetReceiveAddressById(1:string id)									#����������ȡ�ջ���ַ
	GetDeviceTokenRet GetDeviceToken(1:string userBaseId,2:DeviceTypeEnum deviceType)		#�����û������豸���ͻ�ȡToken
	GetUserInfosBySearchRet GetUserInfosBySearch(1:SearchParam param)					#�û�ģ��RPC����ӿ�
	PostVirtualInfoStatus PostVirtualInfo(1: VirtualParam param)						#��������û�����V�û���
	GetReferrerUsersRet GetReferrerUsers(1:string beginDate,2:string endDate)			#�������������Ƽ��ˣ��û���Ϣ
	#GetReferrerByUserIdRet GetReferrerByUserId(1:string userBaseId)					#��ȡ��ǰ��¼�˵��Ƽ���Ϣ
	GetDefaultReceiveAddressRet GetDefaultReceiveAddress(1:string userBaseId)			#��ȡĬ���ջ���ַ
	PutPwdByAccountStatus PutPwdByAccount(1:string accountId,2:string newPwd)			#ͨ���˺Ÿ�������
	GetUserPwdIsTrueRet GetUserPwdIsTrue(1:string userBaseId,2:string userPwd)			#ͨ���û�����ȶԵ�ǰ���������Ƿ���ȷ
	GetLoginRet GetLogin(1:string userName,2:string password)							#��¼�ӿ�
	GetRegisterRet Register(1:string userName,2:string password)						#ע��ӿ�
	GetUsersOfMyMarketRet GetUsersOfMyMarket(1:marketParam param)						#��ȡ�ҵ��г�����Ա�б�
	PutAgentTypeStatus PutAgentType(1:PutAgentParam param)								#���ô��������
	
	#PutUserAuthedRet PutUserAuthed(1:string userBaseId)									#ͨ�����֤����ʵ������Ȩ��֤
	#GetUserInfosAuthRet GetUserInfosAuth(1:UserInfosAuthParam param)					#��ȡ��֤�û��б�������֤���͵��û���
	#GetUserInfoAuthRet GetUserInfoAuthById(1:string userBaseId)							#��ȡ��֤�û����飨������֤���͵��û���
	#GetUserAuthPassRet GetUserAuthPass(1:string userBaseId,2:UserAuthItemsEnum userAuthItemsEnum)								#��֤���ͨ���ӿ�
	#GetUserAuthRejectRet GetUserAuthReject(1:string userBaseId,2:UserAuthItemsEnum userAuthItemsEnum,3:string reasonStr)		#��֤��˲��ؽӿ�
	
	GetIsUserReferrerEmptyRet GetIsUserReferrerEmpty(1:string userBaseId)				#�Ƿ����Ƽ���
	GetUserInfoBySearchRet GetUserInfoBySearch(1:string accountId)						#ͨ���˺Ų�ѯ�û�����
	
	#GetMyFriendIsAuthRet GetMyFriendIsAuth(1:string userBaseId)							#��ǰ�û��Ƿ���֤
	#GetMyFriendIsAuthRet GetMyFriendIsAuth2(1:string userBaseId,2:string friendUserId)	#��ǰ�û��Ƿ���֤
	
	GetIsAuthed5ForEditRet GetIsAuthed5ForEdit(1:string userBaseId)						#֤����Ϣ����Ƿ���Է�д
	Certificate5EditRet Certificate5Edit(1:string userBaseId,Certificate5Param param)	#��֤ģ��5�����ݷ�д�ӿ�
	GetUserInfosRet GetAllUserInfo(1:i32 pageIndex,2:i32 pageSize,3:string searchContent="")                		#��ȡ��Ϣ�б�
	GetTotalUserCountRet GetTotalUserCount()											#�û�������
	GetUserInfosByRet GetUserInfosBy(1:string searchContent,2:i32 pageIndex,3:i32 pageSize)		#�����˺š���ʵ�������ǳƻ�ȡ�û�
	InitAccountOfficialRet InitAccountOfficial(1:string phone)							#����У���������롢��ʼ�˺���Ϣ
	RemoveDirtyDataRet RemoveDirtyData(1:string userBaseId)								#�Ƴ�����������
	UpdateMemberLevelRet UpdateMemberLevel(1:string userBaseId,2:i32 memberLevel)		#���û�Ա�ȼ�
	UpdateLoginAccRet UpdateLoginAcc(1:string userBaseId,2:string newLoginAcc)			#��̨�޸��û��˻�
	RepaireMemberLevelRet RepaireMemberLevel(1:RepaireMemberLevelParam param)			#�޸���Ա������־
}
#=================�û�ģ��RPC����ӿ�=================






#===================�޸���Ա������־===================
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
#===================�޸���Ա������־===================




#=================��̨�޸��û��˻�=================
enum UpdateLoginAccRet{
	Success,
	InvalidUserId,
	InvalidAccount,
	ExistLoginAcc,
}
#=================��̨�޸��û��˻�=================



#=================��д��Ա�ȼ�=================
enum UpdateMemberLevelRet{
	Success,
	InvalidUserId,
	InvalidMemberLevel,
}
#=================��д��Ա�ȼ�=================




#=================�Ƴ�����������=================
enum RemoveDirtyDataRet{
	Success,
	InvalidUserId,
	Exception,
}
#=================�Ƴ�����������=================


#=================����У���������롢��ʼ�˺���Ϣ=================
enum InitAccountOfficialRet{
	Success,
	InvalidPhone,
}
#=================����У���������롢��ʼ�˺���Ϣ=================




#=================�����˺š���ʵ�������ǳƻ�ȡ�û�=================
struct GetUserInfosByRet{
	1: GetUserInfosByStatus status,			#״̬
	2: list<UserInfo> userInfos				#�û���Ϣ
}

enum GetUserInfosByStatus{
	Success,								#�ɹ� 
	InvalidSearchContext,					#��Ч���û�����
	NoExist,								#�����˲�������󳤶ȣ���󳤶�Ϊ50
}

#=================�����˺š���ʵ�������ǳƻ�ȡ�û�=================


#=================�û�������=================
struct GetTotalUserCountRet{
	1: i64 totalCount,
}
#=================�û�������=================



#=================��֤ģ��5�����ݷ�д�ӿ�=================
struct Certificate5Param{			#�����֤������Ϣ��д����
	1: string trueName,				#��ʵ����
	2: string identityCard,			#֤������
	3: string imageFrontCard,		#֤��������
	4: string imageBackCard,		#֤��������
	5: string imageUserCard,		#֤���ֳ���
}

enum Certificate5EditRet{
	Success,					#�ɹ�
	InvalidUserId,				#��Ч���û�����
	InvalidTrueName,			#��Ч����ʵ����
	InvalidIdentityCard,		#��Ч��֤������
	InvalidImageFrontCard,		#��Ч��֤��������
	InvalidImageBackCard,		#��Ч��֤��������
	InvalidImageUserCard,		#��Ч��֤���ֳ���
	NoEdit,						#��ֹ��д
}
#=================��֤ģ��5�����ݷ�д�ӿ�=================



#=================֤����Ϣ����Ƿ���Է�д=================
struct GetIsAuthed5ForEditRet{				#֤����Ϣ�Ƿ�����д
	1:GetIsAuthed5ForEditStatus status,		#״̬
	2:bool isCanEdit,						#true���Է�д;false������д
}

enum GetIsAuthed5ForEditStatus{
	Success,
	InvalidUserId,
}
#=================֤����Ϣ����Ƿ���Է�д=================


#=================��ȡ�ҵĺ�����֤�б�=================
#struct GetMyFriendIsAuthRet{
#	1:GetMyFriendIsAuthStatus status,
#	2:UserInfoStat userInfoStat,		#ͳ���õ����û���Ϣ
#	3:bool isAuth,
#}
#enum GetMyFriendIsAuthStatus{
#	Success,
#	InvalidUserId
#}
#=================��ȡ�ҵĺ�����֤�б�=================



#=================ͨ���˺Ų�ѯ�û�����=================
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
#=================ͨ���˺Ų�ѯ�û�����=================



#=================�Ƿ����Ƽ���=================
struct GetIsUserReferrerEmptyRet{
	1:GetIsUserReferrerEmptyStatus status,
	2:bool exist,
}
enum GetIsUserReferrerEmptyStatus{
	Success,
	InvalidUserId
}
#=================�Ƿ����Ƽ���=================



#=================��֤���ͨ��/���ؽӿ�=================
#enum UserAuthItemsEnum{
#	EmergencyCall,				#�����绰
#	Address,					#�����ַ
#	ImageFrontCard,				#���֤����
#	ImageBackCard,				#���֤����
#	ImageUserCard,				#�ֳ����֤
#	TrueName,					#��ʵ����
#	IdentityCard				#���֤��
#}
#enum GetUserAuthPassRet{
#	Success,								#�ɹ�
#	InvalidUserId,							#��Ч���û�����
#	InvalidUserAuthItemsEnum				#��Ч����֤������
#}
#enum GetUserAuthRejectRet{
#	Success,								#�ɹ�
#	InvalidUserId,							#��Ч���û�����
#	InvalidUserAuthItemsEnum,				#��Ч����֤������ 
#	InvalidReasonStr						#��Ч����ԭ��
#}
#=================��֤���ͨ��/���ؽӿ�=================



#=================��ȡ��֤�û����飨������֤���͵��û���=================
#struct GetUserInfoAuthRet{
#	1:GetUserInfoAuthStatus status,				#״̬
#	2:UserInfoAuth userAuthRet,					#��֤�û�����
#}
#
#enum GetUserInfoAuthStatus{
#	Success,
#	InvalidUserId,
#}

#=================��ȡ��֤�û����飨������֤���͵��û���=================




#=================��ȡ��֤�û��б�������֤���͵��û���=================
#struct UserInfosAuthParam{
#	1:i32 pageIndex,
#	2:i32 pageSize,
#	3:AuthedType authedType,
#	4:string searchContent,
#}
#
#struct GetUserInfosAuthRet{
#	1:GetUserInfosAuthStatus status,			#״̬
#	2:list<UserInfoAuth> usersAuthRet,			#��֤�û��б�
#	3:i32 totalCount,							#�ܼ�����
#}
#enum GetUserInfosAuthStatus{
#	Success,							#�ɹ�
#	InvalidPageIndex,					#��Ч��PageIndex
#	InvalidPageSize,					#��Ч��PageSize
#	InvalidAuthedType,					#��Ч����֤�ȼ�
#	NoExist,							#������
#}
#struct UserInfoAuth{
#	1:string userBaseId,				#�û�����
#	2:string trueName,					#��ʵ����
#	3:bool isTrueNameAuth,				#��ʵ�����Ƿ�����֤
#	4:string phone,						#�绰
#	5:bool isPhoneAuth,					#�绰�Ƿ�����֤
#	6:string identityCard,				#���֤��
#	7:bool IsIdentityCardAuth,			#���֤�Ƿ�����֤
#	8:string email,						#����
#	9:bool isEmailAuth,					#�����Ƿ�����֤
#	10:string emergencyCall,			#�����绰
#	11:bool isEmergencyCallAuth,		#�����绰�Ƿ�����֤
#	12:string address,					#�����ַ
#	13:bool isAddressAuth,				#�����ַ�Ƿ�����֤
#	14:string imageFrontCard,			#���֤����
#	15:bool isImageFrontCardAuth,		#���֤�����Ƿ�����֤
#	16:string imageBackCard,			#���֤����
#	17:bool isImageBackCardAuth,		#���֤�����Ƿ�����֤
#	18:string imageUserCard,			#�ֳ����֤
#	19:bool isImageUserCardAuth,		#�ֳ����֤�Ƿ�����֤
#	20:string loginAccount,				#�˺�
#	21:AgentType agentType,				#���������
#	22:string marketOne,				#�Ƽ���
#	23:string addressReject,			#��ַ�ܾ�ԭ��
#	24:string emergencyCallReject,		#�����绰�ܾ�ԭ��
#	25:string imageBackCardReject,		#���֤����ܾ�ԭ��
#	26:string imageFrontCardReject,		#���֤����ܾ�ԭ��
#	27:string imageUserCardReject,		#�ֳ����֤�ܾ�ԭ��
#	28:string nickName,					#�ǳ�
#	29:string createDateTime,			#����ʱ��
#	30:bool isCancel,					#�˺��Ƿ�ע��
#	31:string authedPercent,			#��֤����
#	32:string auth3ConditionDateTime,	#
#	33:i32 memberLevel,					#��Ա�ȼ�
#}
#=================��ȡ��֤�û��б�������֤���͵��û���=================





#=================���ô��������=================
struct PutAgentParam{
	1:string userBaseId,
	2:AgentType agentType,
	3:string referrerId="",
	4:string loginAccount="",
	5:string nickName=""
}
enum PutAgentTypeStatus{
	Success,				#�ɹ�
	InvalidUserId,          #��Ч���û�����
	InvalidAgentType,		#��Ч�Ĵ�������
	InvalidReferrerId,		#��Ч���Ƽ����û�����
	InvalidAccount,			#��Ч���˺�
	LoginAccExist,			#�˺��Ѵ���
	InvalidNickName,		#��Ч���ǳ�
}
#=================���ô��������=================




#=================��ȡ�ҵ��г�����Ա�б�=================
enum CanShareType{
	All,					#�ҵ��г���ȫ���û���
	IdentityCardAuth		#�ҵ��г�����֤�����֤���û���
}
enum MarketType{
	MarketOne,				#һ���г�
	MarketTwo,				#�����г�
	MarketThree,			#�����г�
}
enum AgentType{
	CommonUser,				#��ͨ�û�
	AgentFirst,				#һ��������
	AgentSecond,			#����������
	AgentThird,				#����������
	ServiceFirst,			#һ��������
	ServiceSecond,			#����������
	ServiceThird,           #����������
}
enum AuthedType{
	WaitingComplete,		#��δ��֤
	WaitingAuth,			#�ȴ���֤
	PartAuth,				#������֤
	AllAuthed,				#ȫ����֤
	All,					#ȫ���û�
}
struct marketParam{
	1:string userBaseId,
	2:MarketType marketType,
	3:CanShareType canShareType,
	4:i32 pageIndex=1,
	5:i32 pageSize=50,
}
struct GetUsersOfMyMarketRet{
	1:GetUsersOfMyMarketStatus status,		#״̬
	2:list<UserInfoStat> userInfoStat,		#ͳ���õ����û���Ϣ
	3:i32 totalCount,						#��ҳ��
}
struct UserInfoStat{
	1: string id,							#�û�����
	2: string loginAccount,					#��¼�˺�
	3: string nickName,						#�ǳ�
	4: string registerDateTime,				#ע��ʱ��
	5: AuthedType authedType,				#��֤����
	6: string authedPercent,				#��֤�ٷֱ�
	7: AgentType agentType,					#���������
	8: string marketOne,					#�Ƽ���
	9: string userImage,					#�û�ͼ��
	10: SexTypeEnum sex,					#�Ա�
	11: string sexString,					#�Ա��ַ���
	12: string trueName,					#��ʵ����
}
enum GetUsersOfMyMarketStatus{
	Success,								#�ɹ�
	InvalidUserId,							#��Ч���û�����
	InvalidMarketType,						#��Ч���г�����
	InvalidPageIndex,						#����ҳ
	InvalidPageSize,						#ҳ��
}
#=================��ȡ�ҵ��г�����Ա�б�=================




#=================��¼�ӿڡ�ע��ӿ�=================
struct GetLoginRet{	
	1:GetLoginStatus status,		#״̬
	2:string token					#��¼֮�󷵻ص�token
}

enum GetLoginStatus{		
	Success,						#�ɹ�
	InvalidUserName,				#��Ч���˺�
	InvalidPassWord,				#��Ч������
	NoExist,						#�˺Ų�����
	PasswordError,					#�������
	AccountCancel,					#�˺�ע��
}

struct GetRegisterRet{			
	1:GetRegisterStatus status,		#״̬
	2:string token                  #��¼֮�󷵻ص�token
}

enum GetRegisterStatus{
	Success,						#�ɹ�
	InvalidUserName,                #��Ч���˺�
	InvalidPassWord                 #��Ч������
}
#=================��¼�ӿڡ�ע��ӿ�=================




#==================ͨ���û�����ȶԵ�ǰ���������Ƿ���ȷ=====================
struct GetUserPwdIsTrueRet{
	1:GetUserPwdIsTrueStatus status,		#״̬
	2:bool isTrue,							#�Ƿ���ȷ
}

enum GetUserPwdIsTrueStatus{
	Success,					#�ɹ�
	InvalidUserId,				#��Ч���û�����	
	InvalidPwd,					#��Ч����
}
#==================ͨ���û�����ȶԵ�ǰ���������Ƿ���ȷ=====================





#==================��ȡĬ���ջ���ַ=====================
enum PutPwdByAccountStatus{
	Success,					#�ɹ�
	InvalidAccount,				#��Ч���˺�
	NoExist,					#������
	InvalidNewPwd,				#��Ч������
}
#==================��ȡĬ���ջ���ַ=====================




#==================��ȡĬ���ջ���ַ=====================
struct GetDefaultReceiveAddressRet{
	1: GetDefaultReceiveAddressStatus status,		#״̬
	2: ReceiveAddress receiveAddress				#�û��ջ���ַ����
}

enum GetDefaultReceiveAddressStatus{
	Success,										#�ɹ�
	InvalidUserId,									#��Ч���û�����
	NoExist,										#������
}
#==================��ȡĬ���ջ���ַ=====================





#=================��ȡ��ǰ��¼�˵��Ƽ���Ϣ=================
struct GetReferrerByUserIdRet{
	1: GetReferrerByUserIdStatus status,		#״̬
	2: string referrer							#�Ƽ����˺�
}

enum GetReferrerByUserIdStatus{
	Success,							#�ɹ�
	IdentityUserBaseId,					#��Ч���û�����
	NoExist,							#�޼�¼
}
#=================��ȡ��ǰ��¼�˵��Ƽ���Ϣ=================





#=================�������������Ƽ��ˣ��û���Ϣ=================
struct GetReferrerUsersRet{
	1: GetReferrerUsersStatus status,		#״̬
	2: list<UserBase> userBases				#�û���Ϣ
}

struct UserBase{
	1: string id,							#�û�����
	2: string loginAccount,					#��¼�˺�
	3: string loginPwd,						#��¼����
	4: string registerDateTime,				#ע��ʱ��
	5: string referrer,						#�Ƽ���
	6: string deviceTokenCode,				#�豸Token
	7: string registerDtDeviceUser,			#�豸�û��״�ע��ʱ��
}
enum GetReferrerUsersStatus{
	Success,								#�ɹ�
	InvalidBeginDate,						#��Ч�Ŀ�ʼʱ��
	InvalidEndDate,							#��Ч�Ľ���ʱ��
}


#=================�������������Ƽ��ˣ��û���Ϣ=================





#================��������û�����V�û���====================
struct VirtualParam{
	1: string userImage			#�û�ͼ��
	2: string nickName			#�ǳ�
	3: string trueName			#��ʵ����
	4: SexTypeEnum sex			#�Ա�
	5: i32 age					#����
	6: string nativePlace		#����
}

enum PostVirtualInfoStatus{
	Success,					#�ɹ�
	InvalidUserImage,			#��Ч���û�ͼ��
	InvalidNickName,			#��Ч���ǳ�
	InvalidTrueName,			#��Ч����ʵ����
	InvalidSex,					#��Ч���Ա�
	InvalidAge,					#��Ч������
	InvalidNativePlace,			#��Ч�ļ���
}
#================��������û�����V�û���====================





#=================���ݲ�ѯ���������û��б�=================
struct GetUserInfosBySearchRet{
	1: GetUserInfosBySearchStatus status,	#״̬
	2: list<UserInfo> userInfos				#�û���Ϣ
}
enum GetUserInfosBySearchStatus{
	Success,									#�ɹ� 
	InvalidSex,									#��Ч���Ա�
	InvalidAge,									#��Ч������
	InvalidProfession,							#��Ч��ְҵ
	InvalidNativePlace,							#��Ч�ļ���
	InvalidBelife,								#��Ч������
	InvalidYearOfBirth,							#��Ч������
	InvalidConstellation,						#��Ч������
	NoExist,									#�޼�¼
	InvalidParam,								#��Ч�Ĳ�ѯ����
	InvalidPageIndex,							#��Ч����ʼҳ
	InvalidPageSize,							#��Ч��ҳ��
	InvalidNickName,							#��Ч���ǳ�
}
struct SearchParam{
	1: SearchOptionEnum SearchOption				#ѡ���ѯ����
	2: SexTypeEnum sex								#�Ա�
	3: i32 beginAge									#��������
	4: string constellation							#����
	5:string constellationString					#�����ַ���
	6: string profession							#ְҵ
	7: string professionString						#ְҵ�ַ���
	8: string nativePlace							#����
	9: string nativePlaceString						#�����ַ���
	10: string belife								#����
	11: string belifeString							#�����ַ���
	12: string yearOfBirth							#����
	13: string yearOfBirthString					#�����ַ���
	14: i32 endAge									#��������
	15: string nickName								#�ǳ�
	16: i32 pageIndex								#��ʼҳ
	17: i32 pageSize								#ҳ��
}
enum SearchOptionEnum{
	Sex = 1,					#�û����Ա�
	Age = 2,					#����	
	Profession = 4,				#ְҵ	
	NativePlace = 8,			#����
	Belife = 16,				#����
	YearOfBirth = 32,			#���� 
	Constellation = 64,			#����
	NickName = 128,				#�ǳ�
}
#=================���ݲ�ѯ���������û��б�=================






#===============�����û������豸���ͻ�ȡToken============
enum DeviceTypeEnum{
	iOS=1,									#ƻ���豸
	Android=2								#Android�豸
}

struct GetDeviceTokenRet{
	1: GetDeviceTokenStatus status,		#״̬
	2: string deviceToken				#�豸Token
}

enum GetDeviceTokenStatus{
	Success,							#�ɹ�
	IdentityUserBaseId,					#��Ч���û�����
	IdentityDeviceType,					#��Ч���豸����
}
#===============�����û������豸���ͻ�ȡToken============





#===============����������ȡ�ջ���ַ����============
struct GetReceiveAddressRet{
	1: GetReceiveAddressStatus status,			#״̬
	2: ReceiveAddress receiveAddress			#�û��ջ���ַ����
}

enum GetReceiveAddressStatus{
	Success,									#�ɹ� 
	InvalidId,									#��Ч��Id
}


struct ReceiveAddress{
	1: string id								#����
	2: string userBaseId						#�û�����
	3: string consignee							#�ջ���
	4: string phone								#��ϵ�绰
	5: string areaInfo							#������Ϣ��ʡ�����ֵ���ţ�
	6: string areaInfoString					#������Ϣ��ʡ�����ֵ����ƣ�
	7: string addressName						#�ջ��˵�ַ�������ַ��
	8: bool isDefaultAddress					#�Ƿ���Ĭ�����͵�ַ
}


#===============����������ȡ�ջ���ַ����============



#===============���ݾ�γ�Ⱦ����ȡ�����Χ�ڵ��û��б�============

struct GetUserInfosByPosiParam{
	1: double positionX,
	2: double positionY,
	3: double instance,
	4: i32 pageIndex,
	5: i32 pageSize,
	6: SexTypeEnum sexType,
	7: i32 afterDay=7								#����������N��֮�ڵ�
}


struct GetUserInfosByPositionRet{
	1: GetUserInfosByPositionStatus status,			#״̬
	2: list<UserInfo> userInfos						#�û���Ϣ
}

enum GetUserInfosByPositionStatus{
	Success,								#�ɹ� 
	InvalidPositionX,						#��Ч�ľ���
	InvalidPositionY,						#��Ч��ά��
	InvalidInstance,						#��Ч�ľ���ֵ
	NoExist,								#������
	InvalidPageIndex,						#��Ч����ʼҳ
	InvalidPageSize,						#��Ч��ҳ��
}


#===============���ݾ�γ�Ⱦ����ȡ�����Χ�ڵ��û��б�============



#=================�����û������ȡ��Ϣ=================
struct GetUserInfoRet{
	1: GetUserInfoStatus status,			#״̬
	2: UserInfo userInfo					#�û���Ϣ
}

enum GetUserInfoStatus{
	Success,								#�ɹ� 
	InvalidUserId,							#��Ч���û�����
	NoExist,								#������
}

struct UserInfo {   
	1: string userId						#�û�ID
	2: string nickName						#�ǳ�
    3: string trueName						#��ʵ����			
    4: SexTypeEnum sex						#�Ա�	
	5: string sexString						#�Ա��ַ���
	6: string birthday						#�û��ĳ�������
	7: string phone							#�ֻ���
	8: string userImage						#�û���ͷ��
	9: AccountStateEnum accountState		#�û���Ϣ״̬
	10: string accountStateString			#�û���Ϣ״̬�ַ���
	11: string remark						#��ע��Ϣ
	12: double instance						#����
	13: string OpenId						#OpenId
	14: string lastUpdateLocationDt			#���һ�θ��¾�γ��ʱ��
	15: AgentType agentType					#���������
	16: AuthedType authedType,				#��֤����
	17: string authedPercent,				#��֤�ٷֱ�
	18: i32 age,							#����
	19: string profession,					#ְҵ
	20: string professionString,			#ְҵ�ַ���
	21: string nativePlace,					#����
	22: string nativePlaceString,			#�����ַ���
	23: string createDateTime,				#ע��ʱ��
	24: string loginAccount,				#�˺�
	25: i32 memberLevel,					#��Ա�ȼ�
	26: bool isCancel,						#�˺��Ƿ�ע��
	27: string marketOne,					#һ���г�
	28: string identityCard,				#֤������
}
enum SexTypeEnum {		
	Unknown,							#δ֪
	Man,								#��
	Woman,								#Ů
	All									#ȫ��
}

enum AccountStateEnum {		
	OffLineState,							#����
	OnLineState,							#����
}
#=================�����û������ȡ��Ϣ=================






#=================�����û������ȡ������Ϣ================= 
struct GetUserExtendInfoRet{
	1: GetUserInfoStatus status,			#״̬
	2: UserExtendInfo userExtendInfo		#�û�������Ϣ
}

struct UserExtendInfo {   
	1: string userId								#�û�ID
	2: string yearOfBirth							#����
	3: string constellation							#����
	4: RegisterTypeEnum registerType				#ע������
	5: string registerTypeString					#ע�������ַ���
	6: string nowPlace								#��ǰ���ڵ�
	7: string nowPlaceString						#��ǰ���ڵ��ַ���
	8: string profession							#ְҵ
	9: string professionString						#ְҵ�ַ���
	10: string nativePlace							#����
	11: string nativePlaceString					#�����ַ���
	12: string nation								#����
	13: string nationString							#�����ַ���
	14: string belife								#����
	15: string belifeString							#�����ַ���
	16: string description							#����˵��
	17: string identityCard							#���֤
	18: string passport								#����
	19: string officersCard							#����֤
	20: CertificateTypeEnum certificateType			#֤������
	21: string certificateValue						#֤������ֵ
	22: string email								#���� 
	23: string yearOfBirthString					#�����ַ���
	24: string constellationString					#�����ַ���
	25: i32 memberLevel								#��Ա�ȼ�
}

enum RegisterTypeEnum {								#ע������
	Personal,										#����
	PrivateEnterPrise,								#˽��
	NationalEnterPrise,								#����
	ForeignEnterPrise,								#����
	Organization,									#������֯
}

enum CertificateTypeEnum {							#֤������
	IdentityCard,									#���֤
	Passport,										#����
	OfficersCard,									#����֤
} 
#=================�����û������ȡ������Ϣ=================








#=================�����û���¼����ȡ��Ϣ================= 
struct GetUserPartInfoByAccountRet {
	1: GetUserPartInfoStatus status			#״̬
	2: UserInfo userInfo					#�û���Ϣ
}

enum GetUserPartInfoStatus{
	Success,								#�ɹ�
	InvalidAccount,							#��Ч�ĵ�¼�˺�
	NoExist,								#�˺Ų�����
}
#=================�����û���¼����ȡ��Ϣ=================





#=================�����û������б��ȡ��Ϣ�б�=================
struct GetUserInfosRet{
	1: GetUserInfosStatus status,			#״̬
	2: list<UserInfo> userInfos,			#�û���Ϣ
	3: i32 totalCount
}

enum GetUserInfosStatus{
	Success,								#�ɹ� 
	InvalidUserId,							#��Ч���û�����
	ListMaxCount,							#�����˲�������󳤶ȣ���󳤶�Ϊ50
}
#=================�����û������б��ȡ��Ϣ�б�=================









#=================�޸��û�������״̬=================
enum PutAccountStateStatus{
	Success,								#�ɹ� 
	InvalidUserId,							#��Ч���û�����
	InvalidAccountState,					#��Ч������״̬
}
#=================�޸��û�������״̬================= 







#=================�����û������޸���Ϣ=================
struct UserInfoParam{
	1: PutOptionEnum putOption						#�޸�����
	2: string nickName								#�ǳ�
	3: string trueName								#��ʵ����	
	4: SexTypeEnum sex								#�Ա�	
	5: string birthday								#�û��ĳ�������
	6: string phone									#�ֻ���
	7: string email									#���� 
	8: string nowPlace								#��ǰ���ڵ�
	9: CertificateTypeEnum certificateType			#֤������
	10: string certificateValue						#֤������ֵ
	11: string OpenId								#����Id
}

enum  PutOptionEnum{				#�޸�����
	NickName = 1,					#�ǳ�
	TrueName = 2,					#��ʵ����	
	Sex		= 4,					#�Ա�	
	Birthday = 8,					#�û��ĳ�������
	Phone	= 16,					#�ֻ���
	Email	= 32,					#���� 
	NowPlace	= 64,				#��ǰ���ڵ�
	Certificate	=128,				#֤��
	OpenId=256,						#����Id
}									


enum PutUserInfoStatus{
	Success,								#�ɹ� 
	InvalidUserId,							#��Ч���û�����
	InfoIsNull,								#��ϢΪ��
	InvalidNickName,                        #��Ч���ǳ�
	InvalidTrueName,  						#��Ч����ʵ����	
	InvalidSex,		 						#��Ч���Ա�	
	InvalidBirthday,  						#��Ч���û��ĳ�������
	InvalidPhone,	 						#��Ч���ֻ���
	InvalidEmail,	 						#��Ч������ 
	InvalidNowPlace,						#��Ч�ĵ�ǰ���ڵ�
	InvalidCertificateType,					#��Ч��֤������
	InvalidCertificateValue,				#��Ч��֤��ֵ
	InvalidOpenId,							#����Id		
	
	AuthedTrueName,							#��ʵ��������֤
	AuthedPhone,							#�ֻ�������֤
	AuthedEmail,							#��������֤
	AuthedIdentityCard,						#���֤����֤	
}
#=================�����û������޸���Ϣ================= 




