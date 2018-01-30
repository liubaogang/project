namespace Net.Boot.Aspnet.Api
{
    using Base;
    using Core;
    using Docs;
    using Net.Core;

    internal class RegisterType : IRegisterType
    {
        public void RegisterTypes(IContainer container)
        {
            container.RegisterType<IWebApiBoot, WebApiConfiguration>("Configuration", LifeTime.Singleton);
            container.RegisterType<IWebApiBoot, DocumentLoader>("DocumentLoader", LifeTime.Singleton);
        }
    }
}
