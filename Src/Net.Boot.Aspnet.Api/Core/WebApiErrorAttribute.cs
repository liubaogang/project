namespace Net.Boot.Aspnet.Api.Core
{
    using System;
    using System.Web.Http.Filters;
    internal class WebApiErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
            //LogBuilder.Create("Cicada.Boot.Aspnet.WebApi").Error(actionExecutedContext.Exception, "WebApi异常");
        }
    }
}
