using gerenciamento_mercadoria.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gerenciamento_mercadoria.Controllers
{
    public class MercadoriaController : Controller
    {
        // GET: Mercadoria
        public ActionResult Index()
        {
            gerenciaEntities db = new gerenciaEntities();
            List<Mercadoria> mercadorias = db.Mercadorias.ToList();
            MercadoriaViewModel mercadoriaVM = new MercadoriaViewModel();
            List<MercadoriaViewModel> mercadoriaVMList = mercadorias.Select(mercadoria => new MercadoriaViewModel
            {
                MercadoriaId = mercadoria.MercadoriaId,
                Nome = mercadoria.Nome,
                NumeroRegistro = mercadoria.NumeroRegistro,
                NomeTipo = mercadoria.Tipos != null ? mercadoria.Tipos.TipoNome : null,
                TipoId = mercadoria.Tipos.TipoId,
                Descricao = mercadoria.Descricao,
                Fabricante = mercadoria.Fabricante
            }).ToList();

            ViewBag.Mercadorias = mercadoriaVMList;

            List<Tipo> tipos = db.Tipoes.ToList();
            ViewBag.TipoList = new SelectList(tipos, "TipoId", "TipoNome");

            return View();
        }

        [HttpPost]
        public ActionResult Index(MercadoriaViewModel model)
        {
            try
            {
                gerenciaEntities db = new gerenciaEntities();
                List<Tipo> tipos = db.Tipoes.ToList();
                ViewBag.TipoList = new SelectList(tipos, "TipoId", "TipoNome");

                Mercadoria mercadoria = new Mercadoria();
                mercadoria.Nome = model.Nome;
                mercadoria.NumeroRegistro = model.NumeroRegistro;
                mercadoria.Fabricante = model.Fabricante;
                mercadoria.TipoId = model.TipoId;
                mercadoria.Descricao = model.Descricao;

                db.Mercadorias.Add(mercadoria);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }

                return RedirectToAction("Index");
            }
        }
    }
}