using Net.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Endpoint
{
    internal class ClientContractFillData : IClientConfigurationFillData
    {
        public bool Fill(string configName, string key, string data, ClientEndpointInfo info)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return false;
            }
            info.ContractType = Type.GetType(data, false);
            if ((info.ContractType == null) || !info.ContractType.IsInterface)
            {
                return false;
            }
            try
            {
                info.ClientType = TypeFinder.GetTypes(info.ContractType.Assembly, info.ContractType).FirstOrDefault<Type>();
            }
            catch (Exception exception)
            {
                string log = exception.Message;
                // 写日志............
            }
            if (info.ClientType == null)
            {
                throw new InvalidOperationException(string.Format("您配置接口类型{0}不是Thrift生成的接口类型，请修改{1}节点", data, configName));
            }
            return true;
        }
    }
}
