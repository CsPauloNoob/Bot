using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus;
using DSharpPlus.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Attributes;

namespace Bot_Manager.ComandosTexto
{
    internal class ComandosPunicao : BaseCommandModule
    {

        #region Comandos de Ban

        [Command("ban")]
        async Task UserBan(CommandContext ctx, ulong id, string reason)
        {
            try
            {
                if (ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
                {
                    await ctx.Guild.BanMemberAsync(id, 0, reason);
                    await ctx.Client.SendMessageAsync(ctx.Channel,
                        EmbedMesages.BuildMessageCustom("Usuario Banido!",
                        $"Usuario {id} Não causará mais problemas! \n Motivo: {reason}", DiscordColor.Black));
                }

                else
                    await ctx.Client.SendMessageAsync(
                        ctx.Channel, $":face_with_monocle: {ctx.Member.Mention} Você não tem permissão" +
                        $" para banir usuarios");
            } 

            catch (NotFoundException)
            {
                await ctx.RespondAsync("Usuario não encontrado");
            }

            catch (UnauthorizedException)
            {
                await ctx.RespondAsync("Esse usuario tem o cargo maior que o meu :confused:");
            }
        }

        [Command("ban")]
        async Task UserBan(CommandContext ctx, ulong id)
        {
            try
            {
                if (ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
                {
                    await ctx.Guild.BanMemberAsync(id, 0);
                    await ctx.Client.SendMessageAsync(ctx.Channel,
                       EmbedMesages.BuildMessageCustom("Usuario Banido!",
                       $"Usuario {id} Não causará mais problemas!", DiscordColor.Black));
                }

                else
                    await ctx.Client.SendMessageAsync(
                    ctx.Channel, $":face_with_monocle: {ctx.Member.Mention} Você não tem permissão" +
                    $" para banir usuarios");
            }

            catch (NotFoundException)
            {
                await ctx.RespondAsync("Usuario não encontrado");
            }

            catch (UnauthorizedException)
            {
                await ctx.RespondAsync("Esse usuario tem o cargo maior que o meu :confused:");
            }
        }

        [Command("ban")]
        async Task UserBan(CommandContext ctx, DiscordMember mention)
        {
            try
            {
                if (ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
                {
                    var m = mention;
                    await ctx.Guild.BanMemberAsync(mention, 0);
                    await ctx.Client.SendMessageAsync(ctx.Channel,
                       EmbedMesages.BuildMessageCustom("Usuario Banido!",
                       $"Usuario {m.Nickname} Não causará mais problemas!", DiscordColor.Black));
                }
                else
                    await ctx.Client.SendMessageAsync(
                    ctx.Channel, $":face_with_monocle: {ctx.Member.Mention} Você não tem permissão" +
                    $" para banir usuarios");
            }

            catch (NotFoundException)
            {
                await ctx.RespondAsync("Usuario não encontrado");
            }

            catch (UnauthorizedException)
            {
                await ctx.RespondAsync("Esse usuario tem o cargo maior que o meu :confused:");
            }
        }

        [Command("unban")]
        async Task UserUnban(CommandContext ctx, ulong id)
        {
            try
            {
                if (ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
                {
                    await ctx.Guild.UnbanMemberAsync(id);
                    await ctx.RespondAsync($"Usuário {id} desbanido!");
                }
                else
                    _ = await ctx.Client.SendMessageAsync(
                    ctx.Channel, $":face_with_monocle: {ctx.Member.Mention} Você não tem permissão" +
                    $" para desbanir usuários");
            }

            catch(NotFoundException)
            {
                await ctx.RespondAsync("Este usuario não está banido");
            }

            catch(UnauthorizedException)
            {
                await ctx.RespondAsync("Esse usuario tem o cargo maior que o meu :confused:");
            }
        }


        #endregion

        [Command("kick")]
        async Task UserKick(CommandContext ctx, ulong id)
        {
             try
            {
                await ctx.Guild.BanMemberAsync(id, 0);
                await ctx.Guild.UnbanMemberAsync(id, null);
                await ctx.Client.SendMessageAsync(ctx.Channel,
                   EmbedMesages.BuildMessageCustom("Usuario Expulso",
                   $"Usuario {id} Foi expulso deste servidor!", DiscordColor.White));
            }

            catch (NotFoundException)
            {
                await ctx.RespondAsync("Usuario não encontrado");
            }

            catch (UnauthorizedException)
            {
                await ctx.RespondAsync("Esse usuario tem o cargo maior que o meu :confused:");
            }
        }

        [Command("Kick")]
        async Task UserKick(CommandContext ctx, DiscordMember mention)
        {
            try
            {
                var m = mention;
                await ctx.Guild.BanMemberAsync(mention, 0);
                await ctx.Guild.UnbanMemberAsync(mention, null);
                await ctx.Client.SendMessageAsync(ctx.Channel,
                   EmbedMesages.BuildMessageCustom("Usuario Expulso", 
                   $"Usuario {m.Nickname} Foi expulso deste servidor!", DiscordColor.White));
            }

            catch (NotFoundException)
            {
                await ctx.RespondAsync("Usuario não encontrado");
            }

            catch (UnauthorizedException)
            {
                await ctx.RespondAsync("Esse usuario tem o cargo maior que o meu :confused:");
            }
        }
    }
}