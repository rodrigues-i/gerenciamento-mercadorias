using gerenciamento_mercadoria.Models;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gerenciamento_mercadoria.Controllers
{
    public class RelatorioController : Controller
    {
        // GET: Relatorio
        public ActionResult Index()
        {
            gerenciaEntities db = new gerenciaEntities();
            var mesesEntrada = db.Entradas
                .Select(entrada => SqlFunctions.DatePart("month", entrada.Data))
                .Distinct()
                .ToList();

            var mesesSaida = db.Saidas
                .Select(saida => SqlFunctions.DatePart("month", saida.Data))
                .Distinct()
                .ToList();

            var textInfo = CultureInfo.CurrentCulture.TextInfo;

            var meses = new List<SelectListItem>();
            foreach (var mes in mesesEntrada.Concat(mesesSaida).Distinct())
            {
                meses.Add(new SelectListItem
                {
                    Text = textInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((int)mes).ToLower()),
                    Value = mes.ToString()
                });
            }

            ViewBag.Meses = meses;

            return View();
        }

        [HttpPost]
        public ActionResult GerarRelatorio(RelatorioViewModel model)
        {
            int selectedMonth = int.Parse(model.Value);

            // perform logic to generate the report based on the selected month

            return View(); // return the view to display the generated report
        }
    }
}