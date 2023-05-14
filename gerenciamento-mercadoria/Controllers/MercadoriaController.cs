using gerenciamento_mercadoria.Models;
using System;
using System.Collections.Generic;
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
                Tipo = mercadoria.Tipos.TipoId,
                Descricao = mercadoria.Descricao,
                Fabricante = mercadoria.Fabricante
            }).ToList();

            return View(mercadoriaVMList);
        }
    }
}