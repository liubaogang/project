namespace Net.Rpc.Thrift.Client
{
    using System;
    using Net.Core;
    using System.Collections.Generic;
    using Castle.DynamicProxy;
    using Endpoint;

    internal class ModuleType : IModuleType
    {
        private readonly IClientEndpointConfig _clientEndpointConfig;
        public ModuleType(IClientEndpointConfig clientEndpointConfig)
        {
            _clientEndpointConfig = clientEndpointConfig;
        }

        public void Execute(IConfigsType dictData)
        {
            ClientEndpointInfo[] infoArray = _clientEndpointConfig.Load(dictData);
            if (infoArray.Length != 0)
            {
                ProxyGenerator generator = new ProxyGenerator();
                foreach (ClientEndpointInfo info in infoArray)
                {
                    IInterceptor interceptor = new Interceptor(info);
                    var ProxyObject = generator.CreateInterfaceProxyWithoutTarget(info.ContractType, interceptor);
                    ContainerSingleton.Instance.RegisterInstance(info.ContractType, ProxyObject);
                }
            }
        }
    }
}
