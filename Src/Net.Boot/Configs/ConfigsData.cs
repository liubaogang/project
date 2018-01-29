using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Boot.Configs
{
    internal class ConfigsData
    {
        public static readonly ConfigsData Instance = new ConfigsData();
        private readonly Dictionary<string, string> _dataDic = new Dictionary<string, string>();
        
        public bool Contains(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(name, "name");

            return _dataDic.ContainsKey(FormatName(name));
        }

        public string Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(name, "name");
            string ret;

            _dataDic.TryGetValue(FormatName(name), out ret);
            return ret;
        }

        public void Add(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(name, "name");

            _dataDic[FormatName(name)] = value;
        }

        public void Remove(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(name, "name");

            _dataDic.Remove(FormatName(name));
        }

        public string[] GetKeys()
        {
            return _dataDic.Keys.ToArray();
        }

        private string FormatName(string name)
        {
            return name.Trim().ToLower();
        }
    }
}
