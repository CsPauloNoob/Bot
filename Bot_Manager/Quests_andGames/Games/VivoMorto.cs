using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Bot_Manager.Logs_e_Eventos;
using DSharpPlus;
using DSharpPlus.Entities;

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



        public async Task IniciarGame(DiscordMessage message)
        {
            var client = StartBotServices.Client;
            await Resposta_Eventos.BtnVivoMortoP2(message, P1);
            await message.DeleteAsync();
            DiscordButtonComponent component = new DiscordButtonComponent(ButtonStyle.Primary, P1, "Disparar");


            message = await client.SendMessageAsync(message.Channel, EmbedMesages.EmbedButton("oi", "Em algum momento um " +
                "botão aparecerá aqui, fiquem espertos!", DiscordColor.Green, component));

            
            
            await Task.Delay(50000);

            component.Disable();
        }
    }
}