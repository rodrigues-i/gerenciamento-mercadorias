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
            return View();
        }
    }
}