using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Net.TestData.Iserver;

namespace Net.TestApi.Controllers
{
    public class HomeController : ApiController
    {
        private ITestData _test;
        public HomeController(ITestData test)
        {
            _test = test;
        }
        // GET: api/Home
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("v2.0/api/Home/Get/{id?}")]
        // GET: api/Home/5
        public string Get(int id)
        {
            return _test.GetTest1("hello world！");
        }

        [HttpPost]
        [Route("v3.0/api/Home/Get")]        
        public string Get(Dictionary<string,string> dic)
        {
            return _test.GetTest1("3.0=hello world！");
        }

        [HttpPost]
        [Route("v4.0/api/Home/Get")]
        public string Get1(Dictionary<string, string> dic)
        {
            return _test.GetTest1("4.0=hello world！");
        }

        // POST: api/Home
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Home/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Home/5
        public void Delete(int id)
        {
        }
    }
}
