using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Net.Boot.Aspnet.Mvc.Core
{
    internal class MvcHandleErrorAttribute: HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            //LogBuilder.Create("Cicada.Boot.Aspnet.Mvc").Error(filterContext.Exception, "全局异常");
        }
    }
}
