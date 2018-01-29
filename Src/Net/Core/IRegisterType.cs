namespace Net.Core
{
    /// <summary>
    /// 框架类型注册
    /// </summary>
    public interface IRegisterType
    {
        /// <summary>
        /// 注册接口与实现类映射关系
        /// </summary>
        /// <param name="container">依赖注入接口</param>
        void RegisterTypes(IContainer container);
    }
}
