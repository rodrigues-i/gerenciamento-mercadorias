using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gerenciamento_mercadoria.Models
{
    public class SaidaViewModel
    {
        public int SaidaId { get; set; }
        [Required(ErrorMessage = "O campo quantidade é obrigatório")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "O campo data é obrigatório")]
        [RegularExpression(@"^(0[1-9]|[1-2][0-9]|3[01])/(0[1-9]|1[0-2])/20[2-9][0-9] ([01][0-9]|2[0-3]):([0-5][0-9])$"
                , ErrorMessage = "Data inválida. Digite uma data no formato dd/MM/yyyy H:mm")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O campo local é obrigatório")]
        public string Local { get; set; }

        [Required(ErrorMessage = "Escolha uma mercadoria")]
        public int MercadoriaId { get; set; }

        // Custom attribute
        public string Nome { get; set; }
    }
}