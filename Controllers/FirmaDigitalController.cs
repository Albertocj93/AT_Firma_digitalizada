using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webApiATSA.Models;
using System.Web;
using System.Data;

namespace webApiATSA.Controllers
{
    public class FirmaDigitalController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment hostEnvironment;

        public FirmaDigitalController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            this.configuration = config;
            this.hostEnvironment = hostEnvironment;
        }

        DataB.firmaDigital dblayer = new DataB.firmaDigital();

        // GET: FirmaDigitalController
        public ActionResult Index()
        {
            ViewBag.sessionN = HttpContext.Session.GetString("nombeCompleto");
            dblayer.config = configuration;
            DataSet ds = dblayer.getAll();
            ViewBag.emp = ds.Tables[0];

            ViewData["Title"] = "Firmas digitales";
            ViewData["Nav"] = "21";
            ViewData["CabNav"] = "Firmas";
            ViewData["DetailNav"] = "Lista firmas";
            return View();
        }
        //POST: Index
        [HttpPost]
        public ActionResult Index(IFormCollection fc)
        {
            ViewBag.sessionN = HttpContext.Session.GetString("nombeCompleto");
            dblayer.config = configuration;

            String NombresCompletos = fc["txtNombresApellidos"];
            Int32 id_gerencia = Convert.ToInt32(fc["cboGerencia"]);

            DataSet ds = dblayer.getFirmaDigitalAllFil(NombresCompletos, id_gerencia);
            ViewBag.emp = ds.Tables[0];

            ViewData["Title"] = "Firmas digitales";
            ViewData["Nav"] = "21";
            ViewData["CabNav"] = "Firmas";
            ViewData["DetailNav"] = "Lista firmas";

            return View(ds);
        }

        //GET: AddFirmaDigital
        public ActionResult AddFirmaDigital()
        {
            ViewBag.sessionN = HttpContext.Session.GetString("nombeCompleto");
            TempData["msg"] = null;
            ViewData["Title"] = "Agregar nueva firma";
            ViewData["Nav"] = "21";
            ViewData["CabNav"] = "Firmas";
            ViewData["DetailNav"] = "Nueva firma digital";
            return View();
        }
        //POST: AddFirmaDigital
        [HttpPost]
        public ActionResult AddFirmaDigital(IFormCollection fc, FirmaDigitalModel fm)
        {
            ViewBag.sessionN = HttpContext.Session.GetString("nombeCompleto");            

            dblayer.config = configuration;
            try { 

                fm.id_usuario = Convert.ToInt32(fc["cboUsuario"]);
                String extension = Path.GetExtension(fm.firmaPath.FileName);
                String email = ViewBag.sessionN = HttpContext.Session.GetString("email");

                String dni = dblayer.setFirmaDigitalIns(fm, extension, email);
                string[] evalError = dni.Split("| ");
                if (evalError[0] == "Error")
                    throw new Exception(evalError[1]);

                String uniqueFileName = null;
                String uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Images/Firmas");               
                //uniqueFileName = dni + ".jpeg";
                uniqueFileName = dni + extension;
                String filePath = Path.Combine(uploadsFolder, uniqueFileName);
                FileStream readFS = new FileStream(filePath, FileMode.Create);
                fm.firmaPath.CopyTo(readFS);
                readFS.Close();
                TempData["msg"] = "Insertado";
            }
            catch (Exception e)
            {
                TempData["msg"] = "Error: " + e.Message;
            }
                return RedirectToAction("Index");
        }

        //GET: UpdFirmaDigital
        public ActionResult UpdFirmaDigital(String id)
        {
            ViewBag.sessionN = HttpContext.Session.GetString("nombeCompleto");
            dblayer.config = configuration;
            try
            {
                FirmaDigitalModel fm = new FirmaDigitalModel();
                fm.Id_firma = id;
                DataSet ds = dblayer.getId(fm);
                if (ds == null)
                {
                    String Error = Convert.ToString(ds.Tables[0]);
                    string[] evalError = Error.Split("| ");
                    throw new Exception(evalError[1]);
                }
                else
                    ViewBag.emp = ds.Tables[0];

                ViewData["Title"] = "Editar firma";
                ViewData["Nav"] = "21";
                ViewData["CabNav"] = "Firmas";
                ViewData["DetailNav"] = "Editar firma digital";
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
        public ActionResult UpdFirmaDigital(IFormCollection fc, FirmaDigitalModel fm)
        {
            ViewBag.sessionN = HttpContext.Session.GetString("nombeCompleto");
            dblayer.config = configuration;
            try
            {
                fm.Id_firma = fc["idFirma"];
                String extension = Path.GetExtension(fm.firmaPath.FileName);
                String email = ViewBag.sessionN = HttpContext.Session.GetString("email");
                dblayer.setFirmaDigitalUpd(fm, extension, email);

                String uniqueFileName = null;
                String dni = fc["dni"];
                String uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Images/Firmas");
                //uniqueFileName = dni + ".jpeg";
                uniqueFileName = dni + extension;
                String filePath = Path.Combine(uploadsFolder, uniqueFileName);
                FileStream readFS = new FileStream(filePath, FileMode.Create);
                fm.firmaPath.CopyTo(readFS);
                readFS.Close();
                TempData["msg"] = "Editado";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["msg"] = "Error: " + e.Message;
            }

            return RedirectToAction("Index");
        }
        public ActionResult UpdEstadoFirmaDigital(String id, String estado)
        {
            ViewBag.sessionN = HttpContext.Session.GetString("nombeCompleto");
            dblayer.config = configuration;
            FirmaDigitalModel fm = new FirmaDigitalModel();
            fm.Id_firma = id;
            fm.estado = estado;
            dblayer.setEstadoFirmaDigitalUP(fm);
            TempData["msg"] = "Inactivo";
            return RedirectToAction("Index");
        }

        // GET: Historialfirma
        public ActionResult Historialfirma()
        {
            ViewBag.sessionN = HttpContext.Session.GetString("nombeCompleto");
            dblayer.config = configuration;
            DataSet ds = dblayer.getFirmaDigitalHistAll();
            ViewBag.emp = ds.Tables[0];

            ViewData["Title"] = "Historial de firmas";
            ViewData["Nav"] = "22";
            ViewData["CabNav"] = "Firmas";
            ViewData["DetailNav"] = "Historial firmas";
            return View();
        }
        //POST: Historialfirma
        [HttpPost]
        public ActionResult Historialfirma(IFormCollection fc)
        {
            ViewBag.sessionN = HttpContext.Session.GetString("nombeCompleto");
            dblayer.config = configuration;
            DataSet ds = dblayer.getFirmaDigitalHistAllFil(fc["txtNombresApellidos"], Convert.ToInt32(fc["cboGerencia"]), fc["txtCodigo"], Convert.ToDateTime(fc["txtFechaDesde"]), Convert.ToDateTime(fc["txtFechaHasta"]));
            ViewBag.emp = ds.Tables[0];

            ViewData["Title"] = "Historial de firmas";
            ViewData["Nav"] = "22";
            ViewData["CabNav"] = "Firmas";
            ViewData["DetailNav"] = "Historial firmas";
            return View();
        }
        // GET: FirmaDigitalController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FirmaDigitalController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FirmaDigitalController/Create
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

        // GET: FirmaDigitalController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FirmaDigitalController/Edit/5
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

        // GET: FirmaDigitalController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FirmaDigitalController/Delete/5
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
