namespace Net.Boot.Aspnet.Mvc
{
    using System.Linq;
    using System.Web.Mvc;
    using Net.Core;
    using Net.Boot.Aspnet.Mvc.Core;

    internal class ModuleType : IModuleType
    {
        public void Execute(IConfigsType dictData)
        {
            FilterAttributeFilterProvider item = FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().FirstOrDefault();
            if (item != null)
            {
                FilterProviders.Providers.Remove(item);
            }
            FilterProviders.Providers.Add(new MvcFilterAttributeProvider());
            HandleErrorAttribute filter = GlobalFilters.Filters.OfType<HandleErrorAttribute>().FirstOrDefault();
            if (filter != null)
            {
                GlobalFilters.Filters.Remove(filter);
            }
            GlobalFilters.Filters.Add(new MvcHandleErrorAttribute());
            DependencyResolver.SetResolver((IDependencyResolver)new MvcDependencyResolver());
        }
    }
}
