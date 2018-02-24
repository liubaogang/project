namespace Net.Rpc.Thrift.Client
{
    using System;
    using Net.Core;
    using System.Collections.Generic;
    using Castle.DynamicProxy;

    internal class ModuleType : IModuleType
    {
        public void Execute(IConfigsType dictData)
        {
            if (dictData.Get("Cicada.DI.AutoRegisterByProductName").IndexOf("Net.TestRpc.Client") >= 0)
            {
                var _type = Type.GetType("ThriftCustomerService+Iface,Net.TestRpc.Client", false);
                ProxyGenerator generator = new ProxyGenerator();
                ContainerSingleton.Instance.RegisterInstance(_type, generator.CreateInterfaceProxyWithoutTarget(_type, new Interceptor()));
            }
        }
    }
}
