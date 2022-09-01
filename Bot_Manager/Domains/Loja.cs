using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Bot_Manager.Domains
{
    public class Loja
    {
        public bool drop_Loja;

        private List<string> ItensLoja;

        public int[] valorItemG = new int[2];

        private Dictionary<string, List<int>> ValorItens;



        //Adcionar descrição para novos itens
        private string[] description = {"\n> 1. *Nitro Clássico*:medal:\n",
                "\n> 2. *Nitro para novos assinantes*:medal:\n" };

        public async Task<DiscordEmbed> View()
        {
            AtualizarLoja().GetAwaiter().GetResult();

            string linhasLoja = "";
            var i = 0;
            var v = 0;
            var c = 0;
            int[] vC = StartBotServices.ItensValue.valueClassicNitro;
            int[] vI = StartBotServices.ItensValue.valueInactiveNitro;

            foreach (var x in ItensLoja)
            {
                if (i == 0)
                    linhasLoja += $"\n> 1. *{x}*:medal:\n" + $"Valor: {vC[0]} Jcash|"
                        + $"{vC[1]} Scash\n";


                if(i == 1)
                    linhasLoja += $"\n> 2. *{x}*:medal:\n" + $"Valor: {vI[0]} Jcash|"
                        + $" {vI[1]} Scash\n";

                if(i>=2)
                    linhasLoja += "\n> 3. "+ItensLoja[i] + $"\nValor: {valorItemG[0]} Jcash|"
                        + $" {valorItemG[1]} Scash";

                i++;
                v = i + 1;
                c++;
            }

            if(drop_Loja)
            return EmbedMesages.StoreView(linhasLoja, drop_Loja);


            return EmbedMesages.StoreView(linhasLoja, false);

        }



        public async Task AtualizarLoja()
        {
            ValorItens = StartBotServices.ItensValue.TOdosOsValores().GetAwaiter().GetResult();
            ItensLoja = StartBotServices.Itens_Loja.TodosOsItens().GetAwaiter().GetResult();

        }

    }
}