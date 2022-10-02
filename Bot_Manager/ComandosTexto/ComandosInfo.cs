using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using System.Reflection;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Bot_Manager.ComandosTexto
{
    internal class ComandosInfo:BaseCommandModule
    {

        [Command("ajuda")]

        async Task Ajuda(CommandContext ctx)
        {
            await ctx.RespondAsync(EmbedMesages.UniqueLineMsg(
                "Comando de ajuda " +
                "\n\n**Comandos da loja:**" +
                "\n```loja, comprar, pegar, criaranun, carteira, anuncios```" +
                "\n\n**Comandos de registro:**" +
                "\n```Registrar, Registrarserv, configlog```"+
                "\n\n**Comandos Jogos**"+
                "\n```Roleta, cota```" +
                "\n\nComandos Informação:"+
                "\n```Ping, Sobre```"
                ));
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

            await ctx.RespondAsync(EmbedMesages.BuildMessageCustom(nomebot +
                $" - V. 0.2 (compil. 06/08/22)",
                "\n\n\n > Desenvolvedor: __Paulo meu primeiro bot :)__\n" +
                "> Iniciado em: __Algum momento de dezembro de 2021__\n> Escrito em: __C# v. 8.0__\n" +
                "> Frameworks: __.NET 5.0__ - https://dotnet.microsoft.com/ " +
                "\n> __Dsharp+ 4.0__ - https://github.com/DSharpPlus/DSharpPlus\n" +
                $"> Rodando em: __{Environment.OSVersion}__", DiscordColor.Aquamarine));

        }

    }
}
