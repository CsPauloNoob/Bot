using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;

namespace Bot_Manager.ComandosTexto
{
    internal class ComandosPaulo :BaseCommandModule
    {

        [Command("drop")]

        async Task Drop(CommandContext ctx)
        {
            if (ctx.Member.Id == 751499220149731411)
            {
                StartBotServices.Loja.drop_Loja = true;
                await ctx.RespondAsync("Dropado");
            }
        }


        //Setar em tempo de execução o valor dos nitros

        #region Set Valores itens

        [Command("vinitro")]

        async Task VInitro(CommandContext ctx, int Jvalue, int Svalue)
        {

            int[] valor = { Jvalue, Svalue };

            if (ctx.Member.Id == 751499220149731411)
            {
                StartBotServices.ItensValue.valueInactiveNitro = valor;

                await ctx.RespondAsync("Item inserido");
            }
        }


        [Command("vcnitro")]

        async Task VCnitro(CommandContext ctx, int Jvalue, int Svalue)
        {

            int[] valor = { Jvalue, Svalue };

            if (ctx.Member.Id == 751499220149731411)
            {
                StartBotServices.ItensValue.valueClassicNitro = valor;

                await ctx.RespondAsync("Item inserido");
            }

        }


        [Command("vinitro")]

        async Task VInitro(CommandContext ctx)
        {

            if (ctx.Member.Id == 751499220149731411)
            {
                await ctx.RespondAsync("!jinitro valor Jcash, Valor Scash\n\n" +
                    "O mesmo se aplica ao outro comando!");
            }
        }

        //Setar novo item generico na loja

        #endregion


        [Command("initro")]

        async Task NovoInitro(CommandContext ctx, string item)
        {

            if (ctx.Member.Id == 751499220149731411)
            {
                if (StartBotServices.ItensDAL.AddInactiveNitro(item))
                {
                    await ctx.RespondAsync("Inserido com sucesso");

                    await StartBotServices.Itens_Loja.AdcionarInitro(item);
                }
            }

        }

        [Command("cnitro")]

        async Task NovoCnitro(CommandContext ctx, string item)
        {

            if (ctx.Member.Id == 751499220149731411)
            {
                if (StartBotServices.ItensDAL.AddClassicNitro(item))
                {
                    await ctx.RespondAsync("Inserido com sucesso");

                    await StartBotServices.Itens_Loja.AdcionarCnitro(item);
                }
            }

        }


        [Command("nitem")]

        async Task NovoItemLoja(CommandContext ctx, string nome_Item, string item, int valorJcash, int valorScash)
        {
            if (ctx.Member.Id == 751499220149731411)
            {
                if(StartBotServices.ItensDAL.NovoItem(item).GetAwaiter().GetResult())
                {
                    nome_Item = nome_Item.Replace("-", " ");
                    StartBotServices.Loja.valorItemG[0] = valorJcash;
                    StartBotServices.Loja.valorItemG[1] = valorScash;

                    if (StartBotServices.Itens_Loja.Adcionaritem(nome_Item, item).GetAwaiter().GetResult())
                        await ctx.RespondAsync("Inserido com sucesso meu craidor :thumbsup:");
                }

            }
        }

        [Command("nitem")]

        async Task NovoItemLoja(CommandContext ctx)
        {
                if (ctx.Member.Id == 751499220149731411)
                await ctx.RespondAsync("Nome do item, Item, Valor em Jcash, valor em Scash");
        }


        [Command("Stt")]

        async Task AtualizarStatus(CommandContext ctx, string texto)
        {

            texto = texto.Replace("-", " ");

            try
            {
                await ctx.Client.UpdateStatusAsync(new DiscordActivity(texto,
                    ActivityType.Playing), UserStatus.Online, DateTime.Now);
            }

            catch (Exception ex)
            {
                await ctx.RespondAsync($"Ocorreu o seguinte erro:\n ```{ex.Message} ```");
            }
        }

    }
}