namespace Net.Rpc.Thrift.Server
{
    using Base;
    using Core;
    using Endpoint;

    internal class RegisterType : IRegisterType
    {
        public void RegisterTypes(IContainer container)
        {
            container.RegisterType<IThriftServer, ThriftServer>(LifeTime.PerResolve);
            container.RegisterType<IServerEndPointConfig, ServerEndPointConfig>(LifeTime.Singleton);
        }
    }
}
