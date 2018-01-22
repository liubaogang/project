namespace Net.Core
{
    public interface IRegisterType
    {
        /// <summary>
        /// 注册接口与实现类映射关系
        /// </summary>
        /// <param name="container">依赖注入接口</param>
        void RegisterTypes(IContainer container);
    }
}
