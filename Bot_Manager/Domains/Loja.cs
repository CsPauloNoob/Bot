using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using Bot_Manager.Models;
using System.Linq;

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

            string usado = "";
            string linhasLoja = "";
            var v = 3;
            var variados = StartBotServices.Itens_Loja.Variados;
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
            } //Arrumar os preços, testar vendas de itens v, arrumar essa tela de loja e encontrar novos bugs

            foreach(var x in variados)
            {

                if(!usado.Contains(x.Nome))
                linhasLoja += $"\n> {x.Id}. *" + x.Nome + $"*\nValor: {c[x.Id][0]} Jcash|"
                        + $" {c[x.Id][1]} Scash\n";

                usado += x.Nome;

            }

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