using System;
using System.Collections.Generic;
using System.Windows;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Exceptions;
using DSharpPlus.Entities;
using Bot_Manager.Domains;

namespace Bot_Manager
{
    public class LogInformations : ILogInformations
    {

        DiscordClient _client;
        public LogInformations()
        {
            
        }

        public void GetMessageEvents(DiscordClient client)
        {
            _client = client;
            
            CatchEventsMessage().GetAwaiter().GetResult();

        }


       private async Task CatchEventsMessage()
        {

            #region Eventos Menssagens para o Log
            // Detecta qualquer menssagem no servidor

            try
            {
                _client.MessageCreated += async (e, s) =>
                {
                    _ = Task.Run(async () =>
                    {
                        if (!s.Message.Author.IsBot &&
                        StartBotServices.ChannelLog.ContainsKey(s.Guild.Id.ToString()))

                            await BuidLogMessages("Escreveu: ``` " + s.Message.Content +
                                "``` \nEm " + s.Channel.Name + " as " + DateTime.Now,
                                s.Message.Author.Username +
                                s.Author.Discriminator,
                                StartBotServices.ChannelLog[s.Guild.Id.ToString()]);
                    });
                };

                _client.MessageDeleted += async (e, s) =>
                {
                    _ = Task.Run(async () =>
                    {
                        if (!s.Message.Author.IsBot &&
                        StartBotServices.ChannelLog.ContainsKey(s.Guild.Id.ToString()))

                            await BuidLogMessages("Deletou: ``` " + s.Message.Content +
                                "``` \nEm " + s.Channel.Name + " as " + DateTime.Now,
                                s.Message.Author.Username +
                                s.Message.Author.Discriminator,
                                StartBotServices.ChannelLog[s.Guild.Id.ToString()]);
                    });
                };

                _client.MessageUpdated += async (e, s) =>
                {
                    _ = Task.Run(async () =>
                    {
                        if (!s.Message.Author.IsBot &&
                        StartBotServices.ChannelLog.ContainsKey(s.Guild.Id.ToString()))

                            await BuidLogMessages("Editou: ``` " + s.MessageBefore.Content + "```" +
                                "\nPara: ```" + s.Message.Content + "``` \nEm " + s.Channel.Name
                                + " as " + DateTime.Now, s.Message.Author.Username +
                                s.Author.Discriminator,
                                StartBotServices.ChannelLog[s.Guild.Id.ToString()]);
                    });
                };
            }

            catch (Exception)
            {
                throw;
            }

            await Task.Delay(-1);
            
            #endregion
        }

        private async Task BuidLogMessages(string message, string user, string logchannel)
        {
            var log = ulong.Parse(logchannel);
            var msg = new DiscordMessageBuilder()
                .WithEmbed(EmbedMesages.BuildLogMessageDefault(message, user))
                .SendAsync(_client.GetChannelAsync(log).Result);
        }

    }
}