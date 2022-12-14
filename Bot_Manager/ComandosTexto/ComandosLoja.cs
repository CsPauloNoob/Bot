using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity.Extensions;
using Bot_Manager.Logs_e_Eventos;
using DSharpPlus;

namespace Bot_Manager.ComandosTexto
{
    internal class ComandosLoja: BaseCommandModule
    {


        
        [Command("loja")]
        async Task VisaoLoja(CommandContext ctx)
        {
            try
            {
                await ctx.RespondAsync(StartBotServices.Loja.View().Result);
            }


            catch (Exception ex)
            {

            }
        }


        [Command("Comprar")]
        async Task Comprar(CommandContext ctx)
        {
            await ctx.RespondAsync("Esse é o comando para comprar alguma coisa da minha loja,\n" +
                "ulitize o comando assim: !jcomprar [id do item a ser comprado]\n" +
                "exemplo: !jcomprar 1");
        }



        [Command("Comprar")]
        async Task Comprar(CommandContext ctx, uint numItem)
        {
            string item = numItem.ToString();

            _ = Task.Run(async () => { 
            try
            {
                if (StartBotServices.Users.Contains(ctx.Member.Id.ToString())) 
                {
                    if (StartBotServices.Itens_Loja.TotalItens(numItem) != 0)
                    {
                        var buttons = StartBotServices.SaveEconomicOP.SaldoSuficiente(ctx.Member.Id, item, true).Result;

                            if (StartBotServices.Itens_Loja.ItemAtivo(numItem.ToString()))
                            {
                                if (buttons.Count > 0)
                                {
                                    await StartBotServices.Resposta_Eventos.AdcionarCompradores
                                        (ctx.Member.Id.ToString(), item);

                                    var message =  ctx.RespondAsync(OpMessages.Menssagen_de_Compra
                                        (ctx.Client, buttons, ctx.User, StartBotServices.Itens_Loja.NomeDe(numItem.ToString())).Result).Result;

                                    await Resposta_Eventos.BtnComprarLoja(message, ctx.User, ctx.Client);
                                }

                                else
                                    await ctx.Client.SendMessageAsync(await ctx.Client.
                                        GetChannelAsync(ctx.Channel.Id),
                                        $"{ctx.Member.Mention} Seu saldo é insuficiente para está compra\n" +
                                        $"Tente um dos meus jogos para tirar uns trocados, digite" +
                                        $" !j ajuda para ver meus jogos disponiveis");
                            }

                            else
                                await ctx.Client.SendMessageAsync(await ctx.Client.
                                       GetChannelAsync(ctx.Channel.Id),
                                       EmbedMesages.UniqueLineMsg($"{ctx.Member.Mention}" +
                                    "Este item não está disponivel na loja"));
                               
                    }
                    else
                        await ctx.RespondAsync("O numero que você digitou não corresponde" +
                            " a nenhum item da loja");
                }

                else
                    await ctx.RespondAsync(EmbedMesages.UniqueLineMsg($"{ctx.Member.Mention}" +
                         " Primeiro use o comando " +
                         "!J ~Registrar~ para poder usar a nossa loja"));
            }

            catch (Exception)
            {
                await ctx.RespondAsync("Algo deu errado, tente novamente");
            }


            });
        }



        [Command("drop")]

        async Task PegarDrop(CommandContext ctx)
        {
            var drop = StartBotServices.Loja.drop_Loja;
            var valor = drop.Valor;
            try
            {
                if (StartBotServices.Users.Contains(ctx.Member.Id.ToString()) && drop.Ativo)
                {

                    if (drop.dropCredito)
                    {
                        await StartBotServices.SaveEconomicOP.AdcionarSaldo(ctx.User.Id, int.Parse(valor), "Scash");

                        await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                            EmbedMesages.UniqueLineMsg($"{ctx.User.Mention} Parabéns você abriu " +
                            $"um drop de {drop.Nome} Scash"));
                    }


                    else
                    {
                        var msg = await ctx.Member.CreateDmChannelAsync();

                        await msg.SendMessageAsync($"Você resgatou um {drop.Nome}, aqui está seu prêmio:\n\n" +
                            drop.Valor);
                    }
                    
                    

                    drop.Clear();
                }

                else if (drop.Ativo)
                    await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                            EmbedMesages.UniqueLineMsg($"{ctx.User.Mention} Você não é um " +
                            $"menbro registrado segundo meus arquivods aqui, dê !j registrar " +
                            $"para usar usar esse comaando"));
                else
                    await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                             EmbedMesages.UniqueLineMsg($"{ctx.User.Mention} Nada a vista!\n\n " +
                             $"Não há drops disponiveis na loja " +
                             $"por enquanto!"));
            }

            catch (UnauthorizedException)
            {
                await ctx.RespondAsync("Não consegui enviar na sua DM seu drop, parece que a sua DM está fechada!");
            }

        }

        [Command("criaranun")]

        public async Task CriarAnuncio(CommandContext ctx)
        {
            if(StartBotServices.Users.Contains(ctx.User.Id.ToString()))
            {
                await ctx.RespondAsync("Função ainda sendo trabalhada melhorada, use este comando da seguinte forma:\n" +
                    "```!jcriaranun [J ou S para a moeda que você quer anunciar] [o número de moedas que você quer" +
                    "anunciar] [o valor cobrado]```\nExemplo: !jcriaranun Jscash 10 4000\n" +
                    "Assim você estará anunciando 10 Jcash por 4000 Scash\n" +
                    " Lembrando que se por exemplo você anunciar 5 Jcash " +
                    "e cobrar 2000, automaticamente os 2000 serão em Scash, ou seja você não pode " +
                    "anunciar uma tipo de moeda e cobrar no mesmo tipo, tenha isso em mente na hora de" +
                    " pensar  no melhor preço para seu anuncio que ficara visivel " +
                    "para todos os servidores em que eu faço parte." +
                    "\nSe este comando não funcionar revise-o e tente novamente!");
            }

            else
                await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result, 
                    EmbedMesages.UniqueLineMsg("Para usar este comando e outros, registresse com " +
                    "**!j Registrar**"));
        }




        [Command("criaranun")]

        public async Task CriarAnuncio(CommandContext ctx, string moeda, ushort qtde, ushort valor_cobrado)
        {

            // moeda: Tipo de moeda para ser anunciada
            // qtde: Quantidade de moeda a ser anunciada, apenas int
            // valor_cobrado: O preço das moedas anunciadas

            try
            {
                bool IsAceptableValue = true;
                bool AuthenticUser = StartBotServices.Users.Contains(ctx.User.Id.ToString());

                if (valor_cobrado > 100 && !moeda.Contains('J', 'j')) //Condição para apenas valores baixos em jcash sejam cobrados
                    IsAceptableValue = false;

                if (AuthenticUser && IsAceptableValue)
                {

                    List<string> m = new List<string> { "Jcash", "J", "jcash", "j", "Scash", "S", "scash", "s" };

                    int[] carteira;
                    var index = "";

                    if (m.IndexOf(moeda) <= 3 && m.IndexOf(moeda) > -1)
                        index = "Jcash";
                    else if (m.IndexOf(moeda) > 3)
                        index = "Scash";



                    var nomeMoeda = index;
                    var moedapag = index.Count() == 5 && index == "Jcash" ? "Scash" : "Jcash";
                    var i = index == "Jcash" ? 0 : 1;

                    if (qtde > 0 && valor_cobrado > 0)
                        if (qtde.GetType() == typeof(ushort) && valor_cobrado.GetType() == typeof(ushort))
                        {
                            if (moeda.Equals(m.Find(x => x == moeda)))
                            {
                                carteira = StartBotServices.UserWalletDAL.FundosCarteira(ctx.User.Id).GetAwaiter().GetResult();

                                if (carteira[i] >= qtde)
                                {
                                    //Terminar anuncios DAL e salvr por aqui
                                    if (StartBotServices.ComercioUsuarios.Anunciar(ctx.User.Id, nomeMoeda, qtde,
                                        valor_cobrado.ToString(), moedapag.ToString()).GetAwaiter().GetResult())
                                    {
                                        await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                                        EmbedMesages.UniqueLineMsg($"{ctx.User.Mention} **Anúncio global criado**" +
                                        "\nAgora todos podem ver seu anúncio"));
                                    }

                                    else
                                        await ctx.RespondAsync("Você já usou seus 2 espaços de anuncios diários, aguarde" +
                                            "a loja resetar para poder anunciar mais.");
                                }

                            }

                            else
                                await ctx.RespondAsync("A moeda declarada deve ser Jcash ou Scash, ou J ou S");
                        }

                        else
                            await ctx.RespondAsync("-Use apenas números\nSem pontos ou vírgulas");
                    else
                        await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                            EmbedMesages.UniqueLineMsg("Valor minímo de 1 (unidade) não alcançado"));
                }

                else if (!AuthenticUser)
                    await ctx.RespondAsync("Para usar este comando e outros, registresse com " +
                    "**!j Registrar**");
                else
                    await ctx.RespondAsync(EmbedMesages.UniqueLineMsg("Valor cobrado em Jcash Alto demais"));
            }

            catch (Exception)
            {
                throw;
            }
        }





        [Command("can")]

        async Task ComprarAnuncio(CommandContext ctx, int id_an)
        {
            if (StartBotServices.Users.Contains(ctx.User.Id.ToString()))
            {
                if (StartBotServices.ComercioUsuarios.AnuncioAtivo(id_an).Result)
                {

                    if (StartBotServices.ComercioUsuarios.Saldo_Sufciente(ctx.User.Id, id_an.ToString()).Result)
                    {

                        if(StartBotServices.ComercioUsuarios.ItemVendido(ctx.User.Id, id_an.ToString()).Result);
                        await ctx.RespondAsync("Item comprado");

                    }

                    else
                    await ctx.RespondAsync("Saldo insuficiente");
                }

                else
                    await ctx.RespondAsync("Esse anuncio não existe, ou não está mais disponível");
            }

            else
                await ctx.RespondAsync("Para usar este comando e outros, registresse com " +
                    "**!j Registrar**");

        }





        [Command("Carteira")]
        
        async Task Carteira(CommandContext ctx)
        {
           
            if(StartBotServices.Users.Contains(ctx.User.Id.ToString()))
            {
                var values = StartBotServices.SaveEconomicOP.RetornaCarteira(ctx.Member.Id).GetAwaiter().GetResult();

                await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                    EmbedMesages.CarteiraView(ctx.User.Username, values));
            }

            else
            {
                await ctx.RespondAsync("Você não é um menbro resgistrado, digite !j Registrar" +
                    "para começar a usar esse comando");
            }
        }





        [Command("anuncios")]

        async Task AnunciosUsuarios(CommandContext ctx)
        {

            if (StartBotServices.Users.Contains(ctx.Member.Id.ToString()))
            {
                if (StartBotServices.ComercioUsuarios.TemAnuncios().GetAwaiter().GetResult())
                    await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                        StartBotServices.ComercioUsuarios.View(ctx.Client).GetAwaiter().GetResult());

                else
                    await ctx.RespondAsync("Não há anúncios ativos no momento");
            }

            else
                await ctx.RespondAsync("Você não é um menbro resgistrado, digite !j Registrar" +
                    "para começar a usar esse comando");
        }

    }
}