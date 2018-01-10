using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace LingYi.WebApiFilter
{
    public class ValidateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string ValidateErorInfo = string.Empty, DecryptText = string.Empty;
            if (actionContext.ModelState.IsValid == false)
            {
                // 在响应体中返回验证错误
                var errors = new Dictionary<string, IEnumerable<string>>();
                foreach (KeyValuePair<string, ModelState> keyValue in actionContext.ModelState)
                {
                    errors[keyValue.Key] = keyValue.Value.Errors.Select(e => e.ErrorMessage);
                }
                ValidateErorInfo = "表单异常:" + JsonConvert.SerializeObject(errors);
            }
            else
            {
                //if (actionContext.Request.Headers.Contains("cipherText"))
                //{
                //    var cipherText = actionContext.Request.Headers.GetValues("cipherText");
                //    using (var rsa = new RSACryptoServiceProvider(2048))
                //    {
                //        try
                //        {
                //            string[] Keys = { "secret", "params", "timestamp" };
                //            var RsaFilePath = AppDomain.CurrentDomain.BaseDirectory + "App_Data";
                //            rsa.FromXmlString(File.ReadAllText(RsaFilePath + "\\RSA.Private"));
                //            var BytesEncrypted = Convert.FromBase64String(cipherText.ToList()[0]);
                //            var BytesPlainText = rsa.Decrypt(BytesEncrypted, false);
                //            DecryptText = Encoding.Unicode.GetString(BytesPlainText);
                //            var OriginValue = JsonConvert.DeserializeObject<Dictionary<string,object>>(DecryptText);
                //            if (OriginValue.Keys.Where(w => Keys.Contains(w)).Count() == 3 && DecryptText.IndexOf(Keys[0]) >= 0)
                //            {
                //                var ParamsValue = new Dictionary<string, string>();
                //                if (actionContext.Request.Method == HttpMethod.Get)
                //                {
                //                    ParamsValue = actionContext.ActionArguments.ToDictionary(k => k.Key, v => v.Value.ToString());
                //                }
                //                else if (actionContext.Request.Method == HttpMethod.Post)
                //                {
                //                    var OriginVal = actionContext.ActionArguments.FirstOrDefault().Value;
                //                    var NewsValue = JsonConvert.SerializeObject(OriginVal);
                //                    ParamsValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(NewsValue);
                //                }
                //                foreach (var dic in OriginValue)
                //                {
                //                    if (string.IsNullOrWhiteSpace(ValidateErorInfo))
                //                    {
                //                        switch (dic.Key.ToLower())
                //                        {
                //                            case "secret":
                //                                {

                //                                }; break;
                //                            //case "params":
                //                            //    {
                //                            //        var val = dic.Value;
                //                            //        var pv = JsonConvert.DeserializeObject<Dictionary<string, string>>(val.ToString());
                //                            //        if (ParamsValue.Intersect(pv).Count() != pv.Count)
                //                            //            ValidateErorInfo = "参数异常:非法参数，ERROR002";
                //                            //    }; break;
                //                            case "timestamp":
                //                                {
                //                                    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                //                                    long longTime = long.Parse(dic.Value + "0000");
                //                                    var FirstRequestDate = dtStart.Add(new TimeSpan(longTime));
                //                                    if (DateTime.Now.Subtract(FirstRequestDate).Minutes > 3)
                //                                        ValidateErorInfo = "参数异常:非法参数，ERROR003";
                //                                }; break;
                //                        }
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                ValidateErorInfo = "参数异常:无效参数，ERROR004";
                //            }
                //        }
                //        catch
                //        {
                //            ValidateErorInfo = "参数异常:非法请求，ERROR000";
                //        }
                //        finally
                //        {
                //            rsa.PersistKeyInCsp = false;
                //        }
                //    }                   
                //}
                //else
                //{
                //    ValidateErorInfo = "参数异常:未加密，ERROR001";
                //}
            }
            if (string.IsNullOrWhiteSpace(ValidateErorInfo))
            {
                //actionContext.Request.Headers.Remove("cipherText");
                //actionContext.Request.Headers.Add("cipherText", DecryptText);
            }
            else
            {
                var ErrorInfo = JsonConvert.SerializeObject(new
                {
                    Data = new string[0],
                    Status = 403,
                    Message = ValidateErorInfo
                });
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = new StringContent(ErrorInfo, Encoding.GetEncoding("UTF-8"))
                };
            }
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            if (null != actionContext.Response)
            {
                if (actionContext.Response.StatusCode == HttpStatusCode.OK)
                {
                    var hadersType = actionContext.Response.Content.Headers.ToString();
                    if (!(hadersType.ToLower().IndexOf("application/octet-stream") >= 0))
                    {
                        actionContext.Response =
                        actionContext.Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            Data =actionContext.ActionContext.Response.Content.ReadAsAsync<object>().Result,
                            Status = 200,
                            Message = "成功加载数据"
                        });
                    }
                }
            }
            base.OnActionExecuted(actionContext);
        }
    }

    public class CipherText
    {
        public string secret { get; set; }
        public Dictionary<string,string> @params { get; set; }
        public string timestamp { get; set; }
    }
}