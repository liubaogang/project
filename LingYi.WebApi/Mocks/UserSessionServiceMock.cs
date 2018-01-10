using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LingYi.WebApi.Mocks
{
	/// <summary>
    /// 用户会话服务模拟类
    /// </summary>
    public class UserSessionServiceMock : UserSessionService.Iface
    {
        /// <summary>
        /// 创建会话
        /// </summary>
        /// <param name="userId">用户编码</param>
        /// <returns>结果</returns>
        public CreateSessionRet CreateSession(string userId)
        {
            throw new NotImplementedException();
        }

        public CreateSessionRet CreateSession(string userId, int userType, string loginFrom)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据会话编码获取用户编码
        /// </summary>
        /// <param name="sessionId">会话编码</param>
        /// <returns>用户编码相关信息</returns>
        public GetUserIdRet GetUserId(string sessionId)
        {
            return new GetUserIdRet
            {
                Status = GetUserIdStatus.Success,
                UserId = sessionId,
            };
        }

        /// <summary>
        /// 删除会话
        /// </summary>
        /// <param name="sessionId">会话编码</param>
        /// <returns>结果</returns>
        public RemoveSessionStatus RemoveSession(string sessionId)
        {
            throw new NotImplementedException();
        }
    }
}