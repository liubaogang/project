using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Cache.Redis
{
    public class Class1
    {
        public bool isEnabled { get; set; } = true;
        public int? getemlementCount(List<int> list)
        {
            return list?.Count();
        }
    }
}
