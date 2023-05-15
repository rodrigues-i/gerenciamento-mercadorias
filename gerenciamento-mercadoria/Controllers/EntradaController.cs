using gerenciamento_mercadoria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gerenciamento_mercadoria.Controllers
{
    public class EntradaController : Controller
    {
        // GET: Entrada
        public ActionResult Index()
        {
            gerenciaEntities db = new gerenciaEntities();
            List<Entrada> entradas = db.Entradas.ToList();
            EntradaViewModel entradaVM = new EntradaViewModel();
            List<EntradaViewModel> entradaVMList = entradas.Select(entrada => new EntradaViewModel
            {
                EntradaId = entrada.EntradaId,
                Nome = entrada.Mercadorias.Nome,
                Quantidade = entrada.Quantidade,
                Data = entrada.Data,
                Local = entrada.Local,
                MercadoriaId = entrada.MercadoriaId
            }).ToList();

            ViewBag.Entradas = entradaVMList;

            List<Mercadoria> mercadorias = db.Mercadorias.ToList();
            ViewBag.MercadoriaList = new SelectList(mercadorias, "MercadoriaId", "Nome");

            return View();
        }

        [HttpPost]
        public ActionResult Index(EntradaViewModel model)
        {
            gerenciaEntities db = new gerenciaEntities();
            List<Mercadoria> mercadorias = db.Mercadorias.ToList();
            ViewBag.MercadoriaList = new SelectList(mercadorias, "MercadoriaId", "Nome");

            Entrada entrada = new Entrada();
            entrada.Quantidade = model.Quantidade;
            entrada.Data = model.Data;
            entrada.Local = model.Local;
            entrada.MercadoriaId = model.MercadoriaId;
            db.Entradas.Add(entrada);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}