using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot_Manager.ultilitarios
{
    public class Textos
    {

        public static DiscordEmbed AjudaPadrao()
        {
            return EmbedMesages.BuildMessageCustom("Comando de ajuda ",
                "\n\n**Comandos da loja:**" +
                "\n```loja, comprar, pegar, criaranun, carteira, anuncios```" +
                "\n\n**Comandos de registro:**" +
                "\n```Registrar, Registrarserv, configlog```" +
                "\n\n**Comandos Jogos**" +
                "\n```Roleta, cota```" +
                "\n\n**Comandos Informação:**" +
                "\n```Ping, Sobre```", DiscordColor.Yellow);
        }

        public static DiscordEmbed Sobre(string nomebot)
        {
            return EmbedMesages.BuildMessageCustom(nomebot + $" - V. 0.2 (compil. 06/08/22)",
             "\n\n\n > Desenvolvedor: __Paulo meu primeiro bot :)__\n" +
             "> Iniciado em: __Algum momento de dezembro de 2021__\n> Escrito em: __C# v. 8.0__\n" +
             "> Frameworks: __.NET 5.0__ - https://dotnet.microsoft.com/ " +
             "\n> __Dsharp+ 4.0__ - https://github.com/DSharpPlus/DSharpPlus\n" +
             $"> Rodando em: __{Environment.OSVersion}__", DiscordColor.Aquamarine);
        }
    }
}