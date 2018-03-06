using Net.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Transport;

namespace Net.Rpc.Thrift.Client
{
    internal class ThriftClient : IDisposable
    {
        public ThriftClient(TSocket tSocket,Type clientType)
        {
            Host = tSocket.Host;
            Port = tSocket.Port;
            IsUse = false;
            IsAvailable = true;
            TCompactProtocol protocol = new TCompactProtocol(tSocket);
            object[] args = new object[] { protocol };
            Client = (IDisposable)Activator.CreateInstance(clientType, args);
        }
        public IDisposable Client { get; set; }
        private bool _isUse { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public bool IsUse
        {
            get
            {
                return _isUse;
            }
            set
            {
                _isUse = value;
                if (_isUse)
                {
                    LastUseTime = DateTime.Now;
                }
            }
        }
        public DateTime LastUseTime { get; set; }

        public void Dispose()
        {
            IsUse = false;
        }
        public bool IsAvailable { get; set; }
    }
}
