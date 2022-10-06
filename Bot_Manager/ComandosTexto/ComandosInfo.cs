using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using System.Reflection;
using Bot_Manager.ultilitarios;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bot_Manager.ComandosTexto
{
    internal class ComandosInfo:BaseCommandModule
    {

        [Command("ajuda")]

        async Task Ajuda(CommandContext ctx)
        {
            await ctx.RespondAsync(Textos.AjudaPadrao());
        }


        [Command("ping")]

        async Task Ping(CommandContext ctx)
        {
            await ctx.RespondAsync(EmbedMesages.UniqueLineMsg("**Ping**\n" +
                $":robot: Bot <===> :satellite: API disocrd: {ctx.Client.Ping.ToString()}ms"));
        }

        [Command("sobre")]

        async Task Sobre(CommandContext ctx)
        {
            var nomebot = ctx.Client.CurrentUser.Username;

            await ctx.RespondAsync(Textos.Sobre(nomebot));

        }

    }
}
