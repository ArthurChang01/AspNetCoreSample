using AspNetCore.WebAPI.ViewModels.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public Dto Dto = new Dto();

        // GET api/values
        [HttpGet]
        [Authorize]
        public Dto Get()
        {
            Dto.Value = "values";

            return Dto;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Authorize]
        public Dto Get(int id)
        {
            Dto.Value = "values";

            return Dto;
        }

        // POST api/values
        [HttpPost]
        [Authorize]
        public Dto Post([FromBody]string value)
        {
            Dto.Value = value;

            return Dto;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize]
        public Dto Put(int id, [FromBody]string value)
        {
            Dto.Value = string.Format("{0}-{1}", id, value);

            return Dto;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize]
        public Dto Delete(int id)
        {
            Dto.Value = "values";

            return Dto;
        }
    }
}