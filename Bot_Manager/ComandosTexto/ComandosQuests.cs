﻿using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus;
using System.Threading.Tasks;

namespace Bot_Manager.ComandosTexto
{
    public class ComandosQuests : BaseCommandModule
    {

        //[Command("Inv")]

        //async Task InviteQuest(CommandContext ctx)
        //{
        //    await ctx.RespondAsync(StartBotServices.Invite.GerarConvite(ctx.Channel).Result);
        //}


        [Command("cota")]

        async Task CotaDiaria(CommandContext ctx)
        {
            if (StartBotServices.Diarias.DarCota(ctx.Member.Id).Result)
                await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                    EmbedMesages.UniqueLineMsg($"Parabés {ctx.User.Mention} você" +
                    $" resgatou seus 300sc diários"));
            else
                await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                    $"{ctx.User.Mention} você já resgatou sua recompensa diaria desse comando");
        }


        async Task Roleta(CommandContext ctx)
        {

        }

    }
}