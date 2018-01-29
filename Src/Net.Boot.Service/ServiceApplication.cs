namespace Net.Boot.Service
{
    using Net.Core;
    using System;
    using Topshelf;
    using Topshelf.HostConfigurators;
    using Topshelf.Runtime;
    using Topshelf.ServiceConfigurators;

    public static class ServiceApplication
    {
        private const string DescriptionConfigName = "Cicada.Boot.Service.Description";
        private const string DisplayNameConfigName = "Cicada.Boot.Service.DisplayName";
        private const string ServiceNameConfigName = "Cicada.Boot.Service.Name";

        public static void Run()
        {
            _Application.Run();
            HostFactory.Run(delegate (HostConfigurator x) {
                x.Service<IService>(delegate (ServiceConfigurator<IService> s) {
                    s.ConstructUsing(delegate (HostSettings name) {
                        IService service = null;
                        try
                        {
                            service = ContainerSingleton.Instance.Resolve<IService>();
                        }
                        catch(Exception ex)
                        {
                            if (service == null)
                            {
                                string msg = "未能找到服务启动类，请确保您创建了IService接口的实现类";
                                throw new InvalidOperationException(msg);
                            }
                            else
                            {
                                throw new Exception(string.Format("服务启动异常：{0}", ex.Message));
                            }
                        }
                        return service;
                    });
                    s.WhenStarted<IService>(tc => tc.Start());
                    s.WhenStopped<IService>(tc => tc.Stop());
                });
                x.UseLinuxIfAvailable();
                IConfigsType local1 = ContainerSingleton.Instance.Resolve<IConfigsType>();
                string str = local1.Get(ServiceNameConfigName);
                if (string.IsNullOrWhiteSpace(str))
                {
                    string Msg = "请通过设置{0}配置项,为服务配置服务名称";
                    throw new InvalidOperationException(string.Format(Msg, ServiceNameConfigName));
                }
                x.SetServiceName(str.Trim());
                string str2 = local1.Get(DisplayNameConfigName);
                if (!string.IsNullOrWhiteSpace(str2))
                {
                    x.SetDisplayName(str2.Trim());
                }
                string str3 = local1.Get(DescriptionConfigName);
                if (!string.IsNullOrWhiteSpace(str3))
                {
                    x.SetDescription(str3);
                }
                x.StartAutomatically();
                x.RunAsLocalSystem();
            });
        }
    }
}
