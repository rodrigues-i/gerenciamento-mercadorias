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
            //int quantidadeEntradasSaida = entradas.Count + saidas.Count;

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

            Paragraph paragrafo = new Paragraph("", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 12));
            string conteudo = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc ac dui lectus. Cras volutpat feugiat augue, sit amet ultrices quam varius ac. Aenean turpis ligula, lacinia in accumsan eget, tincidunt eget velit. Donec ullamcorper congue tellus a suscipit. Fusce et nibh finibus, molestie dolor et, porta ex. Praesent tellus nunc, posuere molestie ex et, cursus mattis ex. Phasellus sit amet accumsan arcu. Cras rhoncus congue dui sit amet imperdiet. Aenean laoreet ipsum non massa interdum, eu tincidunt ex blandit. Suspendisse potenti. Suspendisse potenti. Sed sit amet justo vel urna accumsan mattis. Donec feugiat nibh non enim laoreet, eget fermentum ex rutrum.\r\n\r\nMaecenas eget aliquet augue. Mauris suscipit mattis facilisis. Nulla tortor erat, pellentesque ut massa quis, tempus gravida libero. Donec semper purus sed velit elementum, auctor pharetra mauris vulputate. Proin commodo velit pellentesque eros bibendum euismod. Duis tristique vel metus vitae faucibus. Sed ornare ut odio a tristique. Integer ac nibh vitae leo dictum commodo. Suspendisse vitae suscipit tellus, eu rutrum dolor. Nunc pulvinar, turpis ut sodales malesuada, leo leo pharetra metus, eget fringilla tellus ex nec nisi. Integer pretium aliquam scelerisque. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur dapibus magna et porttitor volutpat.\n\n";
            paragrafo.Add(conteudo);
            doc.Add(paragrafo);

            //PdfPTable table = new PdfPTable(6);

            doc.Close();
            return Redirect("/pdf/relatorio.pdf");
        }
    }
}