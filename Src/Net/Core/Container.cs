namespace Net.Core
{
    using Microsoft.Practices.Unity;
    using Net.Attri;
    using Net.Base;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    #region Container       
    public class Container : IContainer, IDisposable
    {
        private IUnityContainer _container;
        private bool _disposed;

        public Container(): this(new UnityContainer())
        {

        }

        public Container(IUnityContainer container)
        {
            Guard.ThrowIfArgumentIsNull(container, "container");
            _container = container;
        }


        public IContainer CreateChildContainer()
        {
            var childContainer = _container.CreateChildContainer();
            return new Container(childContainer);
        }

        public IContainer RegisterInstance(Type t, string name, object instance)
        {
            UnityContainerExtensions.RegisterInstance(this._container, t, name, instance);
            return this;
        }

        public IContainer RegisterType(Type from, Type to, string name, LifeTime lifeTime = LifeTime.PerResolve)
        {
            LifetimeManager manager;
            if (lifeTime != LifeTime.PerResolve)
            {
                if (lifeTime != LifeTime.Singleton)
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                manager = new PerResolveLifetimeManager();
                goto Label_0021;
            }
            manager = new ContainerControlledLifetimeManager();
            Label_0021:
            this._container.RegisterType(from, to, name, manager, new InjectionMember[0]);
            return this;
        }

        public object Resolve(Type t, string name)
        {
            object obj2;
            try
            {
                obj2 = ((t.IsClass && !t.IsAbstract) || UnityContainerExtensions.IsRegistered(this._container, t, name)) ? this._container.Resolve(t, name, new ResolverOverride[0]) : null;
            }
            catch (ResolutionFailedException exception)
            {
                throw new InvalidOperationException(string.Format("实例化类型失败，类型为:{0},名称为:{1}", t, name), (Exception)exception);
            }
            return obj2;
        }

        public IEnumerable<object> ResolveAll(Type t)
        {
            return this._container.ResolveAll(t, new ResolverOverride[0]);
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
    #endregion
    
    #region NullContainer
    internal class NullContainer : IContainer, IDisposable
    {
        public IContainer CreateChildContainer()
        {
            return new NullContainer();
        }

        public void Dispose()
        {
        }

        public IContainer RegisterInstance(Type t, string name, object instance)
        {
            return this;
        }

        public IContainer RegisterType(Type from, Type to, string name, LifeTime lifeTime = 0)
        {
            return this;
        }

        public object Resolve(Type t, string name)
        {
            return null;
        }

        public IEnumerable<object> ResolveAll(Type t)
        {
            return Enumerable.Empty<object>();
        }
    }
    #endregion

    #region ContainerSingleton
    public static class ContainerSingleton
    {
        private static IContainer _container = NullContainer;
        private static readonly IContainer NullContainer = new NullContainer();

        public static void SetContainer(IContainer container)
        {
            Guard.ThrowIfArgumentIsNull(container, "container");
            _container = container;
        }

        public static IContainer Instance
        {
            get
            {
                return _container;
            }
        }
    }
    #endregion

    #region ContainerExtensions
    public static class ContainerExtensions
    {
        private static void AutoRegister(IContainer container, Type type)
        {
            Type baseInterface = GetBaseInterface(type);
            if (baseInterface != null)
            {
                //ILog log = LogBuilder.Create("Cicada.DI");
                object[] customAttributes = type.GetCustomAttributes(typeof(ComponentAttribute), false);
                if (customAttributes.Length == 0)
                {
                    container.RegisterType(baseInterface, type, LifeTime.PerResolve);
                    object[] args = new object[] { baseInterface, type.FullName };
                    //log.Trace("添加类型注册  {0} <- {1}", args);
                }
                else
                {
                    ComponentAttribute attribute = (ComponentAttribute)customAttributes[0];
                    Type from = attribute.From ?? baseInterface;
                    string name = string.IsNullOrEmpty(attribute.Name) ? null : attribute.Name;
                    container.RegisterType(from, type, name, attribute.Lifetime);
                    object[] objArray2 = new object[] { from, type.FullName, name, attribute.Lifetime };
                    //log.Trace("添加类型注册  {0} <- {1} 名称:{2} 生命周期:{3:g} ", objArray2);
                }
            }
        }

        private static Type GetBaseInterface(Type type)
        {
            Type[] interfaces = type.GetInterfaces();
            if (interfaces.Length == 0)
            {
                if (!(type.BaseType == typeof(object)))
                {
                    return type.BaseType;
                }
                return null;
            }
            if (interfaces.Length != 1)
            {
                if (type.Name.Equals("Client") && (interfaces.Length == 2))
                {
                    return null;
                }
                if (!(type.BaseType == typeof(object)))
                {
                    return interfaces[interfaces.Length - 1];
                }
            }
            return interfaces[0];
        }

        public static IContainer RegisterFromAssembly(this IContainer container, Assembly assembly)
        {
            foreach (Type type in TypeFinder.GetTypes(assembly))
            {
                AutoRegister(container, type);
            }
            return container;
        }

        public static IContainer RegisterFromProductName(this IContainer container, string productName)
        {
            Guard.ThrowIfArgumentIsNullOrEmpty(productName, "productName");
            foreach (Type type in TypeFinder.GetTypes(productName))
            {
                AutoRegister(container, type);
            }
            return container;
        }

        public static IContainer RegisterInstance<TInterface>(this IContainer container, TInterface instance)
        {
            return container.RegisterInstance(typeof(TInterface), null, instance);
        }

        public static IContainer RegisterInstance<TInterface>(this IContainer container, string name, TInterface instance)
        {
            return container.RegisterInstance(typeof(TInterface), name, instance);
        }

        public static IContainer RegisterInstance(this IContainer container, Type t, object instance)
        {
            return container.RegisterInstance(t, null, instance);
        }

        public static IContainer RegisterType<T>(this IContainer container, LifeTime lifeTime = 0)
        {
            return container.RegisterType(null, typeof(T), null, lifeTime);
        }

        public static IContainer RegisterType<TFrom, TTo>(this IContainer container, LifeTime lifeTime = 0) where TTo : TFrom
        {
            return container.RegisterType(typeof(TFrom), typeof(TTo), null, lifeTime);
        }

        public static IContainer RegisterType<T>(this IContainer container, string name, LifeTime lifeTime = 0)
        {
            return container.RegisterType(null, typeof(T), name, lifeTime);
        }

        public static IContainer RegisterType<TFrom, TTo>(this IContainer container, string name, LifeTime lifeTime = 0) where TTo : TFrom
        {
            return container.RegisterType(typeof(TFrom), typeof(TTo), name, lifeTime);
        }

        public static IContainer RegisterType(this IContainer container, Type t, LifeTime lifeTime = 0)
        {
            return container.RegisterType(null, t, null, lifeTime);
        }

        public static IContainer RegisterType(this IContainer container, Type t, string name, LifeTime lifeTime = 0)
        {
            return container.RegisterType(null, t, name, lifeTime);
        }

        public static IContainer RegisterType(this IContainer container, Type from, Type to, LifeTime lifeTime = 0)
        {
            return container.RegisterType(from, to, null, lifeTime);
        }

        public static T Resolve<T>(this IContainer container)
        {
            return (T)container.Resolve(typeof(T), null);
        }

        public static T Resolve<T>(this IContainer container, string name)
        {
            return (T)container.Resolve(typeof(T), name);
        }

        public static object Resolve(this IContainer container, Type t)
        {
            return container.Resolve(t, null);
        }

        public static IEnumerable<T> ResolveAll<T>(this IContainer container)
        {
            return container.ResolveAll(typeof(T)).Cast<T>();
        }
    }

    #endregion

}
