using Net.Attri;
using Net.Base;
using Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Net.Boot.Module
{
    internal class ModulesLoad : IBootStrategy
    {
        private static void ExecuteModules()
        {
            Type[] source = TypeFinder.GetTypes(typeof(IModuleType)).ToArray<Type>();
            //ILog log = LogBuilder.Create("Cicada.Boot");
            object[] args = new object[] { source.Length };
            //log.Trace("自动加载模块单元，共发现【{0}】个", args);
            foreach (IModuleType boot in (from impType in source.Where<Type>(new Func<Type, bool>(ModulesLoad.Where)).OrderBy<Type, int>(new Func<Type, int>(ModulesLoad.GetOrder)) select (IModuleType)ContainerSingleton.Instance.Resolve(impType)).ToArray<IModuleType>())
            {
                object[] objArray2 = new object[] 
                {
                    boot.GetType().FullName
                };
                //log.Trace("加载模块,类型:{0}", objArray2);
                boot.Execute(ContainerSingleton.Instance.Resolve<IConfigsType>());
            }
        }

        private static int GetOrder(Type t)
        {
            ModuleArribute customAttribute = t.GetCustomAttribute<ModuleArribute>();
            if (customAttribute != null)
            {
                return customAttribute.Order;
            }
            return 0;
        }

        public void Run()
        {
            ExecuteModules();
        }

        private static bool Where(Type t)
        {
            ModuleArribute customAttribute = t.GetCustomAttribute<ModuleArribute>();
            if (customAttribute != null)
            {
                return !customAttribute.Disable;
            }
            return true;
        }

    }
}
