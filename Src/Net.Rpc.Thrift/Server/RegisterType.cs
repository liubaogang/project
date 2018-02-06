namespace Net.Rpc.Thrift.Server
{
    using Base;
    using Core;
    internal class RegisterType : IRegisterType
    {
        public void RegisterTypes(IContainer container)
        {
            container.RegisterType<IThriftServer, ThriftServer>(LifeTime.PerResolve);
        }
    }
}
