using gerenciamento_mercadoria.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Objects.SqlClient;
using System.Globalization;
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

        public JsonResult ObterEntradasSaidas(int id)
        {
            var meses = BuscarMeses(id);
            var listaQuantidadeEntradaSaida = ObterQuantidades(id);
            var quantidadeEntradaPorMes = listaQuantidadeEntradaSaida.ElementAt(0);
            var quantidadeSaidaPorMes = listaQuantidadeEntradaSaida.ElementAt(1);

            var data = new
            {
                labels = meses,
                entradas = quantidadeEntradaPorMes,
                saidas = quantidadeSaidaPorMes
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private string[] BuscarMeses(int id)
        {
            gerenciaEntities db = new gerenciaEntities();
            var meses = db.Saidas
                .Where(saida => saida.MercadoriaId == id)
                .Select(saida => SqlFunctions.DatePart("month", saida.Data))
                .Distinct()
                .ToList();

            var nomeMeses = new List<string>();
            foreach (var mes in meses)
            {
                switch (mes)
                {
                    case 1:
                        nomeMeses.Add("Janeiro");
                        break;
                    case 2:
                        nomeMeses.Add("Fevereiro");
                        break;
                    case 3:
                        nomeMeses.Add("Março");
                        break;
                    case 4:
                        nomeMeses.Add("Abril");
                        break;
                    case 5:
                        nomeMeses.Add("Maio");
                        break;
                    case 6:
                        nomeMeses.Add("Junho");
                        break;
                    case 7:
                        nomeMeses.Add("Julho");
                        break;
                    case 8:
                        nomeMeses.Add("Agosto");
                        break;
                    case 9:
                        nomeMeses.Add("Setembro");
                        break;
                    case 10:
                        nomeMeses.Add("Outubro");
                        break;
                    case 11:
                        nomeMeses.Add("Novembro");
                        break;
                    case 12:
                        nomeMeses.Add("Dezembro");
                        break;
                }
            }

            string[] arrayNomeMeses = nomeMeses.ToArray();
            return arrayNomeMeses;
        }

        private List<int[]> ObterQuantidades(int id)
        {
            gerenciaEntities db = new gerenciaEntities();
            // Obter quantidade de saida de mercadoria por mes
            var quantidadesSaidaPorMes = db.Saidas
                .Where(saida => saida.MercadoriaId == id)
                .GroupBy(saida => new { Mes = saida.Data.Month, Ano = saida.Data.Year })
                .Select(g => new { Mes = g.Key.Mes, Ano = g.Key.Ano, QuantidadeTotal = g.Sum(saida => saida.Quantidade) })
                .ToList();

            var arraySaidaQuantidadeTotal = quantidadesSaidaPorMes.Select(q => q.QuantidadeTotal).ToArray();

            // Obter quantidade de entrada de mercadoria por mes
            var quantidadesEntradaPorMes = db.Entradas
                .Where(entrada => entrada.MercadoriaId == id)
                .GroupBy(entrada => new { Mes = entrada.Data.Month, Ano = entrada.Data.Year })
                .Select(g => new { Mes = g.Key.Mes, Ano = g.Key.Ano, QuantidadeTotal = g.Sum(entrada => entrada.Quantidade) })
                .ToList();

            var arrayEntradaQuantidadeTotal = quantidadesEntradaPorMes.Select(q => q.QuantidadeTotal).ToArray();

            // Junta tudo em uma lista
            List<int[]> listaEntradasSaidas = new List<int[]> { arrayEntradaQuantidadeTotal, arraySaidaQuantidadeTotal };

            return listaEntradasSaidas;

        }
    }
}