using Net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Boot.Configs
{
    public class ConfigsLoad : IConfigsLoad
    {
        private readonly Dictionary<string, string> _dataDic = new Dictionary<string, string>();
        //public static readonly ConfigurationDataDictionary Instance = new ConfigurationDataDictionary();
        public bool Contains(string name)
        {
            throw new NotImplementedException();
        }

        public string Get(string name)
        {
            throw new NotImplementedException();
        }

        public string[] GetKeys()
        {
            throw new NotImplementedException();
        }
    }
}
