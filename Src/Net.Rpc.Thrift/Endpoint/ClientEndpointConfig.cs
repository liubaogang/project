using Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Endpoint
{
    internal class ClientEndpointConfig : IClientEndpointConfig
    {
        private static readonly Dictionary<string, IClientConfigurationFillData> FillDataStrategies;
        static ClientEndpointConfig()
        {
            FillDataStrategies= new Dictionary<string, IClientConfigurationFillData> {
                {
                    "server",
                    new ClientDirectServerFillData()
                },
                {
                    "contract",
                    new ClientContractFillData()
                },                
                {
                    "servicefindertype",
                    new ClientFinderTypeFillData()
                },
                {
                    "servicecentre.name",
                    new ClientServiceNameFillData()
                },
                {
                    "clientmaxcollections",
                    new ClientMaxPoolFillData()
                },
                {
                    "servicecentre.respositoryserver",
                    new ClientRespositoryFillData()
                }
            };

        }
        public ClientEndpointInfo[] Load(IConfigsType dictData)
        {
            var dictionary = new Dictionary<string, ClientEndpointInfo>();
            foreach (string str in dictData.GetKeys())
            {
                Match match = Regex.Match(str, @"^Cicada.Rpc.Client.Endpoints.(\w+).([\w.]+)$", RegexOptions.IgnoreCase);
                if (match.Success && (match.Groups.Count == 3))
                {
                    ClientEndpointInfo info;
                    if (dictionary.ContainsKey(match.Groups[1].Value))
                    {
                        info = dictionary[match.Groups[1].Value];
                    }
                    else
                    {
                        info = new ClientEndpointInfo();
                        dictionary.Add(match.Groups[1].Value, info);
                    }
                    string key = match.Groups[2].Value.ToLower();
                    if (!FillDataStrategies.ContainsKey(key))
                    {
                        throw new InvalidOperationException(string.Format("未知的配置，请检查节点{0}是否配置正确", str));
                    }
                    if (!FillDataStrategies[key].Fill(str, key, dictData.Get(str), info))
                    {
                        throw new InvalidOperationException(string.Format("错误的配置，请检查节点{0}是否配置正确", str));
                    }
                }
            }
            return dictionary.Values.ToArray<ClientEndpointInfo>();
        }
    }
}
