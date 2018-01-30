using Net.Core;
using Newtonsoft.Json.Converters;
using System;
using System.Web.Http;

namespace Net.Boot.Aspnet.Api.Core
{
    internal class WebApiConfiguration : IWebApiBoot
    {
        private const string IsActionTrack = "Cicada.Boot.Aspnet.WebApi.ActionTrack";
        public void Load(IConfigsType configurationDataRespository)
        {
            GlobalConfiguration.Configuration.Filters.Add(new WebApiErrorAttribute());
            GlobalConfiguration.Configuration.DependencyResolver = new ApiDependencyResolver();            
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
            new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            });
            if (configurationDataRespository.Get(IsActionTrack) =="true")
                GlobalConfiguration.Configuration.Filters.Add(ContainerSingleton.Instance.Resolve<WebApiTrackAttribute>());
            if (GlobalConfiguration.Configuration.Formatters.XmlFormatter != null)
                GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
        }
    }
}
