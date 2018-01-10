namespace LingYi.GetPushMessage.Server
{
    using Cicada;
    using Cicada.DI;
    using Cicada.Log;
    using LingYi.GetPushMessage.IServer;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    [ComponentAttribute("XiaoMiPush", Lifetime = Lifetime.Singleton)]
    public class MessagePush_XiaoMi : IMessagePush
    {
        private Task ParnetTask;
        private bool IsStop = false;
        private string MI_APPSECRET = "fFGm+xy244K4iZtdv9SS1w==";
        private string MI_PUSH_URL = "https://api.xmpush.xiaomi.com/v2/message/regid";
        private static readonly ILog Log = CicadaFacade.CreateLog<Service>();

        public static Queue<string> Queue { get; set; }
        public MessagePush_XiaoMi()
        {
            Queue = new Queue<string>();
        }

        public void Begin()
        {
            ParnetTask = Task.Factory.StartNew(() =>
            {
                do
                {
                    int times = 0;
                    List<string> DeviceTokenList = new List<string>();
                    try
                    {
                        while (Queue.Count > 0 && (times++) < 200)
                        {
                            DeviceTokenList.Add(Queue.Dequeue());
                        }
                        if (DeviceTokenList.Count > 0)
                        {
                            PushMsgToMiClient(DeviceTokenList);
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

        public void PushMsgToMiClient(List<string> deviceTokenList)
        {
            if (!string.IsNullOrWhiteSpace(MI_APPSECRET) && null != deviceTokenList)
            {
                if (deviceTokenList.Count > 0)
                {
                    var pushBody = "您收到了一条消息";
                    var deviceToken = string.Join(",", deviceTokenList);
                    var encode = Encoding.GetEncoding("utf-8");
                    byte[] buf = encode.GetBytes("description=1&title=1&payload=" + HttpUtility.UrlEncode(pushBody, Encoding.UTF8)
                                                + "&restricted_package_name=" + HttpUtility.UrlEncode("com.lingyi.pli.activity", Encoding.UTF8)
                                                + "&registration_id=" + HttpUtility.UrlEncode(deviceToken, Encoding.UTF8) + "&notify_type="
                                                + HttpUtility.UrlEncode("2", Encoding.UTF8) + "&notify_id=" + HttpUtility.UrlEncode("-1", Encoding.UTF8)
                                                + "&time_to_live=1000&pass_through=" + HttpUtility.UrlEncode("1", Encoding.UTF8));
                    try
                    {
                        WebRequest hr = HttpWebRequest.Create(MI_PUSH_URL);
                        hr.Headers.Add("Authorization", "key=" + MI_APPSECRET);
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
                                    var json = JsonConvert.DeserializeObject(reader.ReadToEnd());
                                    CicadaFacade.CreateLog<ILog>().Info(json.ToString());
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("===============Begin==============");
                        Log.Error("Error:" + ex.Message);
                        Log.Error("Params1:" + deviceToken);
                        Log.Error("Params2:" + pushBody);
                        Log.Error("================End===============");
                    }
                }
            }
        }
    }
}
