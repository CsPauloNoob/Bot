using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Bot_Manager.Quests_andGames.Games;
using DSharpPlus;
using Bot_Manager.Logs_e_Eventos;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Bot_Manager.ComandosTexto
{
    internal class ComandosGames : BaseCommandModule
    {


        [Command("Roleta")]

        async Task Roleta(CommandContext ctx)
        {
            await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                EmbedMesages.UniqueLineMsg("**Roleta da grana fácil**  *Beta*\n\n" +
                "aqui você pode ganhar Scash de maneira simples, " +
                "basta ser sortudo\nPara usar esse comando de maneira correta digite !j roleta" +
                " [numero de 1 a 9] e cruse os dedos\nAtenção: \n-Está ação vai custar 100Sc" +
                "\n-O prêmio diario da roleta pode ser alterado, mas o minimo sempre é 300Sc "));
        }

        [Command("Roleta")]

        public async Task Roleta(CommandContext ctx, uint num)
        {
            if(num <= 9)
            if(StartBotServices.Users.Contains(ctx.Member.Id.ToString()))
            {
                if (StartBotServices.SaveEconomicOP.SaldoSuficiente(ctx.Member.Id, 100).Result)
                {
                    if (StartBotServices.SaveEconomicOP.DebitarSaldo(ctx.Member.Id, 100, "Scash").GetAwaiter().GetResult())
                        if(StartBotServices.Roleta.Girar(num).GetAwaiter().GetResult())
                        {
                            await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync
                                (ctx.Channel.Id).Result, EmbedMesages.UniqueLineMsg
                                ($"{ctx.User.Mention} Você ganhou {StartBotServices.Roleta.ValorPremio}Sc"));

                                StartBotServices.SaveEconomicOP.AdcionarSaldo
                                    (ctx.Member.Id, StartBotServices.Roleta.ValorPremio, "Scash").GetAwaiter().GetResult();
                        }
                    else
                            await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync
                               (ctx.Channel.Id).Result, EmbedMesages.UniqueLineMsg
                               ($"{ctx.User.Mention} :slight_frown: " +
                               $"Não foi dessa vez, o seu número não foi o ganhador"));
                }
                else
                    await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync
                               (ctx.Channel.Id).Result, EmbedMesages.UniqueLineMsg
                               ($"{ctx.User.Mention} Você não tem saldo suficiente em Sc para usar a roleta"));

            }
            else
                await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                EmbedMesages.UniqueLineMsg($"{ctx.User.Mention} Você não é registrado aqui nos meus" +
                $" arquivos, digite !j registrar para começar a me usar :eyes:"));

            else
                await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                EmbedMesages.UniqueLineMsg($"{ctx.User.Mention} Os números precisam ser de 1 a 9"));
        }



        [Command("VivoMorto")]
        [Aliases("vm")]

        public async Task VivoMorto(CommandContext ctx, ushort valor)
        {
            bool existgame = Resposta_Eventos.FilaVivoMorto.Contains(ctx.User.Id.ToString());

            if (StartBotServices.Users.Contains(ctx.User.Id.ToString()) && !existgame) {

                var fundos = StartBotServices.UserWalletDAL.FundosCarteira(ctx.User.Id).Result;
                if (fundos[1] >= valor)
                {
                    await StartBotServices.SaveEconomicOP.DebitarSaldo(ctx.User.Id, valor, "Scash");

                    var vm = new VivoMorto(ctx.User.Id.ToString(), valor);

                    await vm.IniciarGame(ctx.RespondAsync(EmbedMesages.EmbedButton("Vivo ou Morto",
                         "O primeiro a atirar ganha!", DiscordColor.IndianRed,
                         vm.EsperarP2())).GetAwaiter().GetResult(), ctx.User);
                }
                else
                    await ctx.RespondAsync("Você o saldo apostado disponível, tente um valor menor");

            }

            else if (existgame)
                await ctx.RespondAsync("Termine a sua partida anterior ou aguarde até que ela se exclua por conta própria");
            else
                await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
            EmbedMesages.UniqueLineMsg($"{ctx.User.Mention} Você não é registrado aqui nos meus" +
            $" arquivos, digite !j registrar para começar a me usar :eyes:"));
        }

    }
}