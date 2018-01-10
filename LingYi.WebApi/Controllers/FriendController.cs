using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Cicada.Mq;
using System.Web.Http;
using LY.UserAuthorize;
using LingYi.DataTransfer.Models.Message;

namespace LingYi.WebApi.Controllers
{
    [UserAuthorization]
    public class FriendController : ApiController
    {
        private readonly ISender _rabbitDB;
        private readonly IUserContext _userContext;
        public FriendController(ISender rabbitDB, IUserContext userContext)
        {
            _rabbitDB = rabbitDB;
            _userContext = userContext;
        }
        [HttpPost]
        public tbl_FriendGroups GroupInsert(tbl_FriendGroups group)
        {
            group.GroupID = Guid.NewGuid().ToString();
            group.GroupUserID = _userContext.UserId;
            group.GroupDate = DateTime.Now;
            _rabbitDB.Send("Im-FriendController", new
            {
                ParamsType = "GroupInsert",
                ParamsData = group
            });
            return group;
        }
        [HttpPost]
        public bool GroupUpdate(tbl_FriendGroups group)
        {
            _rabbitDB.Send("Im-FriendController", new
            {
                ParamsType = "GroupUpdate",
                ParamsData = group
            });
            return true;
        }
        [HttpGet]
        public bool GroupDelete(string groupID)
        {
            _rabbitDB.Send("Im-FriendController", new
            {
                ParamsType = "GroupDelete",
                ParamsData = groupID
            });
            return true;
        }

        [HttpPost]
        public bool AuthorityInsert(List<Dictionary<string,string>> authorityList,string userID)
        {
            _rabbitDB.Send("Im-FriendController", new
            {
                ParamsType = "AuthorityInsert",
                ParamsData = new
                {
                    FromUserID = _userContext.UserId,
                    ToUserID = userID,
                    Authority = authorityList
                }
            });
            return true;
        }
    }
}
