using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Endpoint
{
    internal class ClientMaxPoolFillData : IClientConfigurationFillData
    {
        public bool Fill(string configName, string key, string data, ClientEndpointInfo info)
        {
            int num;
            if (string.IsNullOrWhiteSpace(data))
            {
                info.ClientMaxCollections = 100;
                return true;
            }
            if (!int.TryParse(data.Trim(), out num))
            {
                throw new InvalidOperationException(string.Format("配置的值{0}不是有效的客户端连接最大连接数，请检查{1}配置项", data, configName));
            }
            info.ClientMaxCollections = num;
            return true;
        }
    }
}
