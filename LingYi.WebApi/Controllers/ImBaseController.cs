using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Collections.Generic;
using Cicada.FileSystem;
using Cicada.Mq;
using Cicada.DI;
using LingYi.DataTransfer.Models.Message;
using LingYi.NetToolClass.ZIP;
using LingYi.WebApiFilter;
using LY.UserAuthorize;
using Newtonsoft.Json;
using Im.Rpc.Redis;
using Im.Rpc.Mysql;
using Cicada;
using Cicada.Configuration;

namespace LingYi.WebApi.Controllers
{
    public class ImBaseController : ApiController
    {
        private readonly ISender _rabbitMqDB;
        private readonly IFileSystem _fileService;
        private readonly IUserContext _userService;
        private readonly RedisService.Iface _redisDB;
        private readonly MysqlService.Iface _mysqlDB;
        public ImBaseController(IFileSystem fileService, IUserContext userService,
                                ISender rabbitMqDB, RedisService.Iface redisDB,
                                MysqlService.Iface mysqlDB)
        {
            _redisDB = redisDB;
            _mysqlDB = mysqlDB;
            _rabbitMqDB = rabbitMqDB;
            _userService = userService;
            _fileService = fileService;
        }

        [HttpGet]
        public string GetGroupMembers(string GroupID)
        {
            return _mysqlDB.GetGroupMembers(GroupID);
        }

        [HttpGet]
        [UserAuthorization]
        public bool SetUnreadNumber(int Count)
        {
            return _redisDB.SetUnreadNumber("7777", Count);
        }

        [HttpGet]
        [UserAuthorization]
        public List<string> GetChatRecords(string FromId, string ToId = null)
        {
            return _mysqlDB.GetChatRecords(FromId, ToId);
        }

        [HttpPost]
        [UserAuthorization]
        public string UserDataSynchronization(string appType)
        {
            var Relation = JsonConvert.DeserializeObject<List<tbl_FriendRelation>>(_mysqlDB.GetFriendRelationList(_userService.UserId));
            var CustomGroup = JsonConvert.DeserializeObject<List<tbl_FriendGroups>>(_mysqlDB.GetFriendGroupList(_userService.UserId));
            if (CustomGroup == null)
                CustomGroup = new List<tbl_FriendGroups>();
            CustomGroup.Add(new tbl_FriendGroups
            {
                GroupID = Guid.Empty.ToString(),
                GroupName = "陌生人",
                GroupCategory = 3,
                GroupType = 0,
                GroupImage = "DefaultImg/sytb/default12.png",
                GroupRemark = "陌生人",
                GroupUserID = _userService.UserId,
                GroupDate = DateTime.Now,
            });
            var InitData = new
            {
                Data = new
                {
                    //*用户版本号要大于变更中最后一个版本号
                    UserVersion = new
                    {
                        UserID = _userService.UserId,
                        Version = 1000
                    },
                    UserDataDict = UserDataDict(),
                    DeskTopIcon = DeskTopIcon(appType),
                    FriendGroup = CustomGroup,
                    FriendList = Relation
                },
                Status = 200,
                Message = "用户数据初始化成功"
            };
            var BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var FileDirectory = BaseDirectory + "UserData\\" + _userService.UserId;
            using (FileStream fs = new FileStream(FileDirectory, FileMode.OpenOrCreate))
            {
                //获得字节数组
                byte[] data = Encoding.Default.GetBytes(JsonConvert.SerializeObject(InitData));
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
            }
            if (File.Exists(FileDirectory))
            {
                //var cipherText = Request.Headers.GetValues("cipherText").ToList()[0];
                //CipherText cipObject = JsonConvert.DeserializeObject<CipherText>(cipherText);
                new ZipHelper().ZipManyFilesOrDictorys
                (
                    new List<string>() { FileDirectory },
                    FileDirectory + ".zip",
                    _userService.UserId
                );
                File.Delete(FileDirectory);
                var FileByte = File.ReadAllBytes(FileDirectory + ".zip");
                var config = CicadaFacade.Container.Resolve<IConfigurationDataRespository>();
                return config.Get("Cicada.FileSystem.UrlPrefix") + _fileService.Upload(FileByte, "zip");
            }
            return string.Empty;
        }

        [HttpPost]
        [UserAuthorization]
        public List<object> UserDictionarysChanging(int version, string appType)
        {
            var UID = Guid.NewGuid().ToString().Replace("-", "");
            var IsProduct = Request.RequestUri.AbsoluteUri.IndexOf("https") >= 0;
            Dictionary<int, object> Changing = new Dictionary<int, object>();
            Changing.Add(1001, new
            {
                //*用户版本号要大于变更中最后一个版本号
                UserVersion = new { UserID = UID, Version = 1001 },
                UserDataDict = new List<tbl_UserDictonary>
                {
                    new tbl_UserDictonary
                    {
                         DataKey= "domain",
                         DataVal= IsProduct ? "0104.lingyi365.com": "ope.lingyi365.com"
                    }
                },
                DeskTopIcon = new List<tbl_DeskTopIcon>
                {
                    new tbl_DeskTopIcon
                    {
                        IconID = 1001,
                        IconType = "upd",
                        IconName = "[资  讯]",
                        IconImage = "imapi/DefaultImg/sytb/default01.png",
                        AppUrl = appType == "ios" ? "LYIMONewsTabBarController" : "news.NewsHomePageActivity",
                        Sort = 1
                    }
                }
            });
            Changing.Add(1002, new
            {
                //*用户版本号要大于变更中最后一个版本号
                UserVersion = new { UserID = UID, Version = 1002 },
                DeskTopIcon = new List<tbl_DeskTopIcon>
                {
                     new tbl_DeskTopIcon
                     {
                        IconID = 1018,
                        IconType = "upd",
                        IconName = "工作消息",
                        IconImage = "DefaultImg/sytb/default44.png",
                        AppUrl = appType == "ios" ? "LYIMOWorkAllEnterpriseMsgListController" : "work.message.gather",
                        Sort = 12
                     }
                }
            });

            return Changing.Where(w => w.Key > version).Select(s => s.Value).ToList();
        }
        [NonAction]
        public List<tbl_UserDictonary> UserDataDict()
        {
            var IsProduct = Request.RequestUri.AbsoluteUri.IndexOf("https") >= 0;
            var UserData = new List<tbl_UserDictonary>()
            {
                new tbl_UserDictonary
                {
                    DataKey="domain",
                    DataVal=IsProduct ? "0104.lingyi365.com": "ope.lingyi365.com"
                },
                new tbl_UserDictonary
                {
                    DataKey="apiPrefix",
                    DataVal=IsProduct ? "https://01.lingyi365.com/im/":"http://ope.lingyi365.com/im/"
                },
                new tbl_UserDictonary
                {
                     DataKey="imgOldPrefix",
                     DataVal=IsProduct?"https://01.lingyi365.com/imapi/":"http://ope.lingyi365.com:5608/imapi/"
                },
                new tbl_UserDictonary
                {
                    DataKey="imgPrefix",
                    DataVal=IsProduct ? "https://01.lingyi365.com/img/":"http://ope.lingyi365.com/img/"
                },
                new tbl_UserDictonary
                {
                    DataKey="filePrefix",
                    DataVal=IsProduct ? "https://01.lingyi365.com:5608/fs/":"http://ope.lingyi365.com:5608/fs/"
                },
            };
            return UserData;
        }

        [NonAction]
        public List<tbl_DeskTopIcon> DeskTopIcon(string appType)
        {
            //有版本变更这里原始数据要修改，否则用户第一次同步出错
            List<tbl_DeskTopIcon> IconList = new List<tbl_DeskTopIcon>();
            #region================= Load List Data =====================
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1001,
                IconType = "add",
                IconName = "资  讯",
                IconImage = "DefaultImg/sytb/default01.png",
                AppUrl = appType == "ios" ? "LYIMONewsTabBarController" : "news.NewsHomePageActivity",
                Sort = 1
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1002,
                IconType = "add",
                IconName = "微  博",
                IconImage = "imapi/DefaultImg/sytb/default02.png",
                AppUrl = appType == "ios" ? "LYIMOWeiBoController" : "news.MyWeiBoActivity",
                Sort = 2
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1003,
                IconType = "add",
                IconName = "小视频",
                IconImage = "DefaultImg/sytb/default36.png",
                AppUrl = appType == "ios" ? "LYIMOmicroSmallVideoController" : "news.MicroSmallVideoActivity",
                Sort = 3
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1004,
                IconType = "add",
                IconName = "它网链接",
                IconImage = "DefaultImg/sytb/default08.png",
                AppUrl = appType == "ios" ? "LYIMOOtherlinkController" : "news.LianJieActivity",
                Sort = 4
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1005,
                IconType = "add",
                IconName = "加我的人",
                IconImage = "DefaultImg/sytb/default37.png",
                AppUrl = appType == "ios" ? "LYIMOMessageNewFriendMainController" : "message.NewFriendRequestListActivity",
                Sort = -1
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1006,
                IconType = "add",
                IconName = "附近的人",
                IconImage = "DefaultImg/sytb/default05.png",
                AppUrl = appType == "ios" ? "LYIMOPeopleNearbyGroupController" : "message.FuJinActivity",
                Sort = -1
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1007,
                IconType = "add",
                IconName = "朋友圈",
                IconImage = "DefaultImg/sytb/default07.png",
                AppUrl = appType == "ios" ? "LYIMOMessageDiscoverHomeController" : "message.FriCircleMainActivity",
                Sort = -1
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1008,
                IconType = "add",
                IconName = "邮  箱",
                IconImage = "DefaultImg/sytb/default03.png",
                AppUrl = appType == "ios" ? "LYIMOEmailMainController" : "email.EmailMainActivity",
                Sort = 5
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1009,
                IconType = "add",
                IconName = "我的订阅",
                IconImage = "DefaultImg/sytb/default06.png",
                AppUrl = appType == "ios" ? "LYIMOMessageSubscribeController" : "news.MyDingYueActivity",
                Sort = -1
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1010,
                IconType = "add",
                IconName = "QQ快捷",
                IconImage = "DefaultImg/sytb/default22.png",
                AppUrl = appType == "ios"
                            ? "https://itunes.apple.com/cn/app/qq-2011/id444934666?mt=8"
                            : "https://qd.myapp.com/myapp/qqteam/AndroidQQ/mobileqq_android.apk",
                Mark = "com.tencent.mobileqq,com.tencent.mobileqq.activity.SplashActivity,mqq://",
                Sort = 6
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1011,
                IconType = "add",
                IconName = "微信快捷",
                IconImage = "DefaultImg/sytb/default21.png",
                AppUrl = appType == "ios"
                        ? "https://itunes.apple.com/cn/app/wei/id414478124"
                        : "http://dldir1.qq.com/weixin/android/weixin6325android861.apk",
                Mark = "com.tencent.mm,com.tencent.mm.ui.LauncherUI,weixin://",
                Sort = 7
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1012,
                IconType = "add",
                IconName = "支付宝快捷",
                IconImage = "DefaultImg/sytb/default31.png",
                AppUrl = appType == "ios"
                        ? "https://itunes.apple.com/cn/app/zhi-fu-bao-kou-bei-sheng-huo/id333206289?mt=8"
                        : "http://m.shouji.360tpcdn.com/160908/fee2d2c787172108085e1e87101d9cba/com.eg.android.AlipayGphone_99.apk",
                Mark = "com.eg.android.AlipayGphone,com.alipay.mobile.quinox.LauncherActivity,alipay://",
                Sort = 8
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1013,
                IconType = "add",
                IconName = "直  播",
                IconImage = "DefaultImg/sytb/default39.png",
                AppUrl = appType == "ios" ? "LYIMOLiveRoomHomeController" : "liveshow.LiveListActivity",
                Sort = -1
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1014,
                IconType = "add",
                IconName = "〇壹客服",
                IconImage = "DefaultImg/sytb/default40.png",
                AppUrl = appType == "ios" ? "LYIMOIMCustomerServiceController" : "message.CustomerServiceActivity",
                Sort = 9
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1015,
                IconType = "add",
                IconName = "〇壹讲堂",
                IconImage = "DefaultImg/sytb/default41.png",
                AppUrl = appType == "ios" ? "LYIMOLiveForumListController" : "linyilectureroom.LectureRoomListActivity",
                Sort = 10
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1016,
                IconType = "add",
                IconName = "公众号",
                IconImage = "DefaultImg/sytb/default42.png",
                AppUrl = appType == "ios" ? "LYIMOWorkFocusAlreadyFocusListController" : "work.oa.UserPubNoList",
                Sort = -1
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1017,
                IconType = "add",
                IconName = "电商消息",
                IconImage = "DefaultImg/sytb/default43.png",
                AppUrl = appType == "ios" ? "LYIMOOnRetailerMessageRootController" : "com.lingyi.pli.activity.mall.MallMessageAty",
                Sort = 11
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1018,
                IconType = "add",
                IconName = "工作消息",
                IconImage = "DefaultImg/sytb/default44.png",
                AppUrl = appType == "ios" ? "LYIMOWorkAllEnterpriseMsgListController" : "work.message.gather",
                Sort = 12
            });
            IconList.Add(new tbl_DeskTopIcon
            {
                IconID = 1019,
                IconType = "add",
                IconName = "收支消息",
                IconImage = "DefaultImg/sytb/default47.png",
                AppUrl = appType == "ios" ? "LYIMORevenueAndExpenditureMessageListController" : "com.lingyi.pli.activity.mall.MallPayMessageAty",
                Sort = -1
            });
            #endregion     
            return IconList;
        }
    }
}
