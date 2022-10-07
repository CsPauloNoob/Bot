using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                "\n```Registrar, configlog```" +
                "\n\n**Comandos Jogos**" +
                "\n```Roleta, cota```" +
                "\n\n**Comandos Informação:**" +
                "\n```Ping, Sobre```", DiscordColor.Yellow);
        }

        public static DiscordEmbed Sobre(string nomebot)
        {
            return EmbedMesages.BuildMessageCustom(nomebot + $" - v. 1.0.0 (compil. 06/10/22)",
             "\n\n\n > Desenvolvedor: __Paulo meu primeiro bot :)__\n" +
             "> Iniciado em: __Algum momento de dezembro de 2021__\n> Escrito em: __C# v. 8.0__\n" +
             "> Frameworks: __.NET 5.0__ - https://dotnet.microsoft.com/ " +
             "\n> __Dsharp+ 4.0__ - https://github.com/DSharpPlus/DSharpPlus\n" +
             $"> Rodando em: __{Environment.OSVersion}__", DiscordColor.Aquamarine);
        }

        public static DiscordEmbed TextoBoasVindas()
        {
            return EmbedMesages.UniqueLineMsg("Bem vindo! Eu sou um bot de " +
                "recompensas e jogos, onde você pode ganhar nitros e outras " +
                "coisas como jogos e gifts-cards\n\n **Eu tenho minhas moedas " +
                "próprias que se chamam Jcash e Scash, elas tem valores diferentes " +
                "na minha loja, onde você pode resgatar seus prêmios, o Jcash " +
                "é como uma gema rara logo tem mais valor e é mais raro de se ter " +
                "o Scash é como se fossem moedas e a moeda mais usada, mas os dois" +
                "são importantes, por isso guardeos e consiga o máximo que puder**\n\n " +
                "De um ```!j ajuda``` em um canal de texto para ver meus comandos.");
        }


    }
}