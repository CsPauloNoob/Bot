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
    class ComandosReg : BaseCommandModule
    {

        /*Classe responsavel por processar os comandos de registro em banco
         * disparados no discord.
         * PS: Limapar op codigo e descentralizatr algumas funções quando a 
         * preguiça permitir*/

        [Command("Registrar")]
        private async Task RegisterUser(CommandContext ctx)
        {
            try
            {
                if (StartBotServices.SaveInfo.RegisterMenber(ctx.Member.Id).GetAwaiter().GetResult())
                    await ctx.RespondAsync(EmbedMesages.UniqueLineMsg("Dados salvos com sucesso!!!" +
                        " Toma aqui 1500Sc para iniciar sua jornada"));

                else
                    await ctx.RespondAsync("Deu ruim! Não consegui concluir corretamente essa tarefa.");
            }

            catch (Exception ex)
            {
                await ctx.RespondAsync("Por algum motivo não consegui concluiu essa tarefa");
            }
        }

        #region Comandos registro do serv

        //Registrar um canal de texto para LOG
        [Command("configlog")]
        private async Task LogChannel(CommandContext ctx, ulong Id)
        {
            try
            {
                if (ctx.Guild.GetChannel(Id) == null)
                    throw new Exception();
                if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
                {
                    await StartBotServices.SaveInfo.RegisterLogChannel(ctx.Guild.Id, Id);
                }

                else
                    await ctx.RespondAsync("Você não tem permissão para usar este comando!");
            }
            catch (NotFoundException)
            {
                await ctx.RespondAsync("O Argumento não é um canal valido para Log");
            }

            catch (NullReferenceException)
            {
                await ctx.RespondAsync("Argumento Inválido");
            }

            catch (Exception)
            {
                await ctx.RespondAsync("> Não estou conseguindo continuar com a sua " +
                    "solicitação!\n> Verifique se o Id digitado está correto");
            }
        }


        [Command("deletelog")]
        async Task DeleteLogChannel(CommandContext ctx)
        {

        }


        [Command("configlog")]
        private async Task LogChannel(CommandContext ctx)
        {
            var message = "Se você quer registrar o esse canal como canal de log, por favor," +
                " forneça-me o ID deste canal, em breve (em uma futura atualização)" +
                " eu mesmo farei isso pra você...";
            await ctx.RespondAsync(EmbedMesages.UniqueLineMsg(message));
        }

        #endregion
    }
}