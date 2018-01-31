namespace Net.Boot.Aspnet.Mvc.Core
{
    using Net.Base;
    using Net.Core;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    internal class MvcDependencyResolver: IDependencyResolver
    {
        private readonly IContainer _container;

        public MvcDependencyResolver() 
            : this(ContainerSingleton.Instance)
        {
        }

        public MvcDependencyResolver(IContainer container)
        {
            Guard.ThrowIfArgumentIsNull(container, "container");
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType);
        }
    }
}
