using Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace Net.Rpc.Thrift.Server
{
    public class ThriftServer : IThriftServer, IDisposable
    {
        private bool _disposed;
        private TServer _server;
        private const int MaxThreadsDefault = 10000;
        private const int ClientTimeoutDefault = 300000;

        public ThriftServer()
        {

        }

        public void Start<T>() where T : class
        {
            Task.Factory.StartNew(() => {                
                var tType = typeof(T);
                if (!tType.IsInterface || tType.ReflectedType == null)
                    throw new InvalidOperationException(string.Format("{0}不是接口", tType.FullName));
                var processorTypeName = tType.ReflectedType.FullName + "+Processor";
                var processorType = tType.Assembly.GetType(processorTypeName, false);
                if (processorType == null)
                {
                    string Info = "没有发现将要向外公开的服务接口,请确保您用的是Thrift生成的服务接口";
                    throw new InvalidOperationException(string.Format(Info, new object[0]));
                }
                TServerSocket socket = new TServerSocket(9527, ClientTimeoutDefault, false);
                TProcessor processor = (TProcessor)ContainerSingleton.Instance.Resolve(processorType);
                _server = new TThreadedServer(processor,socket, 
                    new TTransportFactory(), 
                    new TTransportFactory(), 
                    new TCompactProtocol.Factory(), 
                    new TCompactProtocol.Factory(),
                    MaxThreadsDefault, 
                    new TServer.LogDelegate((str) =>{
                        //写日志。。。。
                    }));
                try
                {
                    _server.Serve();
                }
                finally
                {

                }
            });
        }

        public void Stop()
        {
            if (_server != null)
            {
                _server.Stop();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_server != null)
                    {
                        _server.Stop();
                        _server = null;
                    }
                }
                _disposed = true;
            }
        }
    }
}
