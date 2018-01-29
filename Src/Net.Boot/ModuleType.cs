using Net.Core;
using System;

namespace Net.Boot
{
    internal class ModuleType : IModuleType
    {
        public void Execute(IConfigsType dictData)
        {
            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                var exception = (Exception)e.ExceptionObject;
                if (exception.GetBaseException() == null)
                    return;
                //LogBuilder.Create("Cicada.Boot").Error(exp, "AppDomain未处理异常。");
            };
        }
    }
}
