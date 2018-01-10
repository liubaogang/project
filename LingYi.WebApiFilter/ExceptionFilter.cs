using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;


namespace LingYi.WebApiFilter
{
    public class ExceptionFilter : ExceptionFilterAttribute
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