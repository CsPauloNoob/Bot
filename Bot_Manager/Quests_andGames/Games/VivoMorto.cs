using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Bot_Manager.Logs_e_Eventos;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace Bot_Manager.Quests_andGames.Games
{
    public class VivoMorto
    {

        public string P1 { get; set; }

        public string P2 { get; set; }

        ushort ValorAposta { get; set; }

        DiscordButtonComponent Button;

        public DiscordMessage DiscordMessage { get; set; }

        Timer Time = new Timer();


        public VivoMorto(string p1, ushort valorAposta)
        {
            P1 = p1;

            BotTimers.vivoMortos.Add(this);
            ValorAposta = valorAposta;

            Resposta_Eventos.FilaVivoMorto.Add(P1);
        }


        public DiscordButtonComponent EsperarP2()
        {
            DiscordButtonComponent component = new DiscordButtonComponent(ButtonStyle.Primary, P1, "Esperando jogador 2");
            
            Button = component;

            Random random = new Random();

            Time.AutoReset = false;
            Time.Interval = 40000;
            Time.Enabled = true;
            Time.Elapsed += new ElapsedEventHandler(Esperatimeout);
            Time.Start();

            return Button;
        }


        private async void Esperatimeout(object source, ElapsedEventArgs e)
        {
            BotTimers.vivoMortos.Remove(this);
            Resposta_Eventos.FilaVivoMorto.Remove(P1);
            await StartBotServices.SaveEconomicOP.AdcionarSaldo(ulong.Parse(P1), ValorAposta, "Scash");

            if(DiscordMessage.Content != null)
            {
                await DiscordMessage.DeleteAsync();
            }

        }



        public async Task<bool> IniciarGame(DiscordMessage message, DiscordUser user)
        {   DiscordMessage = message;
            
            if (await Resposta_Eventos.BtnVivoMortoP2(message, P1))
            {
                var aposta_total = ValorAposta + ValorAposta;
                var client = StartBotServices.Client;
                var ch = message.Channel;
                await message.DeleteAsync();
                DiscordButtonComponent component = new DiscordButtonComponent(ButtonStyle.Primary, P1, "Iniciar");
                Time.Stop();
                Time.Interval = 20000;

                var message2 = await client.SendMessageAsync(ch, EmbedMesages.EmbedButton("Prontos?",
                    $"{client.GetUserAsync(ulong.Parse(P2)).Result.Username} e {client.GetUserAsync(ulong.Parse(P1)).Result.Username}" +
                    $" cliquem no botão para iniciar! E quando o botão Verde aparecer na tela, vence quam" +
                    $" clicar nele primeiro!", DiscordColor.Yellow, component));

                DiscordMessage = message2;
                var interaction = message2.WaitForButtonAsync(user).Result;

                if (!interaction.TimedOut)
                {
                    Time.Start();
                    await message2.DeleteAsync();
                    component = new DiscordButtonComponent(ButtonStyle.Success, P1, "Disparar");

                    var message3 = await client.SendMessageAsync(ch, EmbedMesages.EmbedButton("**Fogo**",
                    $"{client.GetUserAsync(ulong.Parse(P2)).Result.Username} vs. {client.GetUserAsync(ulong.Parse(P1)).Result.Username}", DiscordColor.Green, component));

                    DiscordMessage = message3;
                    var inter = message3.WaitForButtonAsync().Result;

                    if (!inter.TimedOut)
                    {
                        if (inter.Result.User.Id.ToString() == P1)
                        {
                            if (await StartBotServices.SaveEconomicOP.AdcionarSaldo(ulong.Parse(P1), Convert.ToInt16(aposta_total), "Scash"))
                            {
                                Time.Stop();
                                await message3.DeleteAsync();
                                Resposta_Eventos.FilaVivoMorto.Remove(P1);
                                await client.SendMessageAsync(ch, EmbedMesages.UniqueLineMsg($"Parabéns {client.GetUserAsync(ulong.Parse(P1)).Result.Username} " +
                                    $"você ganhou o jogo e levou {ValorAposta} Scash"));

                                await StartBotServices.SaveEconomicOP.DebitarSaldo(ulong.Parse(P2), ValorAposta, "Scash");
                                BotTimers.vivoMortos.Remove(this);
                                return true;
                            }
                        }

                        else if (inter.Result.User.Id.ToString() == P2)
                        {
                            if (await StartBotServices.SaveEconomicOP.AdcionarSaldo(ulong.Parse(P2), aposta_total, "Scash"))
                            {
                                Time.Stop();
                                await message3.DeleteAsync();
                                Resposta_Eventos.FilaVivoMorto.Remove(P1);
                                await client.SendMessageAsync(ch, EmbedMesages.UniqueLineMsg($"Parabéns {client.GetUserAsync(ulong.Parse(P2)).Result.Mention} " +
                                    $"você ganhou o jogo e levou {ValorAposta} Scash"));

                                await StartBotServices.SaveEconomicOP.DebitarSaldo(ulong.Parse(P1), ValorAposta, "Scash");
                                BotTimers.vivoMortos.Remove(this);
                                return true;
                            }
                        }
                    }

                }
            }


            else
            {
                BotTimers.vivoMortos.Remove(this);

                if(message.Content != null)
                await message.DeleteAsync();

                Resposta_Eventos.FilaVivoMorto.Remove(P1);
                await StartBotServices.SaveEconomicOP.AdcionarSaldo(ulong.Parse(P1), ValorAposta, "Scash");
                Time.Enabled = false;
                Time.Close();
                Time.Dispose();
            }

            return false;
        }
    }
}