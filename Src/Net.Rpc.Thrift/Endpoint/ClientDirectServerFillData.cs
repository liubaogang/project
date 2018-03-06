using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Endpoint
{
    internal class ClientDirectServerFillData : IClientConfigurationFillData
    {
        public bool Fill(string configName, string key, string data, ClientEndpointInfo info)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                int num;
                char[] separator = new char[] { ':' };
                string[] strArray = data.Trim().Split(separator);
                if (strArray.Length != 2)
                {
                    return false;
                }
                info.Server = strArray[0].Trim();
                if (!int.TryParse(strArray[1].Trim(), out num))
                {
                    throw new InvalidOperationException(string.Format("配置的值{0}不是有效的端口号，请检查{1}配置项", data, configName));
                }
                info.Port = num;
            }
            return true;
        }
    }
}
