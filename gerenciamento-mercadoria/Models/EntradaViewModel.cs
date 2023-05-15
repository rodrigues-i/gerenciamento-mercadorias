using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gerenciamento_mercadoria.Models
{
    public class EntradaViewModel
    {
        public int EntradaId { get; set; }
        public int Quantidade { get; set; }
        public DateTime Data { get; set; }
        public string Local { get; set; }
        public int MercadoriaId { get; set; }

        // Custom attribute
        public string Nome { get; set; }
    }
}