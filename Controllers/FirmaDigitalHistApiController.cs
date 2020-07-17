using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webApiATSA.Models;

namespace webApiATSA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirmaDigitalHistApiController : Controller
    {
        private readonly IConfiguration configuration;
        public FirmaDigitalHistApiController(IConfiguration config)
        {
            this.configuration = config;
        }

        DataB.FirmaDigitalApi dblayer = new DataB.FirmaDigitalApi();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value3", "value4" };
        }

        [HttpPost]
        public ActionResult get(String usuario, String contrasena, String idFirma, String serie)
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
                fd.Id_firma = idFirma;
                fd.codigoSerie = Convert.ToInt32(serie);
                String idFirmaHis = dblayer.setFirmaDigitalHistId(fd);

                string[] evalErrorF = idFirmaHis.Split("| ");
                if (evalErrorF[0] == "Error")
                    throw new Exception("Favor de verificar el envío de todos los datos correctamente o si cuenta con firma digital");

                return Json(new { Fecha = evalErrorF[0], NroDocumento = evalErrorF[1], Sistema = evalErrorF[2], Ip = evalErrorF[3], Localizacion = evalErrorF[4], Nombres = evalErrorF[5],Apellidos = evalErrorF[6], PuestoTrabajo = evalErrorF[7], Gerencia = evalErrorF[8], Dni = evalErrorF[9] });
            }
            catch (Exception e)
            {
                return NotFound(new string[] { e.Message });
            }
        }
    }
}
