using Net.Base;
using Net.Boot.Configs;
using Net.Core;

namespace Net.Boot
{
    internal class RegisterType : IRegisterType
    {
        public void RegisterTypes(IContainer container)
        {
            container.RegisterType<IConfigsType, ConfigsType>(LifeTime.Singleton);
        }
    }
}
