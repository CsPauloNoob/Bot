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

        private Dictionary<string, List<int>> ValorItens;



        public async Task<DiscordEmbed> View()
        {
            AtualizarDados().GetAwaiter().GetResult();

            string linhasLoja = "";
            var i = 0;
            var v = 3;
            var c = StartBotServices.ItensValue.ValorItensV;
            int[] vC = StartBotServices.ItensValue.valueClassicNitro;
            int[] vI = StartBotServices.ItensValue.valueInactiveNitro;

            foreach (var x in ItensLoja)
            {

                if (x.Contains("Clássico"))

                    linhasLoja += $"\n> 1. *{x}*:medal:\n" + $"Valor: {vC[0]} Jcash|"
                        + $"{vC[1]} Scash\n";

                else if (x.Contains("novos"))

                    linhasLoja += $"\n> 2. *{x}*:medal:\n" + $"Valor: {vI[0]} Jcash|"
                        + $" {vI[1]} Scash\n";

                else {

                    linhasLoja += $"\n> {v}. *" + x + $"*\nValor: {c[v.ToString()][0]} Jcash|"
                        + $" {c[v.ToString()][1]} Scash\n";
                    if(v <= c.Count+3)
                    v++;
                }


            } //Arrumar os preços, testar vendas de itens v, arrumar essa tela de loja e encontrar novos bugs

            if(drop_Loja)
            return EmbedMesages.StoreView(linhasLoja, drop_Loja);


            return EmbedMesages.StoreView(linhasLoja, false);

        }


        public async Task ResetLoja()
        {
            StartBotServices.Itens_Loja.RestartItens();

            StartBotServices.ComercioUsuarios.ResetaAnuncios();
        }


        private async Task AtualizarDados()
        {
            ValorItens = StartBotServices.ItensValue.TOdosOsValores().GetAwaiter().GetResult();
            ItensLoja = StartBotServices.Itens_Loja.TodosOsItens().GetAwaiter().GetResult();

        }

    }
}