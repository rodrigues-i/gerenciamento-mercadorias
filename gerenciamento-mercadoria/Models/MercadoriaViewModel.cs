using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gerenciamento_mercadoria.Models
{
    public class MercadoriaViewModel
    {
        public int MercadoriaId { get; set; }
        public string Nome { get; set; }
        public string NumeroRegistro { get; set; }
        public string Fabricante { get; set; }
        public int TipoId { get; set; }
        public string Descricao { get; set; }

        // Custom property
        public string NomeTipo { get; set; }
    }
}