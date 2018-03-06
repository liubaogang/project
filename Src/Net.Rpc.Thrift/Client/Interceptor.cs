using Castle.DynamicProxy;
using Net.Base;
using Net.Core;
using Net.Rpc.Thrift.Endpoint;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Client
{
    internal class Interceptor : IInterceptor
    {
        private readonly IThriftPools _thriftPools;
        private readonly ClientEndpointInfo _clientEndpointInfo;
        public Interceptor(ClientEndpointInfo clientEndpointInfo)
        {
            _clientEndpointInfo = clientEndpointInfo;
            _thriftPools = ContainerSingleton.Instance.Resolve<IThriftPools>();
            _thriftPools.DataInit(clientEndpointInfo);
        }
        public void Intercept(IInvocation invocation)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                stopwatch.Start();
                object[] args = new object[] { invocation };
                new Action<IInvocation>(Execute).TryDo(3, 500, args);
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private void Execute(IInvocation invocation)
        {
            using (ThriftClient client = _thriftPools.GetRpcClient())
            {
                try
                {
                    invocation.ReturnValue = invocation.Method.Invoke(client.Client, invocation.Arguments);
                }
                catch(Exception ex)
                {
                    client.IsAvailable = false;
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
