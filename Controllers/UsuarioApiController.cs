using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using webApiATSA.Models;

namespace webApiATSA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioApiController : Controller
    {
        private readonly IConfiguration configuration;
        public UsuarioApiController(IConfiguration config)
        {
            this.configuration = config;
        }

        DataB.usuario dblayer = new DataB.usuario();

        //GET api/UsuarioApi
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //dblayer.config = configuration;
            //return Ok({ "value1", "value2" });

            return new string[] { "value1", "value2" };
            //if (true)
            //    return BadRequest("Error!");

            //return await dblayer.GetAll();
        }
        //public async Task<List<UsuarioModel>> Get()
        //{
        //    //dblayer.config = configuration;
        //    //return Ok({ "value1", "value2" });

        //    return await( new string[] { "value1", "value2" });
        //    //if (true)
        //    //    return BadRequest("Error!");

        //    //return await dblayer.GetAll();
        //}
    }
}
