using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApiATSA.Controllers
{
    public class FirmaDigitalController : Controller
    {
        // GET: FirmaDigitalController
        public ActionResult Index()
        {
            ViewData["Title"] = "Firmas digitales";
            ViewData["Nav"] = "21";
            ViewData["CabNav"] = "Firmas";
            ViewData["DetailNav"] = "Lista firmas";
            return View();
        }

        //GET: AddFirmaDigital
        public ActionResult AddFirmaDigital()
        {
            TempData["msg"] = null;
            ViewData["Title"] = "Agregar nueva firma";
            ViewData["Nav"] = "21";
            ViewData["CabNav"] = "Firmas";
            ViewData["DetailNav"] = "Nueva firma digital";
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
