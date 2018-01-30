using Net.Base;
using Net.Core;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace Net.Boot.Aspnet.Api.Core
{
    internal class ApiDependencyResolver : IDependencyResolver, IDependencyScope, IDisposable
    {
        private IContainer _container;
        private bool _disposed;

        public ApiDependencyResolver() : this(ContainerSingleton.Instance)
        {
        }

        public ApiDependencyResolver(IContainer container)
        {
            Guard.ThrowIfArgumentIsNull(container, "container");
            this._container = container;
        }

        public IDependencyScope BeginScope()
        {
            return new ApiDependencyResolver(_container.CreateChildContainer());
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing && (this._container != null))
                {
                    this._container.Dispose();
                    this._container = null;
                }
                this._disposed = true;
            }
        }

        ~ApiDependencyResolver()
        {
            this.Dispose(false);
        }

        public object GetService(Type serviceType)
        {
            return this._container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._container.ResolveAll(serviceType);
        }
    }
}
