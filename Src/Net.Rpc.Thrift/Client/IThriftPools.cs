using Net.Rpc.Thrift.Endpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Client
{
    internal interface IThriftPools
    {
        int MaxCount { get; set; }
        int MinCount { get; set; }
        ThriftClient GetRpcClient();
        void DataInit(ClientEndpointInfo endPointInfo);
    }
}
