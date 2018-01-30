namespace Net.Boot.Aspnet.Api.Core
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    internal class WebApiTrackAttribute : ActionFilterAttribute
    {
        private const string ActionExecuteTimeKey = "Cicada.WebApiActionExecuteTimeKey";
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            object obj2;
            int? nullable = null;
            if (actionExecutedContext.Request.Properties.TryGetValue(ActionExecuteTimeKey, out obj2))
            {
                DateTime time = DateTime.FromBinary(Convert.ToInt64(obj2));
                TimeSpan span = (TimeSpan)(DateTime.Now - time);
                nullable = new int?((int)span.TotalMilliseconds);
            }
            //object[] args = new object[] { actionExecutedContext.Request.RequestUri, actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName, actionExecutedContext.ActionContext.ActionDescriptor.ActionName, this.GetResponseContent(actionExecutedContext), nullable.HasValue ? nullable.Value.ToString(CultureInfo.InvariantCulture) : "[未知]" };
            //Log.Info("响应操作 url：{0}  控制器名称：{1} 动作：{2} 结果：{3} 执行时间:{4}毫秒", args);
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            actionContext.Request.Properties[ActionExecuteTimeKey] = DateTime.Now.ToBinary();
            //object[] args = new object[] { actionContext.Request.RequestUri, actionContext.ControllerContext.ControllerDescriptor.ControllerName, actionContext.ActionDescriptor.ActionName, this.GetRequestContent(actionContext) };
            //Log.Info("请求操作 url：{0}  控制器名称：{1} 动作：{2} 数据：{3}", args);
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}
