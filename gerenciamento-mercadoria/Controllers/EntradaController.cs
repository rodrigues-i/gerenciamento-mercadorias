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

        public ActionResult AtualizarEntrada(int id)
        {
            gerenciaEntities db = new gerenciaEntities();
            Entrada entrada = db.Entradas.Find(id);
            EntradaViewModel entradaVM = new EntradaViewModel();

            if(entrada != null)
            {
                entradaVM.Quantidade = entrada.Quantidade;
                entradaVM.Data = entrada.Data;
                entradaVM.Local = entrada.Local;
                entradaVM.MercadoriaId = entrada.MercadoriaId;
                entradaVM.Nome = entrada.Mercadorias.Nome;
                entradaVM.EntradaId = entrada.EntradaId;

                ViewBag.Entrada = entradaVM;
                List<Mercadoria> mercadorias = db.Mercadorias.ToList();
                ViewBag.MercadoriaList = new SelectList(mercadorias, "MercadoriaId", "Nome");
            }
            

            return View(entradaVM);
        }

        [HttpPost]
        public ActionResult AtualizarEntrada(EntradaViewModel model, int id)
        {
            gerenciaEntities db = new gerenciaEntities();
            List<Mercadoria> mercadorias = db.Mercadorias.ToList();
            ViewBag.MercadoriaList = new SelectList(mercadorias, "MercadoriaId", "Nome");
            Entrada entrada = db.Entradas.Find(id);
            entrada.Quantidade = model.Quantidade;
            entrada.Data = model.Data;
            entrada.Local = model.Local;
            entrada.MercadoriaId = model.MercadoriaId;

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}