using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Endpoint
{
    internal class ClientEndpointInfo
    {
        public ClientEndpointInfo()
        {
            ClientMaxCollections = 100;
        }
        public Type ContractType { get; set; }
        public Type ClientType { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string ServiceFinderType { get; set; }
        public string ServiceRespository { get; set; }
        public string ServiceCentreName { get; set; }
        public int ClientMaxCollections { get; set; }

        //public IServiceFinder ServiceFinder { get; set; }
    }
}
