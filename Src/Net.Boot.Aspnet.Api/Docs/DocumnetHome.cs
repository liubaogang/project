namespace Net.Boot.Aspnet.Api.Docs
{
    using System;
    using System.Web;
    internal class DocumnetHome : IHttpModule
    {
        private bool _disposed;
        private bool _initialized;
        private static readonly object InitLock = new object();

        public void Init(HttpApplication app)
        {
            if (!_initialized)
            {
                lock (InitLock)
                {
                    if (!_initialized)
                    {
                        app.BeginRequest += (sender, e) =>
                        {
                            if (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath == "~/")
                                HttpContext.Current.RewritePath("~/swagger");
                        };
                        app.Error += (sender, e) =>
                        {
                            Exception exp = ((HttpApplication)sender).Server.GetLastError();
                            if (exp != null)
                            {
                                //LogBuilder.Create("Cicada.Boot.Aspnet").Error(exp, "WEB未处理异常");
                            }
                        };
                        _initialized = true;
                    }
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                this._disposed = true;
            }
        }

        ~DocumnetHome()
        {
            this.Dispose(false);
        }
    }
}
