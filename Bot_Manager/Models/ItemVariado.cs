using System;
using System.Collections.Generic;
using System.Text;

namespace Bot_Manager.Models
{
    
    public class ItemVariado
    {
        public string Id { get; set; }

        public string Nome { get; set; }

        public string conteudo { get; set; }

        public ItemVariado() { }

        public ItemVariado(string id, string nome, string conteudo)
        {
            Id = id;
            Nome = nome;
            this.conteudo = conteudo;
        }
    }
}
