namespace Net.Rpc.Thrift.Client
{
    using Base;
    using Core;
    using Endpoint;

    internal class RegisterType : IRegisterType
    {
        public void RegisterTypes(IContainer container)
        {            
            container.RegisterType<IThriftPools, ThriftPools>(LifeTime.PerResolve);
            container.RegisterType<IClientEndpointConfig, ClientEndpointConfig>(LifeTime.PerResolve);
        }
    }
}
