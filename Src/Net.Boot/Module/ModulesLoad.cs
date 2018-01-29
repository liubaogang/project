using Net.Core;
using System;

namespace Net.Boot.Module
{
    public class ModulesLoad : IModulesLoad
    {
        /// <summary>
        /// TODO:将配置作为参数进行传递
        /// </summary>
        public void Execute()
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
