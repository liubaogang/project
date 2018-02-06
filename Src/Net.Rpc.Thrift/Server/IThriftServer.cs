using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Rpc.Thrift.Server
{
    public interface IThriftServer
    {
        void Start<T>() where T :class;
        void Stop();
    }
}
