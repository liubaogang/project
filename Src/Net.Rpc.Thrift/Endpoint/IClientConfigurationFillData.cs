using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Endpoint
{
    internal interface IClientConfigurationFillData
    {
        bool Fill(string configName, string key, string data, ClientEndpointInfo info);
    }
}
