using Microsoft.AspNetCore.Mvc;
using RNC_API.Modelo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RNC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContribuyenteController : ControllerBase
    {
        //private companias_dbContext _context = new companias_dbContext();
        private ContribuyenteModel modeloC = new ContribuyenteModel();

        // GET: api/<ContribuyenteController>
        [HttpGet]
        public ActionResult<IEnumerable<Contribuyente>> Get()
        {
            return Ok("");
        }

        // GET api/<ContribuyenteController>/5
        [HttpGet("rnc/{rnc}")]
        public ActionResult<Contribuyente> Hola(string rnc)
        {
            var contribuyente = modeloC.GetContribuyentePorRnc(rnc);

            /*Contribuyente contribuyente = _context.Contribuyentes
                .Where(s => s.Rnc == rnc).FirstOrDefault<Contribuyente>();
            */
            
            Debug.WriteLine(contribuyente);

            return Ok(contribuyente);
        }

        [HttpGet("razonSocial/{razonSocial}")]
        public ActionResult<Contribuyente> GetRazonSocial(string razonSocial)
        {
            //var contribuyente = _context.Contribuyentes.Where(s => s.RazonSocial.Equals(razonSocial)).FirstOrDefault<Contribuyente>();
            var contribuyente = modeloC.GetContribuyentePorRazonSocial(razonSocial);
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
