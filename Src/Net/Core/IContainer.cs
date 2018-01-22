using Net.Base;
using System.Collections.Generic;

namespace Net.Core
{
    public interface IContainer
    {
        /// <summary>
        /// 注册对象
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="name">名称</param>
        /// <param name="instance">对象</param>
        /// <returns>依赖注入接口</returns>
        IContainer RegisterInstance(System.Type t, string name, object instance);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="from">父类型</param>
        /// <param name="to">类型</param>
        /// <param name="name">名称</param>
        /// <param name="lifeTime">生命周期</param>
        /// <returns>依赖注入接口</returns>
        IContainer RegisterType(System.Type from, System.Type to, string name, LifeTime lifeTime = LifeTime.PerResolve);

        /// <summary>
        /// 解析成对象
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="name">名称</param>
        /// <returns>对象</returns>
        object Resolve(System.Type t, string name);


        /// <summary>
        /// 解析所有对象
        /// </summary>
        /// <param name="t">类型</param>
        /// <returns>对象列表</returns>
        IEnumerable<object> ResolveAll(System.Type t);


        /// <summary>
        /// 创建子容器
        /// </summary>
        /// <returns>子容器</returns>
        IContainer CreateChildContainer();
    }
}
