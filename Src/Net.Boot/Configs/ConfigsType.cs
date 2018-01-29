using Net.Core;

namespace Net.Boot.Configs
{
    public class ConfigsType : IConfigsType
    {
        public bool Contains(string name)
        {
            return ConfigsData.Instance.Contains(name);
        }

        public string Get(string name)
        {
            return ConfigsData.Instance.Get(name);
        }

        public string[] GetKeys()
        {
            return ConfigsData.Instance.GetKeys();
        }
    }
}
