namespace Net.Rpc.Thrift.Client
{
    using Base;
    using Core;
    internal class RegisterType : IRegisterType
    {
        public void RegisterTypes(IContainer container)
        {
            container.RegisterType<IThriftPools, ThriftPools>(LifeTime.PerResolve);
        }
    }
}
