namespace Net.Boot.Aspnet.Api.Docs
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Net.Core;
    using Swashbuckle.Application;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;

    internal class DocumentLoader : IWebApiBoot
    {
        private const string EnableConfigName = "Cicada.Boot.Aspnet.WebApi.Doc.Enabled";
        private const bool EnableDefault = true;
        private const string IsDefaultPageConfigName = "Cicada.Boot.Aspnet.WebApi.Doc.IsDefaultPage";
        private const bool IsDefaultPageDefault = true;
        private const string TitleConfigName = "Cicada.Boot.Aspnet.WebApi.Doc.Title";
        private const string TitleDefault = "WebApi在线接口文档";
        private const string VersionConfigName = "Cicada.Boot.Aspnet.WebApi.Doc.Version";
        private const string VersionDefault = "v1";

        public void Load(IConfigsType configurationDataRespository)
        {
            if (configurationDataRespository.Get(EnableConfigName) == "true")
            {
                Swashbuckle.Application.HttpConfigurationExtensions.EnableSwagger(
                GlobalConfiguration.Configuration, delegate (SwaggerDocsConfig c)
                {
                    SetVersionAndComments(c, (configurationDataRespository.Get(VersionConfigName) ?? VersionDefault)
                                            , (configurationDataRespository.Get(TitleConfigName) ?? TitleDefault));
                    c.IgnoreObsoleteActions();
                    c.IgnoreObsoleteProperties();
                }).EnableSwaggerUi(delegate (SwaggerUiConfig c)
                {
                    Assembly assembly = base.GetType().Assembly;
                    c.InjectJavaScript(assembly, "Cicada.Boot.Aspnet.WebApi.Doc.Content.translator.js");
                    c.InjectJavaScript(assembly, "Cicada.Boot.Aspnet.WebApi.Doc.Content.cicadaAuth.js");
                });
            }
            if (configurationDataRespository.Get(IsDefaultPageConfigName) == "true")
            {
                DynamicModuleUtility.RegisterModule(typeof(DocumnetHome));
            }
        }

        private void SetVersionAndComments(SwaggerDocsConfig config, string version, string title)
        {
            string str = string.Empty;
            try
            {
                config.SingleApiVersion(version, title);
                char[] trimChars = new char[] { '\\' };
                str = AppDomain.CurrentDomain.BaseDirectory.Trim(trimChars);
                string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (directoryName != null)
                {
                    char[] chArray2 = new char[] { '\\' };
                    if (directoryName.Trim(chArray2) != str)
                    {
                        str = Path.Combine(str, "bin");
                    }
                }
            }
            catch { }
            if (!string.IsNullOrEmpty(str))
            {
                foreach (string str3 in Directory.EnumerateFiles(str, "*.XML").ToArray<string>())
                {
                    config.IncludeXmlComments(str3);
                }
            }
        }

    }
}
