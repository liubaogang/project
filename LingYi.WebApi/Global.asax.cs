using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace LingYi.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var FileDirectory = AppDomain.CurrentDomain.BaseDirectory ;
            if (!Directory.Exists(FileDirectory + "UserData"))
                Directory.CreateDirectory(FileDirectory + "UserData");
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
