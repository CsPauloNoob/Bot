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

        DiscordButtonComponent button;


        public VivoMorto(string p1, ushort valorAposta)
        {
            P1 = p1;

            BotTimers.vivoMortos.Add(this);
            ValorAposta = valorAposta;
        }


        public DiscordButtonComponent EsperarP2()
        {
            DiscordButtonComponent component = new DiscordButtonComponent(ButtonStyle.Primary, P1, "Esperando jogador 2");
            
            button = component;

            Random random = new Random();

            Timer tm = new Timer();


            tm.Elapsed += new ElapsedEventHandler(Esperatimeout);
            tm.AutoReset = false;
            tm.Interval = 10000;
            tm.Enabled = true;
            tm.Start();

            return component;
        }


        private async void Esperatimeout(object source, ElapsedEventArgs e)
        {
            BotTimers.vivoMortos.Remove(this);
            Resposta_Eventos.FilaVivoMorto.Remove(P1);
            await StartBotServices.SaveEconomicOP.AdcionarSaldo(ulong.Parse(P1), ValorAposta, "Scash");

            button.Disable();

        }



        public async Task IniciarGame(DiscordMessage message)
        {

            var client = StartBotServices.Client;

            await message.ModifyAsync("PREPARADOS? JOGO INICIA EM *3*");
            await Task.Delay(1000);

            await message.ModifyAsync("PREPARADOS? JOGO INICIA EM *2*");
            await Task.Delay(1000);

            await message.ModifyAsync("PREPARADOS? JOGO INICIA EM *1*");
            await Task.Delay(1000);
        }
    }
}