using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Interactivity;
using DSharpPlus;
using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace Bot_Manager.ComandosBarra
{
    public class ComandosBarra1:ApplicationCommandModule
    {

        [SlashCommand("AN", "Cria um anuncio global")]

        public async Task NovoAnuncio(InteractionContext ctx,[Option("Quatidade", 
            "quantidade anunciada (min J/S máx: 1/800 J/S: 499/99999)")]
            int valor, [Choice("Jcash", "Jcash")]
            [Choice("Scash", "Scash")] [Option("moeda", "Tipo de moeda a ser vendido")] string moeda)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent("Success!"));

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Thanks for waiting!"));
        }


        [SlashCommand("Test", "Bans a user")]

        public async Task kkkkdsd(InteractionContext ctx, [Option("user", "User to ban")] DiscordUser user,
        [Choice("None", 0)]
        [Choice("1 Day", 1)]
        [Choice("1 Week", 7)]
        [Option("deletedays", "Number of days of message history to delete")] long deleteDays = 0)

        {

        }

        [SlashCommand("Default", "teste de slash command")]

        public async Task Test(InteractionContext ctx)
        {

        }

    }
}