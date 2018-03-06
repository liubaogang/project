using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.TestRpc
{
    class TestServer : ThriftCustomerService.Iface
    {
        public int Add(Customer customer)
        {
            Console.WriteLine("我被调用了！" + DateTime.Now);
            System.Threading.Thread.Sleep(1000);
            return 5;
        }

        public Customer Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetList()
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, Customer> GetMap()
        {
            throw new NotImplementedException();
        }
    }
}
