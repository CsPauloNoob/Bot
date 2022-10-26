using System;
using System.Collections.Generic;
using System.Text;

namespace Bot_Manager.Quests.Drops
{
    public class Drops
    {
        public string Nome { get; set; }

        public string Valor { get; set; }

        public bool dropCredito { get; set; }

        public bool Ativo { get; set; }

        

        public void Clear()
        {
            this.Nome = "";
            this.Valor = "";
            this.dropCredito = false;
            this.Ativo = false;
        }
    }
}