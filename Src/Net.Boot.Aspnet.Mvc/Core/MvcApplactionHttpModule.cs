namespace Net.Boot.Aspnet.Mvc.Core
{
    using System;
    using System.Web;

    internal class MvcApplactionHttpModule: IHttpModule
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
                            //日志处理
                        };
                        app.Error += (sender, e) =>
                        {
                            //日志处理
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

        ~MvcApplactionHttpModule()
        {
            this.Dispose(false);
        }
    }
}
