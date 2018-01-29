using System;
using Net.Core;
using Net.Boot.Configs;
using Net.Base;

namespace Net.Boot
{
    internal class RegisterType : IRegisterType
    {
        public void RegisterTypes(IContainer container)
        {
            container.RegisterType<IConfigsLoad, ConfigsLoad>(LifeTime.Singleton);
        }
    }
}
