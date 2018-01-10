namespace LingYi.GetPushMessage.Server
{
    using Cicada;
    using Cicada.Configuration;
    using Cicada.DI;
    using Cicada.Log;
    using LingYi.GetPushMessage.IServer;
    using NetToolClass.Base;
    using NetToolClass.Redis;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    [ComponentAttribute("HuaWeiPush", Lifetime = Lifetime.Singleton)]
    public class MessagePush_HuaWei : IMessagePush
    {
        private Task ParnetTask;
        private bool IsStop = false;
        private static string ACCESS_TOKEN;
        private static string HuaWei_APPID = "10727050";
        private static string HuaWei_APPSECRET = "ce7aacd3a99f3667e93c9de1aa19d789";
        private static string HuaWei_TokenUrl = "https://login.vmall.com/oauth2/token";
        private static string HuaWei_PushUrl = "https://api.push.hicloud.com/pushsend.do";
        private static readonly ILog Log = CicadaFacade.CreateLog<Service>();

        public static Queue<string> Queue { get; set; }
        public MessagePush_HuaWei()
        {
            var cicada = CicadaFacade.Container.Resolve<IConfigurationDataRespository>();
            RedisDB._redisIP = cicada.Get("Cicada.Cache.Redis.Server");
            Queue = new Queue<string>();
        }

        public void Begin()
        {
            GetAccess_Token();
            ParnetTask = Task.Factory.StartNew(() =>
            {
                do
                {
                    int times = 0;
                    List<string> DeviceTokenList = new List<string>();
                    try
                    {
                        while (Queue.Count > 0 && (times++) < 500)
                        {
                            DeviceTokenList.Add(Queue.Dequeue());
                        }
                        if (DeviceTokenList.Count > 0)
                        {
                            PushMsgToHuaWeiClient(DeviceTokenList);
                        }
                    }
                    catch
                    {
                        if (DeviceTokenList.Count > 0)
                        {
                            foreach (string item in DeviceTokenList)
                            {
                                Queue.Enqueue(item);
                            }
                        }
                    }
                    finally
                    {
                        if (Queue.Count > 0)
                            System.Threading.Thread.Sleep(100);
                        else
                            System.Threading.Thread.Sleep(500);
                    }
                } while (!IsStop || Queue.Count > 0);
            });
        }

        public void End()
        {
            IsStop = true;
            Task.WaitAll(ParnetTask);
        }

        public void Push(string deviceToken, string pushBody)
        {
            Queue.Enqueue(deviceToken);
        }

        private void PushMsgToHuaWeiClient(List<string> deviceTokenList)
        {
            var PayLoad = new
            {
                hps = new
                {
                    msg = new
                    {
                        type = 3,
                        body = new
                        {
                            content = "您收到一条消息",
                            title = "〇壹"
                        },
                        action = new
                        {
                            type = 3,
                            param = new
                            {
                                appPkgName = "com.lingyi.pli.activity"
                            }
                        }
                    }
                }
            };
            var PostBody = string.Format("access_token={0}&nsp_svc={1}&nsp_ts={2}&device_token_list={3}&payload={4}",
                                        HttpUtility.UrlEncode(ACCESS_TOKEN, Encoding.UTF8),
                                        HttpUtility.UrlEncode("openpush.message.api.send", Encoding.UTF8),
                                        HttpUtility.UrlEncode(Base.ConvertTimestamp(DateTime.Now), Encoding.UTF8),
                                        HttpUtility.UrlEncode(JsonConvert.SerializeObject(deviceTokenList), Encoding.UTF8),
                                        HttpUtility.UrlEncode(JsonConvert.SerializeObject(PayLoad), Encoding.UTF8));
            var HuWei_NspCtx = HttpUtility.UrlEncode("{\"ver\":\"1\", \"appId\":\"" + HuaWei_APPID + "\"}", Encoding.UTF8);
            var Result = RequestData(HuaWei_PushUrl + "?nsp_ctx=" + HuWei_NspCtx, PostBody);
        }

        private void GetAccess_Token()
        {
            if (!RedisDB.KeyExists("HuaWeiAccessToken"))
            {
                //获取华为推送accesstoken
                var ReturnVal = RequestData(HuaWei_TokenUrl, "grant_type=client_credentials&client_secret="
                                            + HuaWei_APPSECRET + "&client_id=" + HuaWei_APPID);
                var JObjectVal = ((JObject)JsonConvert.DeserializeObject(ReturnVal));
                if (JObjectVal["access_token"] != null)
                {
                    ACCESS_TOKEN = JObjectVal["access_token"].ToString();
                    var TimeOut = Convert.ToInt32(JObjectVal["expires_in"]) / 60;
                    RedisDB.StringSet("HuaWeiAccessToken", ACCESS_TOKEN, TimeOut);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(ACCESS_TOKEN))
                    ACCESS_TOKEN = RedisDB.StringGet("HuaWeiAccessToken");
            }
        }

        private string RequestData(string requestUrl, string requestPar)
        {
            var encode = Encoding.GetEncoding("utf-8");
            byte[] buf = encode.GetBytes(requestPar);
            try
            {
                WebRequest hr = HttpWebRequest.Create(requestUrl);
                hr.ContentType = "application/x-www-form-urlencoded";
                hr.ContentLength = buf.Length;
                hr.Method = "POST";
                using (Stream RequestStream = hr.GetRequestStream())
                {
                    RequestStream.Write(buf, 0, buf.Length);
                    using (WebResponse response = hr.GetResponse())
                    {
                        var stream = response.GetResponseStream();
                        using (StreamReader reader = new StreamReader(stream, encode))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("===============Begin==============");
                Log.Error("Error:" + ex.Message);
                Log.Error("Params1:" + requestUrl);
                Log.Error("Params2:" + requestPar);
                Log.Error("================End===============");
                var Result = new { error = "0000", error_description = "Error:灵铱请求错误！" };
                return JsonConvert.SerializeObject(Result);
            }
        }
    }
}
