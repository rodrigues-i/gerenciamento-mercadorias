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
            //MercadoriaViewModel mercadoriaVM = new MercadoriaViewModel();
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

        public ActionResult AtualizarMercadoria(int id)
        {
            gerenciaEntities db = new gerenciaEntities();
            Mercadoria mercadoria = db.Mercadorias.Find(id);
            MercadoriaViewModel mercadoriaVM = new MercadoriaViewModel();

            if (mercadoria != null)
            {

                mercadoriaVM.MercadoriaId = mercadoria.MercadoriaId;
                mercadoriaVM.Nome = mercadoria.Nome;
                mercadoriaVM.NumeroRegistro = mercadoria.NumeroRegistro;
                mercadoriaVM.NomeTipo = mercadoria.Tipos != null ? mercadoria.Tipos.TipoNome : null;
                mercadoriaVM.TipoId = mercadoria.Tipos.TipoId;
                mercadoriaVM.Descricao = mercadoria.Descricao;
                mercadoriaVM.Fabricante = mercadoria.Fabricante;


                ViewBag.Mercadoria = mercadoriaVM;

                List<Tipo> tipos = db.Tipoes.ToList();
                ViewBag.TipoList = new SelectList(tipos, "TipoId", "TipoNome");
            }
            
            return View(mercadoriaVM);
        }

        [HttpPost]
        public ActionResult AtualizarMercadoria(MercadoriaViewModel model, int id)
        {
            try
            {
                gerenciaEntities db = new gerenciaEntities();
                List<Tipo> tipos = db.Tipoes.ToList();
                ViewBag.TipoList = new SelectList(tipos, "TipoId", "TipoNome");
                Mercadoria mercadoria = db.Mercadorias.Find(id);

                mercadoria.Nome = model.Nome;
                mercadoria.NumeroRegistro = model.NumeroRegistro;
                mercadoria.Fabricante = model.Fabricante;
                mercadoria.TipoId = model.TipoId;
                mercadoria.Descricao = model.Descricao;

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

        public ActionResult RemoverMercadoria(int id)
        {

            gerenciaEntities db = new gerenciaEntities();
            Mercadoria mercadoria = db.Mercadorias.Find(id);
            if(mercadoria != null)
            {
                db.Mercadorias.Remove(mercadoria);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult VisualizarEntradaSaida(int id)
        {
            return View();
        }

        public JsonResult ObterEntradasSaidas()
        {
            var data = new
            {
                labels = new[] { "January", "February", "March", "April", "May", "June", "July" },
                input = new[] { 10, 20, 30, 40, 50, 60, 70 },
                output = new[] { 5, 15, 25, 35, 45, 55, 65 }
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}