using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.TestRpc
{
    class Program
    {
        static void Main(string[] args)
        {
            Net.Boot.Service.ServiceApplication.Run();
            Console.ReadKey();
        }
    }
}
