using Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Endpoint
{
    internal class ServerEndPointConfig: IServerEndPointConfig
    {
        private readonly IConfigsType _configInfo;
        private const string PortConfigName = "Cicada.Rpc.Server.Port";        
        private const string PublishNameConfigName = "Cicada.Rpc.Server.ServiceCentre.Name";
        private const string PublishServerConfigName = "Cicada.Rpc.Server.ServiceCentre.Server";
        private const string PublishRespositoryServerConfigName = "Cicada.Rpc.Server.ServiceCentre.RespositoryServer";
        private const string ConnectionFailProcessModeConfigName = "Cicada.Rpc.Server.ServiceCentre.ConnectionFailProcessMode";

        public int Port { get; private set; }        
        public string PublishName { get; private set; }
        public string PublishServer { get; private set; }
        public string PublishRespositoryServer { get; private set; }
        //public Cicada.Rpc.ServiceCentre.ConnectionFailProcessMode ConnectionFailProcessMode { get; set; }

        public ServerEndPointConfig(IConfigsType configInfo)
        {
            _configInfo = configInfo;
            SetPort();
            SetPublishRespositoryServer();
            if (!string.IsNullOrEmpty(PublishRespositoryServer))
            {
                SetPublishName();
                SetPublishServer();
            }
            SetConnectionFailProcessMode();
        }

        private void SetConnectionFailProcessMode()
        {
            //string str = _configInfo.Get("Cicada.Rpc.Server.ServiceCentre.ConnectionFailProcessMode");
            //Cicada.Rpc.ServiceCentre.ConnectionFailProcessMode retry = Cicada.Rpc.ServiceCentre.ConnectionFailProcessMode.Retry;
            //if (!string.IsNullOrWhiteSpace(str) && !Enum.TryParse<Cicada.Rpc.ServiceCentre.ConnectionFailProcessMode>(str, out retry))
            //{
            //    throw new InvalidOperationException(string.Format("您为连接注册中心出现错误时配置的处理方式无效，必须为Throw或者Retry，请修改配置项{0}", "Cicada.Rpc.Server.ServiceCentre.ConnectionFailProcessMode"));
            //}
            //ConnectionFailProcessMode = retry;
        }

        private void SetPort()
        {
            Port = Convert.ToInt32(_configInfo.Get(PortConfigName));
            if (Port == 0)
            {
                var Info = "请为RPC服务器配置端口，请修改配置项{0}";
                throw new InvalidOperationException(string.Format(Info, PortConfigName));
            }
        }

        private void SetPublishName()
        {
            string str = _configInfo.Get(PublishNameConfigName);
            if (string.IsNullOrWhiteSpace(str))
            {
                var Info = "请为RPC服务器配置发布名称，请修改配置项{0}";
                throw new InvalidOperationException(string.Format(Info, PublishNameConfigName));
            }
           PublishName = str.Trim();
        }

        private void SetPublishRespositoryServer()
        {
            string str = _configInfo.Get(PublishRespositoryServerConfigName);
            PublishRespositoryServer = string.IsNullOrWhiteSpace(str) ? string.Empty : str.Trim();
        }

        private void SetPublishServer()
        {
            string str = _configInfo.Get(PublishServerConfigName);
            if (string.IsNullOrWhiteSpace(str))
            {
                var Info = "请为RPC服务器配置发布服务器地址，请修改配置项{0}";
                throw new InvalidOperationException(string.Format(Info, PublishServerConfigName));
            }
            PublishServer = str.Trim();
        }
    }
}
