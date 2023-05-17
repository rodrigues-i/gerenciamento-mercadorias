using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gerenciamento_mercadoria.Models
{
    public class MercadoriaViewModel
    {
        public int MercadoriaId { get; set; }
        [Required(ErrorMessage = "O campo nome obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo número de registro obrigatório")]
        public string NumeroRegistro { get; set; }
        [Required(ErrorMessage = "O campo fabricante obrigatório")]
        public string Fabricante { get; set; }
        [Required(ErrorMessage = "Escolha o tipo de mercadoria")]
        public int TipoId { get; set; }
        [Required(ErrorMessage = "O campo descrição obrigatório")]
        public string Descricao { get; set; }

        // Custom property
        public string NomeTipo { get; set; }
    }
}