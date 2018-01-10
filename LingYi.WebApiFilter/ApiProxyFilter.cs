using System.Net;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Collections.Generic;

namespace LingYi.WebApiFilter
{
    public class ApiProxyFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string ValidateErorInfo = string.Empty;
            if (actionContext.Request.Method == HttpMethod.Post)
            {
                var OriginVal = actionContext.ActionArguments.FirstOrDefault();
                var ParamsValue = OriginVal.Value as List<RequestData>;
                if (ParamsValue.Count > 0 && ParamsValue.Count < 4)
                {
                    var encode = Encoding.GetEncoding("utf-8");
                    foreach(var ReqData in ParamsValue)
                    {
                        switch(ReqData.HttpMethod.ToUpper())
                        {
                            case "GET":
                                {
                                    WebRequest hr = HttpWebRequest.Create(ReqData.Url);
                                    hr.Headers.Add("Authentication", "");
                                    hr.ContentType = "application/json";
                                    hr.Method = ReqData.HttpMethod.ToUpper();
                                    hr.Timeout = 20000;
                                    using (WebResponse response = hr.GetResponse())
                                    {
                                        var stream = response.GetResponseStream();
                                        using (StreamReader reader = new StreamReader(stream, encode))
                                        {
                                            reader.ReadToEnd();
                                        }
                                    }
                                };break;
                            case "POST":
                                {
                                    WebRequest hr = HttpWebRequest.Create(ReqData.Url);
                                    byte[] buf = encode.GetBytes(JsonConvert.SerializeObject(ReqData.RequestParam));
                                    hr.Headers.Add("Authentication", "");
                                    hr.ContentType = "application/json";
                                    hr.ContentLength = buf.Length;
                                    hr.Method = ReqData.HttpMethod.ToUpper();
                                    hr.Timeout = 20000;
                                    using (Stream RequestStream = hr.GetRequestStream())
                                    {
                                        RequestStream.Write(buf, 0, buf.Length);
                                        using (WebResponse response = hr.GetResponse())
                                        {
                                            var stream = response.GetResponseStream();
                                            using (StreamReader reader = new StreamReader(stream, encode))
                                            {
                                                reader.ReadToEnd();
                                            }
                                        }
                                    }
                                };break;
                        }
                    }
                }
            }
            else
            {
                ValidateErorInfo = "请求异常:类型应为POST，ERROR005";
            }
            base.OnActionExecuting(actionContext);
        }
    }


    #region 请求对象
    /// <summary>
    /// 数据包中的实体
    /// </summary>
    public class RequestData
    {
        public RequestData()
        {
            this.RequestParam = new Dictionary<string, string>();
        }
        /// <summary>
        /// 本次通讯唯一标示
        /// </summary>
        public string GuidKey { get; set; }
        /// <summary>
        /// 请求方式0:get,1:post
        /// </summary>
        public string HttpMethod { get; set; }
        /// <summary>
        /// 要调用的方法
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 方法的参数列表
        /// </summary>
        public IDictionary<string, string> RequestParam { get; set; }
    }
    #endregion


    #region 响应对象
    /// <summary>
    /// 数据包实体
    /// </summary>
    public class ResponseData
    {
        /// <summary>
        /// 本次传输过程中唯一标识
        /// </summary>
        public string GuidKey { get; set; }
        /// <summary>
        /// 状态：100失败，200成功
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 数据包：Json对象
        /// </summary>
        public string Data { get; set; }
    }
    #endregion
    
}