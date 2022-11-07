using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using DSharpPlus;
using Bot_Manager.Quests.Drops;

namespace Bot_Manager
{
    public static class EmbedMesages
    {
        
        
        public static DiscordEmbed StoreView(string linhas, Drops drop)
        {
            var embed = new DiscordEmbedBuilder();
            
            
            embed.AddField($"\t:fleur_de_lis: *Lojinha*\n|\n\n"
            , linhas + "```**Para comprar digite o comando " +
            "!jcomprar ~Número ao lado do item~**\n\n\n```" +
            "||**Um drop acabou de cair na loja, seja" +
            " rápido e digite !drop para resgata-lo**||");
            return embed.Build();
        }



        public static DiscordEmbed StoreView(string linhas)
        {
            var embed = new DiscordEmbedBuilder();
            embed.Color = DiscordColor.Purple;


            embed.AddField($":fleur_de_lis: *Lojinha*\n\n\n"
        ,   linhas + "```\n**Para comprar digite o comando " +
            "!jcomprar ~Número ao lado do item~**```");

            return embed.Build();
        }


        public static DiscordEmbed LojaUsuariosView(string linhasAnuncios)
        {
            var embed = new DiscordEmbedBuilder();

            embed.Color = DiscordColor.Azure;

            embed.AddField("Anúncios", linhasAnuncios);

            return embed.Build();

        }


        public static DiscordEmbed BuildLogMessageDefault(string message, string user)
        {
            var embedMessage = new DiscordEmbedBuilder();
            embedMessage.Color = DiscordColor.Red;
            embedMessage.AddField(user, message, true);
            return embedMessage.Build();
        }

        public static DiscordEmbed BuildMessageCustom(string title, string message, DiscordColor color)
        {
            var embedMessage = new DiscordEmbedBuilder();
            embedMessage.Color = color;
            embedMessage.AddField(title, message, true);
            return embedMessage.Build();
        }

        public static DiscordEmbed UniqueLineMsg(string message)
        {
            var embedMessage = new DiscordEmbedBuilder();
            embedMessage.Color = DiscordColor.Red;
            embedMessage.Description = message;
            return embedMessage.Build();
        }

        public static DiscordEmbed CarteiraView(string nome, int[] carteira)
        {
            var embed = new DiscordEmbedBuilder();
            embed.Color = DiscordColor.Green;

            embed.Title = " :coin: Carteira de " + nome;

            embed.AddField($"Saldo {carteira[0]} JC\n", $"**Saldo {carteira[1]} SC**");

            return embed.Build();
        }


        public static DiscordMessageBuilder EmbedButton(string titulo, string message, DiscordColor color, DiscordButtonComponent button)
        {
            var embed = new DiscordEmbedBuilder()
                .WithColor(color)
                .AddField(titulo, message);

            embed.Build();

            var comp = new DiscordMessageBuilder()
            .AddComponents(button)
            .AddEmbed(embed);

            return comp;
        }

    }
}