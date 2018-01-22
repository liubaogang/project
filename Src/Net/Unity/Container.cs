using Net.Base;
using Net.Core;
using System;
using System.Collections.Generic;
using Unity;
using Unity.Exceptions;
using Unity.Lifetime;

namespace Net.Unity
{
    public class Container : IContainer
    {
        private IUnityContainer _container;
        private bool _disposed;

        public Container()
            : this(new UnityContainer())
        {

        }

        public Container(IUnityContainer container)
        {
            //Guard.ThrowIfArgumentIsNull(container, "container");
            _container = container;
        }


        public IContainer CreateChildContainer()
        {
            var childContainer = _container.CreateChildContainer();
            return new Container(childContainer);
        }

        public IContainer RegisterInstance(Type t, string name, object instance)
        {
            _container.RegisterInstance(t, name, instance);
            return this;
        }

        public IContainer RegisterType(Type from, Type to, string name, LifeTime lifeTime = LifeTime.PerResolve)
        {
            LifetimeManager lifetimeManager;
            switch (lifeTime)
            {
                case LifeTime.PerResolve:
                    lifetimeManager = new PerResolveLifetimeManager();
                    break;
                case LifeTime.Singleton:
                    lifetimeManager = new ContainerControlledLifetimeManager();
                    break;
                default:
                    throw new NotImplementedException();
            }
            _container.RegisterType(from, to, name, lifetimeManager);
            return this;
        }

        public object Resolve(Type t, string name)
        {
            try
            {
                return (t.IsClass && !t.IsAbstract) || _container.IsRegistered(t, name)
                    ? _container.Resolve(t, name)
                    : null;
            }
            catch (ResolutionFailedException e)
            {
                var Message = string.Format("实例化类型失败，类型为:{0},名称为:{1}", t, name);
                throw new InvalidOperationException(Message, e);
            }
        }

        public IEnumerable<object> ResolveAll(Type t)
        {
            return _container.ResolveAll(t);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                if (_container != null)
                {
                    _container.Dispose();
                    _container = null;
                }
            }
            _disposed = true;
        }

        ~Container()
        {
            Dispose(false);
        }
    }
}
