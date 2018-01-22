using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Core
{
    internal interface IConfiguration
    {
        bool Contains(string name);

        string Get(string name);

        string[] GetKeys();
    }
}
