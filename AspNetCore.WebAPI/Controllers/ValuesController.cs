using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebAPI.Controllers
{
    public class dto
    {
        public string Value { get; set; }
    }

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public dto Dto = new dto();

        // GET api/values
        [HttpGet]
        [Authorize]
        public dto Get()
        {
            Dto.Value = "values";

            return Dto;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Authorize]
        public dto Get(int id)
        {
            Dto.Value = "values";

            return Dto;
        }

        // POST api/values
        [HttpPost]
        [Authorize]
        public dto Post([FromBody]string value)
        {
            Dto.Value = value;

            return Dto;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize]
        public dto Put(int id, [FromBody]string value)
        {
            Dto.Value = string.Format("{0}-{1}", id, value);

            return Dto;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize]
        public dto Delete(int id)
        {
            Dto.Value = "values";

            return Dto;
        }
    }
}