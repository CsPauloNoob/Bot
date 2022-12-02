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

        [SlashCommand("meu", "Resgata sua recompensa diária")]

        public async Task Test(InteractionContext ctx)
        {
            if (StartBotServices.Users.Contains(ctx.User.Id.ToString()))
            {
                if (StartBotServices.Diarias.DarCota(ctx.Member.Id).Result)

                    await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                        EmbedMesages.UniqueLineMsg($"Parabés {ctx.User.Mention} você" +
                        $" resgatou seus 300sc diários"));
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource);
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Você já resgatou sua recompensa diária"));
                }
            }
        }

    }
}