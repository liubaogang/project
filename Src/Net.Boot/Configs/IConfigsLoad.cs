using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Boot.Configs
{
    /// <summary>
    /// 记载配置
    /// </summary>
    public interface IConfigsLoad
    {
        bool Contains(string name);

        string Get(string name);

        string[] GetKeys();
    }
}
