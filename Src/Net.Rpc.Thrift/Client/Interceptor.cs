using Castle.DynamicProxy;
using Net.Base;
using Net.Core;
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
        private readonly IThriftPools _clientSocket;
        public Interceptor()
        {
            _clientSocket = ContainerSingleton.Instance.Resolve<IThriftPools>();
        }
        public void Intercept(IInvocation invocation)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                stopwatch.Start();
                object[] args = new object[] { invocation };
                new Action<IInvocation>(Execute).TryDo(5, 500, args);
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private void Execute(IInvocation invocation)
        {
            using (ThriftClient client = _clientSocket.GetRpcClient())
            {
                invocation.ReturnValue = invocation.Method.Invoke(client.Client, invocation.Arguments);
            }
        }
    }
}
