using Microsoft.AspNetCore.Mvc;
using RNC_API.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RNC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContribuyenteController : ControllerBase
    {
        private companias_dbContext _context = new companias_dbContext();
        // GET: api/<ContribuyenteController>
        [HttpGet]
        public ActionResult<IEnumerable<Contribuyente>> Get()
        {
            return Ok("");
        }

        // GET api/<ContribuyenteController>/5
        [HttpGet("{rnc}")]
        public ActionResult<Contribuyente> Get(string rnc)
        {
            var contribuyente = _context.Contribuyentes
                .Where(s => s.Rnc == rnc).FirstOrDefault<Contribuyente>();
            
            return Ok(contribuyente);
        }

        //// POST api/<ContribuyenteController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
            
        //}

        //// PUT api/<ContribuyenteController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
     
        //}

        //// DELETE api/<ContribuyenteController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
