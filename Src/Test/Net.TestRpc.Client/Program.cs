using Net.Boot;
using Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.TestRpc.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            _Application.Run();
            
            var dbiface =ContainerSingleton.Instance.Resolve<ThriftCustomerService.Iface>();

            dbiface.Add(null);

        }
    }
}
