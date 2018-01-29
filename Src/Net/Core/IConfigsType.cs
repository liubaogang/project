namespace Net.Core
{
    public interface IConfigsType
    {
        bool Contains(string name);

        string Get(string name);

        string[] GetKeys();
    }
}
