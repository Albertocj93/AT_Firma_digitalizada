using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using webApiATSA.Models;

namespace webApiATSA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirmaDigitalApiController : Controller
    {
        private readonly IConfiguration configuration;
        public FirmaDigitalApiController(IConfiguration config)
        {
            this.configuration = config;
        }

        DataB.FirmaDigitalApi dblayer = new DataB.FirmaDigitalApi();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public ActionResult get(String usuario, String contrasena, String nombreDocumento, String sistema, String ip, String localizacion)
        {
            dblayer.config = configuration;
            try
            {
                UsuarioModel um = new UsuarioModel();
                um.Usr = usuario;
                um.Pwd = contrasena;

                String personal = dblayer.setUsuarioLogin(um);
                string[] evalError = personal.Split("| ");
                if (evalError[0] == "Error")
                    throw new Exception(evalError[1]);

                FirmaDigitalHistModel fd = new FirmaDigitalHistModel();
                fd.nombreDocumento = nombreDocumento;
                fd.sistema = sistema;
                fd.ip = ip;
                fd.localizacion = localizacion;
                String idFirma = dblayer.setFirmaDigitalHist(fd, personal);

                string[] evalErrorF = idFirma.Split("| ");
                if (evalErrorF[0] == "Error")
                    throw new Exception("Favor de verificar el envío de todos los datos correctamente o si cuenta con firma digital");

                return Json(new { result = idFirma });
            }
            catch (Exception e)
            {
                return NotFound(new string[] { e.Message });
            }
        }

    }
}
