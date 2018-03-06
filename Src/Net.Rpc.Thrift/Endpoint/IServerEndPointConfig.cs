using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Endpoint
{
    internal interface IServerEndPointConfig
    {
        int Port { get; }
        string PublishRespositoryServer { get; }
        string PublishName { get; }
        string PublishServer { get; }
        //Cicada.Rpc.ServiceCentre.ConnectionFailProcessMode ConnectionFailProcessMode { get; set; }
    }
}
