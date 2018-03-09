using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Net.TestApi.Controllers
{
    public class IndexController : ApiController
    {
        // GET: api/Index
        public async Task<string[]> Get()
        {
            return await  Task.Run(() =>
            {
                Thread.Sleep(1000);
                return new string[] { GetBeginThreadInfo(), GetEndThreadInfo() };
            });
            //return Ok(result);
        }

        // GET: api/Index/5
        public string Get(int id)
        {
            Thread.Sleep(1000);
            return GetBeginThreadInfo()+GetEndThreadInfo();
        }

        // POST: api/Index
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Index/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Index/5
        public void Delete(int id)
        {
        }

        private string GetBeginThreadInfo()
        {
            int t1, t2, t3;
            ThreadPool.GetAvailableThreads(out t1, out t3);
            ThreadPool.GetMaxThreads(out t2, out t3);
            return string.Format("开始:{0:mm:ss,ffff} 线程Id:{1} Web线程数:{2}",
                                    DateTime.Now,
                                    Thread.CurrentThread.ManagedThreadId,
                                    t2 - t1);
        }

        private string GetEndThreadInfo()
        {
            int t1, t2, t3;
            ThreadPool.GetAvailableThreads(out t1, out t3);
            ThreadPool.GetMaxThreads(out t2, out t3);
            return string.Format(" 结束:{0:mm:ss,ffff} 线程Id:{1} Web线程数:{2}",
                                    DateTime.Now,
                                    Thread.CurrentThread.ManagedThreadId,
                                    t2 - t1);
        }
    }
}
