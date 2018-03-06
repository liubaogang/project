using Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Endpoint
{
    internal interface IClientEndpointConfig
    {
        ClientEndpointInfo[] Load(IConfigsType dictData);
    }
}
