using Net.TestData.Iserver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.TestData.Server
{
    public class TestData : ITestData
    {
        public string GetTest1(string str)
        {
            return str;
        }
    }
}
