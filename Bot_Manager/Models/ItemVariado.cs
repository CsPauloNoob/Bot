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

        public int Jcash { get; set; }

        public int Scash { get; set; }

        public ItemVariado() { }

        public ItemVariado(string id, string nome, string conteudo, int jcash, int scash)
        {
            Id = id;
            Nome = nome;
            this.conteudo = conteudo;
            Jcash = jcash;
            Scash = scash;
        }
    }
}
