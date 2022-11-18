using System;
using System.Collections.Generic;
using System.Text;

namespace Bot_Manager.Models
{
    public class Anuncio
    {

        public string Id_Anuncio { get; set; }

        public string Id_Vendedor { get; set; }

        public string Moeda { get; set; }

        public string Quantidade { get; set; }

        public string Valor_Receber { get; set; }

        public string Moeda_pagamento { get; set; }



        public Anuncio()
        {

        }



        public Anuncio(string id_Anuncio, string id_Vendedor, string moeda, string quantidade, string valor_Receber)
        {
            Id_Anuncio = id_Anuncio;
            Id_Vendedor = id_Vendedor;
            Moeda = moeda;
            Quantidade = quantidade;
            Valor_Receber = valor_Receber;

            if (moeda.Contains("Jcash") || moeda.Contains("jcash") || moeda.Contains("j"))
                Moeda_pagamento = "Scash";
            else
                Moeda_pagamento = "Jcash";
        }


        public Anuncio(string id_Anuncio, string id_Vendedor, string moeda, string quantidade, string valor_Receber, string moeda_pag)
        {

            Id_Anuncio = id_Anuncio;
            Id_Vendedor = id_Vendedor;
            Moeda = moeda;
            Quantidade = quantidade;
            Valor_Receber = valor_Receber;
            Moeda_pagamento = moeda_pag;
                
        }
    }
}