using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIDemo.Controllers
{
    //notice that, this is inherited from ApiController, not Controller, this is for web api, not mvc.
    public class ValuesController : ApiController
    {
        private static List<string> strings = new List<string>{ "value0", "value1", "value2" };
        // GET api/values
        public IEnumerable<string> Get()
        {
            return strings;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return strings[id];
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
            strings.Append(value);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
            strings[id] = value;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            strings.RemoveAt(id);
        }
    }
}
