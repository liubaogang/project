namespace Net.Boot.Aspnet.Api.Core
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;
    internal class WebApiErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response =
            actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new
            {
                Data = new string[0],
                Status = 500,
                Message = "处理异常:" + actionExecutedContext.Exception.Message
            });
        }
    }
}
