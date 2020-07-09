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
using Microsoft.Extensions.DependencyInjection;
using webApiATSA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Transactions;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using webApiATSA.DataB;

namespace webApiATSA.Controllers
{
    
    public class UsuarioController : Controller
    {
        private readonly IConfiguration configuration;

        public UsuarioController(IConfiguration config)
        {
            this.configuration = config;
        }

        DataB.usuario dblayer = new DataB.usuario();

        //GET: Index
        public ActionResult Index()
        {
            dblayer.config = configuration; //Envía paramétro de Iconfiguration
            DataSet ds = dblayer.getAll();
            ViewBag.emp = ds.Tables[0];

            ViewData["Title"] = "Usuarios";
            ViewData["Nav"] = "11";
            ViewData["CabNav"] = "Usuarios";
            ViewData["DetailNav"] = "Lista usuarios";
            //ViewData["Usuarios"] = lista;

            return View(ds);
        }
        //POST: Index
        [HttpPost]
        public ActionResult Index(IFormCollection fc)
        {
            dblayer.config = configuration; //Envía paramétro de Iconfiguration
            PersonalModel us = new PersonalModel();
            us.NombresCompletos = fc["txtNombresApellidos"];
            if(fc["cboGerencia"] == "")
                us.id_gerencia = 0;
            else
                us.id_gerencia = Convert.ToInt32(fc["cboGerencia"]);
            us.activo = Convert.ToByte(fc["cboEstado"]);
            DataSet ds = dblayer.getUsuarioAllFil(us);
            ViewBag.emp = ds.Tables[0];

            ViewData["Title"] = "Usuarios";
            ViewData["Nav"] = "11";
            ViewData["CabNav"] = "Usuarios";
            ViewData["DetailNav"] = "Lista usuarios";

            ModelState.Clear();
            return View(ds);
        }

        //GET: AddUsuario
        public ActionResult AddUsuario()
        {
            TempData["msg"] = null;
            ViewData["Title"] = "Agregar nuevo usuario";
            ViewData["Nav"] = "11";
            ViewData["CabNav"] = "Usuarios";
            ViewData["DetailNav"] = "Nuevo usuario";
            return View();
        }
        //POST: AddUsuario
        [HttpPost]
        public ActionResult AddUsuario(IFormCollection fc, String countRows)
        {
            dblayer.config = configuration; //Envía paramétro de Iconfiguration
            try
            {
                PersonalModel pe = new PersonalModel();
                pe.NombresCompletos = fc["txtNombresApellidos"];
                pe.apellidos = fc["txtApellidos"];
                pe.Email = fc["txtEmail"];
                pe.CodigoSAP = "";
                pe.activo = Convert.ToByte(fc["cboEstado"]);
                pe.MecanicoLinea = 0;
                pe.id_gerencia = Convert.ToInt32(fc["cboGerencia"]);
                pe.dni = fc["txtDni"];
                pe.puestoTrabajo = fc["txtPuestoTrabajo"];
                pe.licencia = fc["txtLicencia"];

                UsuarioModel us = new UsuarioModel();
                us.Usr = fc["txtUsuario"];
                us.Pwd = "";

                int countPerfiles = Convert.ToInt32(countRows);
                List<int> idPerfil = new List<int>();
                for (int i = 1; i <= countPerfiles; i++)
                {
                    idPerfil.Add(Convert.ToInt32(fc["perfil" + i + ""]));
                }

                String personal = dblayer.setPersonalIns(pe, us, idPerfil);
                string[] evalError = personal.Split("| ");
                if (evalError[0] == "Error")
                    throw new Exception(evalError[1]);

                //String idUsuario = dblayer.setUsuarioIns(us);

                TempData["msg"] = "Insertado";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
              TempData["msg"] = "Error: "+ e.Message;
            }
            return RedirectToAction("Index");
        }

        //Methods
        public JsonResult GetGerencia()
        {
            dblayer.config = configuration;
            DataSet ds = dblayer.getGerenciaAll();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new SelectListItem { Text = dr["Descripcion"].ToString(), Value = dr["id"].ToString() });
            }
            //ViewBag.gerencia = ds.Tables[0];
            return Json(list);
        }
        public JsonResult GetPerfil()
        {
            dblayer.config = configuration;
            DataSet ds = dblayer.getPerfilAll();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new SelectListItem { Text = dr["Descripcion"].ToString(), Value = dr["id"].ToString() });
            }

            return Json(list);
        }
        public JsonResult GetUsuarioPerfilIdUsu(int id_usuario)
        {
            dblayer.config = configuration;
            DataSet ds = dblayer.getUsuarioPerfilGetIdUsu(id_usuario);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new SelectListItem { Text = dr["Descripcion"].ToString(), Value = dr["id_perfil"].ToString() });
            }
            //ViewBag.gerencia = ds.Tables[0];
            return Json(list);
        }

        //GET: UpdUsuario
        public ActionResult UpdUsuario(String id)
        {
            dblayer.config = configuration; //Envía paramétro de Iconfiguration
            try
            {
                PersonalModel us = new PersonalModel();
                us.id = Convert.ToInt32(id);
                DataSet ds = dblayer.getId(us);
                if (ds == null)
                {
                    String Error = Convert.ToString(ds.Tables[0]);
                    string[] evalError = Error.Split("| ");
                    throw new Exception(evalError[1]);
                }else
                    ViewBag.emp = ds.Tables[0];

                ViewData["Title"] = "Editar usuario";
                ViewData["Nav"] = "11";
                ViewData["CabNav"] = "Usuarios";
                ViewData["DetailNav"] = "Editar usuario";
                TempData["msg"] = null;
                return View();
            }
            catch (Exception e)
            {
                TempData["msg"] = "Error: " + e.Message;
            }
            return View();
        }
        //POST: UpdUsuario
        [HttpPost]
        public ActionResult UpdUsuario(IFormCollection fc, String countRows)
        {
            dblayer.config = configuration; //Envía paramétro de Iconfiguration
            try
            {
                PersonalModel pe = new PersonalModel();
                pe.id = Convert.ToInt32(fc["id"]);
                pe.NombresCompletos = fc["txtNombresApellidos"];
                pe.apellidos = fc["txtApellidos"];
                pe.Email = fc["txtEmail"];
                pe.CodigoSAP = "";
                pe.activo = Convert.ToByte(fc["cboEstado"]);
                pe.MecanicoLinea = 0;
                pe.id_gerencia = Convert.ToInt32(fc["cboGerencia"]);
                pe.dni = fc["txtDni"];
                pe.puestoTrabajo = fc["txtPuestoTrabajo"];
                pe.licencia = fc["txtLicencia"];

                String chk = fc["chkPss"]; //on or null
                if (chk == "on")
                    chk = "on";
                else
                    chk = "";
                UsuarioModel us = new UsuarioModel();
                us.Usr = fc["txtUsuario"];
                us.Pwd = "";

                int countPerfiles = Convert.ToInt32(countRows);
                List<int> idPerfil = new List<int>();
                for (int i = 1; i <= countPerfiles; i++)
                {
                    idPerfil.Add(Convert.ToInt32(fc["perfil" + i + ""]));
                }

                String personal = dblayer.setPersonalUpd(pe, us, idPerfil, chk);
                string[] evalError = personal.Split("| ");
                if (evalError[0] == "Error")
                    throw new Exception(evalError[1]);

                TempData["msg"] = "Editado";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["msg"] = "Error: " + e.Message;
            }
            return RedirectToAction("Index");
        }
        public ActionResult UpdEstadoPersonal(String id, String estado)
        {
            dblayer.config = configuration;
            PersonalModel pe = new PersonalModel();
            pe.id = Convert.ToInt32(id);
            pe.activo = Convert.ToByte(estado);
            dblayer.setEstadoPersonalUP(pe);
            TempData["msg"] = "Inactivo";
            return RedirectToAction("Index");
        }

        // GET: UsuarioController/Details/5
        public ActionResult All()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }

}
