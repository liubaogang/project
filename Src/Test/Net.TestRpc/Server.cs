using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.TestRpc
{
    class Server : Net.Boot.Service.IService
    {
        private Net.Rpc.Thrift.Server.IThriftServer _server;
        public Server(Net.Rpc.Thrift.Server.IThriftServer server)
        {
            _server = server;
        }
        public void Start()
        {
            _server.Start<ThriftCustomerService.Iface>();
        }

        public void Stop()
        {
            _server.Stop();
        }
    }
}
