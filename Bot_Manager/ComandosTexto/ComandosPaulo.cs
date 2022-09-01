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


        [Command("nitem")]

        async Task NovoItemLoja(CommandContext ctx, string nome_Item, string item, int valorScash, int valorJcash)
        {
            if (ctx.Member.Id == 751499220149731411)
            {
                if(StartBotServices.ItensDAL.NovoItem(item).GetAwaiter().GetResult())
                {
                    StartBotServices.Loja.valorItemG[0] = valorJcash;
                    StartBotServices.Loja.valorItemG[1] = valorScash;

                    if (StartBotServices.Itens_Loja.AdcionaritemV(nome_Item, item).GetAwaiter().GetResult())
                        await ctx.RespondAsync("Inserido com sucesso meu craidor :thumbsup:");
                }

            }
        }



        [Command("Stt")]

        async Task AtualizarStatus(CommandContext ctx, string texto)
        {
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