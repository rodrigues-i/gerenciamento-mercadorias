using gerenciamento_mercadoria.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

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
            int mesSelecionado = int.Parse(model.Value);
            gerenciaEntities db = new gerenciaEntities();
            
            var entradas = db.Entradas
                .Where(entrada => entrada.Data.Month == mesSelecionado)
                .ToList();

            var saidas = db.Saidas
                .Where(saida => saida.Data.Month == mesSelecionado)
                .ToList();

            return GerarPdf(entradas, saidas);
        }

        private ActionResult GerarPdf(List<Entrada> entradas, List<Saida> saidas)
        {

            Document doc = new Document(PageSize.A4);
            doc.SetMargins(40, 40, 40, 80);
            doc.AddCreationDate();
            string caminho = AppDomain.CurrentDomain.BaseDirectory + @"\pdf\" + "relatorio.pdf";

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));

            doc.Open();

            Paragraph titulo = new Paragraph();
            titulo.Font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 35);
            titulo.Alignment = Element.ALIGN_CENTER;
            titulo.Add("Relatório\n\n");
            doc.Add(titulo);

            var table = new PdfPTable(5);
            table.AddCell("Tipo");
            table.AddCell("N° Registro da Mercadoria");
            table.AddCell("Quantidade");
            table.AddCell("Data");
            table.AddCell("Local");

            for (int i = 0; i < entradas.Count || i < saidas.Count; i++)
            {
                if (i < entradas.Count)
                {
                    table.AddCell("Entrada");
                    table.AddCell(entradas[i].Mercadorias.NumeroRegistro);
                    table.AddCell(entradas[i].Quantidade.ToString());
                    table.AddCell(entradas[i].Data.ToString("dd/MM/yyyy"));
                    table.AddCell(entradas[i].Local);
                }
                else
                {
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                }

                if (i < saidas.Count)
                {
                    table.AddCell("Saída");
                    table.AddCell(saidas[i].Mercadorias.NumeroRegistro);
                    table.AddCell(saidas[i].Quantidade.ToString());
                    table.AddCell(saidas[i].Data.ToString("dd/MM/yyyy"));
                    table.AddCell(saidas[i].Local);
                }
                else
                {
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                }
            }

            doc.Add(table);

            doc.Close();
            return Redirect("/pdf/relatorio.pdf");
        }
    }
}