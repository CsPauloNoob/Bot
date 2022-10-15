using System;
using System.Collections.Generic;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;

namespace Bot_Manager.Domains
{
    public class SaveEconomicOP
    {
        /* 
         * As casas 0 das variaveis valoritem e valorcarteira são Jcash e
         * as casas 1 são Scash
         */

        public async Task<bool> SaldoSuficiente(ulong UserId, string Item)
        { //validar tipo de moeda consertar, arrumar um jeito de completar a compra de forma coesa
            try
            {
                var valorItem = StartBotServices.ItensValue.ValorDe(Item);

                var valorcarteira = await StartBotServices.UserWalletDAL.FundosCarteira(UserId);

                if (valorcarteira[0] >= valorItem[0] ||
                    valorcarteira[1] >= valorItem[1])
                    return true;
                else
                    return false;
            }

            catch (Exception)
            {
                return false;
            }
        }



        public async Task<bool> SaldoSuficiente(ulong UserId, int valorItem)
        {
            var valor = StartBotServices.UserWalletDAL.FundosCarteira(UserId).GetAwaiter().GetResult();

            if(valor[1] >= valorItem )
                return true;

            return false;
        }


        public async Task<List<DiscordButtonComponent>> SaldoSuficiente(ulong UserId, string Item, bool component)
        {


            List<DiscordButtonComponent> cp = new List<DiscordButtonComponent>();

            try
            {
                var valorItem = StartBotServices.ItensValue.ValorDe(Item);
                var valorcarteira = await StartBotServices.UserWalletDAL.FundosCarteira(UserId);


                if (valorcarteira[0] >= valorItem[0])
                    cp.Add(new DiscordButtonComponent(ButtonStyle.Primary, "Jcash", "Jcash"));

                if (cp.Count != 0)
                {
                    if (valorcarteira[1] >= valorItem[1])
                        cp.Add(new DiscordButtonComponent(ButtonStyle.Primary, "Scash", "Scash"));
                }

                else if (valorcarteira[1] >= valorItem[1])
                    cp.Add(new DiscordButtonComponent(ButtonStyle.Primary, "Scash", "Scash"));



                return await Task.FromResult(cp);

            }

            catch (Exception)
            {
                return await Task.FromResult(cp);
            }
        }



        public async Task<List<DiscordButtonComponent>> SaldoSuficiente(ulong UserId, string Id1, string Id2)
        { //validar tipo de moeda consertar, arrumar um jeito de completar a compra de forma coesa


            List<DiscordButtonComponent> cp = new List<DiscordButtonComponent>();

            try
            {
                var valorcarteira = await StartBotServices.UserWalletDAL.FundosCarteira(UserId);


                if (valorcarteira[0] > 0)
                    cp.Add(new DiscordButtonComponent(ButtonStyle.Primary, Id1, "Jcash"));

                if (cp.Count != 0)
                {
                    if (valorcarteira[1] > 800)
                        cp.Add(new DiscordButtonComponent(ButtonStyle.Primary, Id2, "Scash"));
                }

                else if (valorcarteira[1] >= 800)
                    cp.Add(new DiscordButtonComponent(ButtonStyle.Primary, Id2, "Scash"));



                return await Task.FromResult(cp);

            }

            catch (Exception)
            {
                return await Task.FromResult(cp);
            }
        }


        public async Task<string> DisponibilizarItem(ulong userId, string item, string moneyType)
        {
            int[] saldo = StartBotServices.UserWalletDAL.FundosCarteira(userId).Result;

            int[] valorItem = StartBotServices.ItensValue.ValorDe(item);

            string prize = "";

            int _item = int.Parse(item);


            if (moneyType == "Jcash" && saldo[0] >= valorItem[0])
            {
                if (StartBotServices.Itens_Loja.ItemAtivo(item))
                    prize = StartBotServices.Itens_Loja.darItem(_item);
                else
                    return await Task.FromResult("Item indisponível");

            }

            if (moneyType == "Scash" && saldo[1] >= valorItem[1])
            {
                if (StartBotServices.Itens_Loja.ItemAtivo(item))
                    prize = StartBotServices.Itens_Loja.darItem(_item);
                else
                    return await Task.FromResult("Item indisponível");
            }
            else if(prize == "")
                return await Task.FromResult("saldo insuficiente");

            return prize;
        }


        public async Task<bool> ComitarVendaDb(string itemNum, string link, ulong idComprador, string moeda)
        {
            var result = false;
            string item;
            var valor = StartBotServices.ItensValue.ValorDe(itemNum, moeda);

            item = itemNum == "1" ? "Cnitro" : "Initro";

            if (int.Parse(itemNum) >= 3)
                item = StartBotServices.Itens_Loja.Variados.Find(n => n.Id == itemNum).Nome;

            if(StartBotServices.VendasDAL.ComitarVenda(item, link, valor, idComprador, moeda).GetAwaiter().GetResult())
                result = true;

            return await Task.FromResult(result);

        }

        public async Task<bool> AdcionarSaldo(ulong id, int valor, string moeda)
        {
            var valorAtual = StartBotServices.UserWalletDAL.FundosCarteira(id).GetAwaiter().GetResult();

            valor = moeda == "Scash" ? valorAtual[1]+valor : valorAtual[0]+valor;

            if(StartBotServices.UserWalletDAL.AdcionarSaldo(id, valor.ToString(), moeda)
                .GetAwaiter().GetResult())
                return true;

            return false;
        }

        public async Task<bool> DebitarSaldo(ulong id, int valor, string moeda)
        {
            var valorAtual = StartBotServices.UserWalletDAL.FundosCarteira(id).GetAwaiter().GetResult();

            valor = moeda == "Scash" ? valorAtual[1]-valor : valorAtual[0]-valor;

            if (StartBotServices.UserWalletDAL.DebitarSaldo(id, valor.ToString(), moeda)
                .GetAwaiter().GetResult())
                return true;

            return false;

        }


        public async Task<int[]> RetornaCarteira(ulong id)
        {
            var result = StartBotServices.UserWalletDAL.FundosCarteira(id).GetAwaiter().GetResult();

            return result;
        }


    }
}