using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus;
using System.Threading.Tasks;
using System.Linq;
using Bot_Manager.ultilitarios;
using DSharpPlus.Interactivity.Extensions;

namespace Bot_Manager.Logs_e_Eventos
{
    public class Resposta_Eventos
    {

        private DiscordClient Client;

        public static List<string> FilaVivoMorto = new List<string>();

        public static Dictionary<string, string> PossivelCompra = new Dictionary<string, string>();

        public Resposta_Eventos(DiscordClient client)
        {
            Client = client;

            Botao_Pressionado().GetAwaiter().GetResult();

            ModalUp().GetAwaiter().GetResult();
            
        }

        public async Task AdcionarCompradores(string id, string item)
        {
            var a = 0;
            if (!PossivelCompra.ContainsKey(id))
                PossivelCompra.Add(id, item);
            else
            {
                PossivelCompra.Remove(id);

                PossivelCompra.Add(id, item);
            }
            if (PossivelCompra.Count > 200)
                PossivelCompra.Clear();
        }


        //Remoção devido a erros no debian

        public async Task Entradas()
        {

            Client.GuildMemberAdded += async (s, e) =>
            {
                var a = await e.Member.CreateDmChannelAsync();
                await a.SendMessageAsync(Textos.TextoBoasVindas());
            };

            Client.GuildAvailable += async (s, e) =>
            {
                if(!StartBotServices.ChannelLog.ContainsKey(e.Guild.Id.ToString()))
                {
                    await StartBotServices.SaveInfo.RegisterNewGuild(e.Guild.Id, e.Guild.Owner.Id);
                }
            };
        }


        /*Evento de Botões pressionados

        Implementar Padrão strategy futuramente ou algo que diminua os IFs e deixe o app mais eficiente*/


        
        public async Task Botao_Pressionado()
        {
            Client.ComponentInteractionCreated += async (s, e) =>
            {

                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (StartBotServices.Users.Contains(e.User.Id.ToString()))
                        {
                            if (PossivelCompra.ContainsKey(e.User.Id.ToString()) && e.Id.Contains("cash"))
                            {

                               /* var prize = OpMessages.BotaoMoedaPres(e.Guild.GetMemberAsync
                                    (e.User.Id).Result, PossivelCompra[e.User.Id.ToString()], e.Id, e.Channel.Id).Result ;

                                await e.Interaction.CreateResponseAsync(
                                    InteractionResponseType.ChannelMessageWithSource,
                                    new DiscordInteractionResponseBuilder()
                                    .WithContent("Caso sua DM esteja fechada, " +
                                    "essa é sua compra:\n\n" + prize)
                                    .AsEphemeral(true)
                                    .WithTitle("Aqui está...")
                                    );

                                await e.Message.DeleteAsync();*/

                            }

                            else if (e.Id == "Jcash" || e.Id == "Scash")

                                await s.SendMessageAsync(await s.GetChannelAsync(e.Channel.Id),
                                    $"||{e.Interaction.User.Mention}||\n " +
                                    $"Você não pode interagir com esse pedido de compra, use !jcomprar" +
                                    $" para usar a loja de maneira correta");


                            else if (e.Id.Contains("anun"))
                            {
                                var buttons = StartBotServices.SaveEconomicOP.SaldoSuficiente(e.User.Id, "aJcash", "aScash").GetAwaiter().GetResult();


                                await e.Interaction.CreateResponseAsync(
                                    InteractionResponseType.ChannelMessageWithSource,
                                    new DiscordInteractionResponseBuilder()
                                    .WithContent("Qual moeda Você quer vender?")
                                    .AsEphemeral(true)
                                    .AddComponents(buttons)
                                    );
                            }

                            else if (e.Id == "aJcash" || e.Id == "aScash")
                            {
                                //Cria modal para receber valor a ser anunciado

                                var response = new DiscordInteractionResponseBuilder();

                                response
                                .WithTitle("Digite o total de moedas que você quer vender")
                                .WithCustomId("valor")
                                .AddComponents(new TextInputComponent("Modal", e.Id, null,
                                null, true, TextInputStyle.Short, 1, 5));

                                await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, response);

                            }

                        }

                        else
                        {
                            await e.Interaction.CreateResponseAsync(
                                   InteractionResponseType.ChannelMessageWithSource,
                                   new DiscordInteractionResponseBuilder()
                                   .WithContent($"{e.User.Mention} Você não pode interagir com esse botão porque " +
                                   $"você não está registrado, user !j Registrar para começar a usar os meus comandos ")
                                   .AsEphemeral(true)
                                   );
                        }

                    }


                    catch (Exception)
                    {
                        await e.Message.DeleteAsync();
                        await s.SendMessageAsync(await s.GetChannelAsync(e.Channel.Id),
                                $"||{e.Interaction.User.Mention}||\n " + "Erro 404 X(");
                        throw;
                    }
                });
            };
        }

        public static async Task BtnComprarLoja(DiscordMessage message, DiscordUser user, DiscordClient client)
        {
            var inter = message.WaitForButtonAsync(user).Result;
            string prize = "";

            if(!inter.TimedOut)
            prize = OpMessages.BotaoMoedaPres(inter.Result.Guild.GetMemberAsync(user.Id).Result,
               PossivelCompra[user.Id.ToString()], inter.Result.Id, inter.Result.Channel.Id).Result;

            /*await e.Interaction.CreateResponseAsync(
                InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                .WithContent("Caso sua DM esteja fechada, " +
                "essa é sua compra:\n\n" + prize)
                .AsEphemeral(true)
                .WithTitle("Aqui está...")
                );*/

            await message.DeleteAsync();
        }

        public static async Task BtnComprarAnuncio(DiscordMessage message, DiscordUser user)
        {

        }

        //Terminar VM

        public static async Task BtnVivoMortoP2(DiscordMessage message, string player1)
        {
            var inter = message.WaitForButtonAsync().Result;
            var player2 = inter.Result.User.Id.ToString();


            if (!inter.TimedOut && StartBotServices.Users.Contains(player2))
            {
                var vm = BotTimers.vivoMortos.Find(p => p.P1 == player1);

                vm.P2 = player2;
            }

            else
            {
                await inter.Result.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource
                    , new DiscordInteractionResponseBuilder()
                    .WithContent("Você não está cadastrado, use !jRegistrar para poder jogar esse jogo")
                    .AsEphemeral(true));
            }
        }



        //Eventos de Submissão de Modal

        public async Task ModalUp()
        {
            Client.ModalSubmitted += async (s, e) =>
            {

                if(e.Values.ContainsKey("aScash") || e.Values.ContainsKey("aJcash"))
                {
                    var a = e.Interaction.GetOriginalResponseAsync();


                    

                    /*var response = new DiscordInteractionResponseBuilder();

                    response
                    .WithTitle("Digite o valor que deseja cobrar")
                    .WithCustomId("valor")
                    .AddComponents(new TextInputComponent("Modal", e.Values.Keys.ToString().Remove(0, 1), 
                    null, null, true, TextInputStyle.Short, 1, 5));

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, response);*/

                }

                else if (e.Values.ContainsKey("Scash") || e.Values.ContainsKey("Jcash"))
                {

                }

            };
        }
    }
}