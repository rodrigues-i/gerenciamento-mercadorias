using gerenciamento_mercadoria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gerenciamento_mercadoria.Controllers
{
    public class SaidaController : Controller
    {
        // GET: Saida
        public ActionResult Index()
        {
            gerenciaEntities db = new gerenciaEntities();
            List<Saida> saidas = db.Saidas.ToList();
            List<SaidaViewModel> saidaViewModelList = saidas.Select(saida => new SaidaViewModel
            {
                SaidaId = saida.SaidaId,
                Nome = saida.Mercadorias.Nome,
                Quantidade = saida.Quantidade,
                Data = saida.Data,
                Local = saida.Local,
                MercadoriaId = saida.MercadoriaId,
            }).ToList();

            ViewBag.Saidas = saidaViewModelList;
            List<Mercadoria> mercadorias = db.Mercadorias.ToList();
            ViewBag.MercadoriaList = new SelectList(mercadorias, "MercadoriaId", "Nome");

            return View();
        }

        [HttpPost]
        public ActionResult Index(SaidaViewModel model)
        {
            gerenciaEntities db = new gerenciaEntities();
            List<Mercadoria> mercadorias = db.Mercadorias.ToList();
            ViewBag.MercadoriaList = new SelectList(mercadorias, "MercadoriaId", "Nome");

            Saida saida = new Saida();
            saida.Quantidade = model.Quantidade;
            saida.Data = model.Data;
            saida.Local = model.Local;
            saida.MercadoriaId = model.MercadoriaId;

            db.Saidas.Add(saida);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}