﻿using System;
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
        }


        public DiscordButtonComponent EsperarP2()
        {
            DiscordButtonComponent component = new DiscordButtonComponent(ButtonStyle.Primary, P1, "Esperando jogador 2");
            
            Button = component;

            Random random = new Random();

            Time.AutoReset = false;
            Time.Interval = 60000;
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

            Button.Disable();

        }



        public async Task IniciarGame(DiscordMessage message, DiscordUser user)
        {
            Time.Interval = 50000;
            Time.Start();
            var client = StartBotServices.Client;
            await Resposta_Eventos.BtnVivoMortoP2(message, P1);
            var ch = message.Channel;
            await message.DeleteAsync();
            DiscordButtonComponent component = new DiscordButtonComponent(ButtonStyle.Primary, P1, "Iniciar");


            var message2 = await client.SendMessageAsync(ch, EmbedMesages.EmbedButton("Prontos?", 
                $"{client.GetUserAsync(ulong.Parse(P2)).Result.Mention} e {client.GetUserAsync(ulong.Parse(P1)).Result.Mention}" +
                $" cliquem no botão para iniciar! E quando o botão Verde aparecer na tela, vence quam" +
                $" clicar nele primeiro!", DiscordColor.Yellow, component));

            var interaction = message2.WaitForButtonAsync(user).Result;

            if(!interaction.TimedOut)
            {
                await message2.DeleteAsync();
                component = new DiscordButtonComponent(ButtonStyle.Primary, P1, "Disparar");

                var message3 = await client.SendMessageAsync(ch, EmbedMesages.EmbedButton("**Fogo**",
                $"{client.GetUserAsync(ulong.Parse(P2)).Result} vs. {client.GetUserAsync(ulong.Parse(P1)).Result}", DiscordColor.Green, component));

                var inter = message3.WaitForButtonAsync().Result;

                if(!inter.TimedOut)
                {
                    if(message3.Interaction.User.Id.ToString() == P1)
                    {
                        if(await StartBotServices.SaveEconomicOP.AdcionarSaldo(ulong.Parse(P1), Convert.ToInt16(ValorAposta), "Scash"))
                        {
                            await message3.DeleteAsync();
                            await client.SendMessageAsync(ch, EmbedMesages.UniqueLineMsg($"Parabéns {client.GetUserAsync(ulong.Parse(P1)).Result.Mention} " +
                                $"você ganhou o jogo e levou {ValorAposta} Scash"));

                            Time.Stop();
                            BotTimers.vivoMortos.Remove(this); //validações
                        }
                    }
                }

            }
        }
    }
}