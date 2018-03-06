using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Endpoint
{
    internal class ClientFinderTypeFillData : IClientConfigurationFillData
    {
        public bool Fill(string configName, string key, string data, ClientEndpointInfo info)
        {
            //string name = string.IsNullOrWhiteSpace(data) ? "direct" : data.Trim();
            //name = name.ToLower();
            //IServiceFinder finder = CicadaFacade.Container.Resolve<IServiceFinder>(name);
            //if (finder == null)
            //{
            //    throw new InvalidOperationException(string.Format("您配置Rpc服务发现类型{0}是无效的，请修改{1}节点", data, configName));
            //}
            //info.ServiceFinderType = name;
            //info.ServiceFinder = finder;
            return true;
        }
    }
}
