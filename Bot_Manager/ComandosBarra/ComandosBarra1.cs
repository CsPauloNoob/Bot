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

        [SlashCommand("diaria", "Resgata sua recompensa diária")]

        public async Task Test(InteractionContext ctx)
        {
            if (StartBotServices.Users.Contains(ctx.User.Id.ToString()))
            {
                if (StartBotServices.Diarias.DarCota(ctx.Member.Id).Result)

                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                   .WithContent($"Parabéns ^^ {ctx.User.Mention} você ganhou 300Sc")
                   .WithTitle("Erro")
                   .AsEphemeral(true));
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                   .WithContent("Você já resgatou sua recompensa diária")
                   .WithTitle("Erro")
                   .AsEphemeral(true));
                }
            }
        }

        
        [SlashCommand("Registrar", "Registre-se com esse comando")]
        public async Task Registrar(InteractionContext ctx)
        {
            try
            {
                if (StartBotServices.SaveInfo.RegisterMenber(ctx.Member.Id).GetAwaiter().GetResult())
                    await ctx.CreateResponseAsync(EmbedMesages.UniqueLineMsg("Dados salvos com sucesso!!!" +
                        " Toma aqui 1500Sc para iniciar sua jornada"), true);

                else
                    await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("Não consegui concluir essa tarefa!"));
            }

            catch (Exception ex)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("Algum erro aconteceu aqui do meu lado, tente mais tarde")
                    .WithTitle("Erro")
                    .AsEphemeral(true));
            }
        }

    }
}