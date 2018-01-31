using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Net.Boot.Aspnet.Mvc.Core
{
    internal class MvcFilterAttributeProvider: FilterAttributeFilterProvider
    {
        protected override IEnumerable<FilterAttribute> GetActionAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return base.GetActionAttributes(controllerContext, actionDescriptor);
        }

        protected override IEnumerable<FilterAttribute> GetControllerAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return base.GetControllerAttributes(controllerContext, actionDescriptor);
        }
    }
}
