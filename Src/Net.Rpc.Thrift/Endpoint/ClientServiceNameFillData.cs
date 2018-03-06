using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Endpoint
{
    internal class ClientServiceNameFillData : IClientConfigurationFillData
    {
        public bool Fill(string configName, string key, string data, ClientEndpointInfo info)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                info.ServiceCentreName = data.Trim();
            }
            return true;
        }
    }
}
