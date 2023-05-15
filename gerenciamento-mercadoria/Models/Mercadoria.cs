//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace gerenciamento_mercadoria.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mercadoria
    {
        public Mercadoria()
        {
            this.Entradas = new HashSet<Entrada>();
            this.Saidas = new HashSet<Saida>();
        }
    
        public int MercadoriaId { get; set; }
        public string Nome { get; set; }
        public string NumeroRegistro { get; set; }
        public string Fabricante { get; set; }
        public int TipoId { get; set; }
        public string Descricao { get; set; }
    
        public virtual Tipo Tipos { get; set; }
        public virtual ICollection<Entrada> Entradas { get; set; }
        public virtual ICollection<Saida> Saidas { get; set; }
    }
}
