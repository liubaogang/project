namespace Net.Boot.Aspnet.Api
{
    using System;
    using Net.Core;
    using System.Collections.Generic;

    internal class ModuleType : IModuleType
    {
        public void Execute(IConfigsType dictData)
        {
            using (IEnumerator<IWebApiBoot> enumerator = ContainerSingleton.Instance.ResolveAll<IWebApiBoot>().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Load(dictData);
                }
            }
        }
    }
}
