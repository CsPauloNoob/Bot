using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System.Threading.Tasks;
using DSharpPlus;

namespace Bot_Manager
{
    public class OpMessages
    {
        public static DiscordClient Client;



        public static async Task GenericMessage(Exception? ex, string message, ulong IdChannel)
        {
            await Client.SendMessageAsync(Client.GetChannelAsync(IdChannel).Result, message);
        }


        public static async Task<DiscordMessageBuilder> Menssagen_de_Compra(DiscordClient client, List<DiscordButtonComponent> buttons, DiscordUser user)
        {
            
            DiscordMessageBuilder message = new DiscordMessageBuilder()
                .AddComponents(buttons)
                .AddEmbed(EmbedMesages.UniqueLineMsg($"||{user.Mention}|| Selecione a moeda desejada"));
            return message;
        }

        public static async Task BotaoMoedaPres(DiscordMember member, string item, string moneytype, ulong channel)
        {
            try
            {
                var msg = await member.CreateDmChannelAsync();
                var prize = await StartBotServices.SaveEconomicOP.DisponibilizarItem(member.Id, item, moneytype);

                if (prize != null)
                {

                    await Client.SendMessageAsync(Client.GetChannelAsync(channel).Result
                                     , $"{member.Mention} :) entreuguei na sua DM, " +
                                     $"se você não recebeu, não se preocupe, seus pontos" +
                                     $" não serão descontados e você pode tentar novamente!");

                    await StartBotServices.SaveEconomicOP.DebitarSaldo(member.Id, StartBotServices.ItensValue.
                         ValorDe(item, moneytype), moneytype);

                    await msg.SendMessageAsync("Olá, aqui está seu item comprado");
                    await msg.SendMessageAsync(prize);

                    StartBotServices.SaveEconomicOP.ComitarVendaDb(item, prize, member.Id, moneytype).GetAwaiter();

                    await StartBotServices.Itens_Loja.RemoverItem(item);

                }

            }

            catch(Exception)
            {
                await Client.SendMessageAsync(Client.GetChannelAsync(channel).Result,
                    EmbedMesages.UniqueLineMsg("Um erro aconteceu, talvez sua DM esteja bloqueada?"));
            }
        }

        public static async Task<DiscordMessageBuilder> MsgGenericaComponent(List<DiscordButtonComponent> buttons)
        {

            DiscordMessageBuilder message = new DiscordMessageBuilder()
                .AddComponents(buttons)
                .AddEmbed(EmbedMesages.UniqueLineMsg("kkkk"));
            return message;
        }

    }
}