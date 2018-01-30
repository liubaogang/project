namespace Net.Boot.Aspnet.Api
{
    using Net.Core;
    internal interface IWebApiBoot
    {
        void Load(IConfigsType configurationDataRespository);
    }
}
