using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using webApiATSA.Models;

namespace webApiATSA.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration configuration;
        public AccountController(IConfiguration config)
        {
            this.configuration = config;
        }

        DataB.Login dblayer = new DataB.Login();

        //GET: Index
        public ActionResult Index()
        {
            return View();
        }

        //POST: Index
        [HttpPost]
        public async Task<ActionResult> Index(IFormCollection fc)
        {
            dblayer.config = configuration;
            try { 
                UsuarioModel um = new UsuarioModel();
                um.Usr = fc["usuario"];
                um.Pwd = fc["contrasena"];

                String nombreCompleto = "";
                String id_usuario = "";
                String email = "";
                Boolean b = fc["usuario"].ToString().Contains("@atsaperu.com");

                PersonalModel us = new PersonalModel();

                if (b)//Verificar que esta ingresando con correo azure
                {
                    if (await dblayer.Get(fc["usuario"].ToString(), fc["contrasena"].ToString()))
                    {
                        us.Email = fc["usuario"].ToString();
                        DataSet ds = dblayer.getIdEmail(us);
                        ViewBag.session = ds.Tables[0];

                        foreach (System.Data.DataRow dr in ViewBag.session.Rows)
                        {
                            nombreCompleto = dr["NombresCompletos"].ToString();
                            id_usuario = dr["id_usuario"].ToString();
                            email = dr["Email"].ToString();
                        }
                    }
                    else
                    {
                        throw new Exception("Error en cuenta Office 365");
                    }
                }
                else { 
                    String personal = dblayer.setUsuarioLogin(um);
                    string[] evalError = personal.Split("| ");
                    if (evalError[0] == "Error")
                        throw new Exception(evalError[1]);

                    us.id = Convert.ToInt32(personal);
                    DataSet ds = dblayer.getId(us);
                    ViewBag.session = ds.Tables[0];

                    foreach (System.Data.DataRow dr in ViewBag.session.Rows)
                    {
                        nombreCompleto = dr["apellidos"].ToString() +", "+ dr["nombres"].ToString();
                        id_usuario = dr["id_usuario"].ToString();
                        email = dr["Email"].ToString();
                    }
                }
                
                HttpContext.Session.SetString("nombeCompleto", nombreCompleto);
                HttpContext.Session.SetString("id_usuario", id_usuario);
                HttpContext.Session.SetString("email", email);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                TempData["msg"] = "Error: " + e.Message;
                return View();
            }
        }

        //GET: ResetPassword
        public ActionResult ResetPassword()
        {
            ViewBag.sessionN = HttpContext.Session.GetString("nombeCompleto");
            ViewData["Title"] = "Home";
            ViewData["Nav"] = "00";
            ViewData["CabNav"] = "Home";
            ViewData["DetailNav"] = "Cambiar contraseña";
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(IFormCollection fc)
        {
            ViewBag.sessionN = HttpContext.Session.GetString("nombeCompleto");
            dblayer.config = configuration;
            try { 
                string id_usuario = HttpContext.Session.GetString("id_usuario");
                UsuarioModel um = new UsuarioModel();
                um.id = Convert.ToInt32(id_usuario);
                um.Pwd = fc["ResetPassword"];
                String personal = dblayer.setcontrasenaUPD(um);

                string[] evalError = personal.Split("| ");
                if (evalError[0] == "Error")
                    throw new Exception(evalError[1]);

                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                TempData["msg"] = "Error: " + e.Message;
                ViewData["Title"] = "Home";
                ViewData["Nav"] = "00";
                ViewData["CabNav"] = "Home";
                ViewData["DetailNav"] = "Cambiar contraseña";
                return View();
            }
        }
        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
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

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
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

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
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
