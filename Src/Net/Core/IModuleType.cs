namespace Net.Core
{
    /// <summary>
    /// 模块加载
    /// </summary>
    public interface IModuleType
    {
        /// <summary>
        /// 配置模块
        /// </summary>
        void Execute(IConfigsType dictData);
    }
}
