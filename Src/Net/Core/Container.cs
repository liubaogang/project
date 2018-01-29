using System;
using System.Collections.Generic;
using Unity;
using Unity.Exceptions;
using Unity.Lifetime;
using Net.Base;
using System.Linq;
using System.Reflection;

namespace Net.Core
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

    /************************************************/
    /*                 Container 扩展方法           */
    /************************************************/

    public static class ContainerExtensions
    {
        /// <summary>
        /// 注册对象
        /// </summary>
        /// <param name="container">依赖注入接口</param>
        /// <param name="t">类型</param>
        /// <param name="instance">对象</param>
        /// <returns>依赖注入接口</returns>
        public static IContainer RegisterInstance(this IContainer container, System.Type t, object instance)
        {
            return container.RegisterInstance(t, null, instance);
        }

        /// <summary>
        /// 注册对象
        /// </summary>
        /// <typeparam name="TInterface">类型</typeparam>
        /// <param name="container">依赖注入接口</param>
        /// <param name="instance">对象</param>
        /// <returns>依赖注入接口</returns>
        public static IContainer RegisterInstance<TInterface>(this IContainer container, TInterface instance)
        {
            return container.RegisterInstance(typeof(TInterface), null, instance);
        }

        /// <summary>
        /// 注册对象
        /// </summary>
        /// <typeparam name="TInterface">类型</typeparam>
        /// <param name="container">依赖注入接口</param>
        /// <param name="name">名称</param>
        /// <param name="instance">对象</param>
        /// <returns>依赖注入接口</returns>
        public static IContainer RegisterInstance<TInterface>(this IContainer container, string name, TInterface instance)
        {
            return container.RegisterInstance(typeof(TInterface), name, instance);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="container">依赖注入接口</param>
        /// <param name="t">类型</param>
        /// <param name="lifeTime">生命周期</param>
        /// <returns>依赖注入接口</returns>
        public static IContainer RegisterType(this IContainer container, System.Type t, LifeTime lifeTime = LifeTime.PerResolve)
        {
            return container.RegisterType(null, t, null, lifeTime);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="container">依赖注入接口</param>
        /// <param name="t">类型</param>
        /// <param name="name">名称</param>
        /// <param name="lifeTime">生命周期</param>
        /// <returns>依赖注入接口</returns>
        public static IContainer RegisterType(this IContainer container, System.Type t, string name, LifeTime lifeTime = LifeTime.PerResolve)
        {
            return container.RegisterType(null, t, name, lifeTime);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="container">依赖注入接口</param>
        /// <param name="lifeTime">生命周期</param>
        /// <returns>依赖注入接口</returns>
        public static IContainer RegisterType<T>(this IContainer container, LifeTime lifeTime = LifeTime.PerResolve)
        {
            return container.RegisterType(null, typeof(T), null, lifeTime);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="container">依赖注入接口</param>
        /// <param name="name">名称</param>
        /// <param name="lifeTime">生命周期</param>
        /// <returns>依赖注入接口</returns>
        public static IContainer RegisterType<T>(this IContainer container, string name, LifeTime lifeTime = LifeTime.PerResolve)
        {
            return container.RegisterType(null, typeof(T), name, lifeTime);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="container">依赖注入接口</param>
        /// <param name="from">父类型</param>
        /// <param name="to">类型</param>
        /// <param name="lifeTime">生命周期</param>
        /// <returns>依赖注入接口</returns>
        public static IContainer RegisterType(this IContainer container, Type from, System.Type to, LifeTime lifeTime = LifeTime.PerResolve)
        {
            return container.RegisterType(from, to, null, lifeTime);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TFrom">父类型</typeparam>
        /// <typeparam name="TTo">类型</typeparam>
        /// <param name="container">依赖注入接口</param>
        /// <param name="lifeTime">生命周期</param>
        /// <returns>依赖注入接口</returns>
        public static IContainer RegisterType<TFrom, TTo>(this IContainer container, LifeTime lifeTime = LifeTime.PerResolve) where TTo : TFrom
        {
            return container.RegisterType(typeof(TFrom), typeof(TTo), null, lifeTime);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TFrom">父类型</typeparam>
        /// <typeparam name="TTo">类型</typeparam>
        /// <param name="container">依赖注入接口</param>
        /// <param name="name">名称</param>
        /// <param name="lifeTime">生命周期</param>
        /// <returns>依赖注入接口</returns>
        public static IContainer RegisterType<TFrom, TTo>(this IContainer container, string name, LifeTime lifeTime = LifeTime.PerResolve) where TTo : TFrom
        {
            return container.RegisterType(typeof(TFrom), typeof(TTo), name, lifeTime);
        }

        /// <summary>
        /// 解析成对象
        /// </summary>
        /// <param name="container">依赖注入接口</param>
        /// <param name="t">类型</param>
        /// <returns>对象</returns>
        public static object Resolve(this IContainer container, System.Type t)
        {
            return container.Resolve(t, null);
        }

        /// <summary>
        /// 解析成对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="container">依赖注入接口</param>
        /// <returns>对象</returns>
        public static T Resolve<T>(this IContainer container)
        {
            return (T)container.Resolve(typeof(T), null);
        }

        /// <summary>
        /// 解析成对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="container">依赖注入接口</param>
        /// <param name="name">名称</param>
        /// <returns>对象</returns>
        public static T Resolve<T>(this IContainer container, string name)
        {
            return (T)container.Resolve(typeof(T), name);
        }


        /// <summary>
        /// 解析所有对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="container">依赖注入接口</param>
        /// <returns>对象列表</returns>
        public static IEnumerable<T> ResolveAll<T>(this IContainer container)
        {
            return container.ResolveAll(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// 自动装配指定程序集中定义的类型
        /// </summary>
        /// <param name="container">依赖注入接口</param>
        /// <param name="assembly"></param>
        /// <returns>依赖注入接口</returns>
        public static IContainer RegisterFromAssembly(this IContainer container, Assembly assembly)
        {
            //foreach (var type in TypeFinder.GetTypes(assembly))
            //{
            //    AutoRegister(container, type);
            //}

            return container;
        }


        /// <summary>
        /// 自动装配指定产品名称的程序集中的类型
        /// </summary>
        /// <param name="container">依赖注入接口</param>
        /// <param name="productName">产品名称</param>
        public static IContainer RegisterFromProductName(this IContainer container, string productName)
        {
            //Guard.ThrowIfArgumentIsNullOrEmpty(productName, "productName");

            //foreach (var type in TypeFinder.GetTypes(productName))
            //{
            //    AutoRegister(container, type);
            //}

            return container;
        }

        private static void AutoRegister(IContainer container, Type type)
        {
            //var baseInterface = GetBaseInterface(type);

            //if (baseInterface == null) return;

            //var log = LogBuilder.Create("Cicada.DI");
            //var diAttributes = type.GetCustomAttributes(typeof(ComponentAttribute), false);

            //if (diAttributes.Length == 0)
            //{
            //    container.RegisterType(baseInterface, type);
            //    log.Trace("添加类型注册  {0} <- {1}", baseInterface, type.FullName);
            //}
            //else
            //{
            //    var diAttribute = (ComponentAttribute)diAttributes[0];
            //    var from = diAttribute.From ?? baseInterface;
            //    var name = string.IsNullOrEmpty(diAttribute.Name) ? null : diAttribute.Name;

            //    container.RegisterType(from, type, name, diAttribute.Lifetime);
            //    log.Trace("添加类型注册  {0} <- {1} 名称:{2} 生命周期:{3:g} ", from, type.FullName, name, diAttribute.Lifetime);
            //}
        }
        private static Type GetBaseInterface(Type type)
        {
            var interfaces = type.GetInterfaces();

            if (interfaces.Length == 0)
                return type.BaseType == typeof(Object) ? null : type.BaseType;
            if (interfaces.Length == 1) return interfaces[0];
            return type.BaseType == typeof(Object) ? interfaces[0] : interfaces[interfaces.Length - 1];
        }
    }

}
