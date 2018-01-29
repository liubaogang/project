using Net.Attri;
using Net.Base;
using Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Boot.Register
{
    internal class RegisterLoad : IBootStrategy
    {
        private static void AutoRegisterFromProductName()
        {
            string str = ContainerSingleton.Instance.Resolve<IConfigsType>().Get("Cicada.DI.AutoRegisterByProductName");
            if (!string.IsNullOrWhiteSpace(str))
            {
                //ILog log = LogBuilder.Create("Cicada.Boot");
                object[] args = new object[] { str };
                //log.Trace("Cicada按照【产品名称】方式自动注册类型,您配置的产品名称为:{0}", args);
                char[] separator = new char[] { ',' };
                string[] strArray = str.Split(separator);
                for (int i = 0; i < strArray.Length; i++)
                {
                    string productName = strArray[i].Trim();
                    if (productName.Length != 0)
                    {
                        object[] objArray2 = new object[] { productName };
                        //log.Trace("Cicada自动注册类型，产品名称为:{0}", objArray2);
                        ContainerSingleton.Instance.RegisterFromProductName(productName);
                    }
                }
            }
        }

        private static void ExecuteRegisterTypes()
        {
            Type[] typeArray = TypeFinder.GetTypes(typeof(IRegisterType)).ToArray<Type>();
            //ILog log = LogBuilder.Create("Cicada.Boot");
            object[] args = new object[] { typeArray.Length };
            //log.Trace("自动加载类型注册模块，共发现【{0}】个", args);
            foreach (Type type in typeArray)
            {
                object[] objArray2 = new object[] { type.FullName };
                //log.Trace("加载类型注册模块,类型:{0}", objArray2);
                ((IRegisterType)ContainerSingleton.Instance.Resolve(type)).RegisterTypes(ContainerSingleton.Instance);
            }
        }

        public void Run()
        {
            ExecuteRegisterTypes();
            AutoRegisterFromProductName();
        }
    }
}
