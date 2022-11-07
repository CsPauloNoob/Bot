using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Bot_Manager.Quests_andGames.Games
{
    public class VivoMorto
    {

        private string P1 { get; set; }

        private string P2 { get; set; }

        private List<DiscordButtonComponent> Butoes;


        public VivoMorto(string p1)
        {
            P1 = p1;

            BotTimers.vivoMortos.Add(this);
        }


        public DiscordButtonComponent EsperarP2()
        {
            DiscordButtonComponent component = new DiscordButtonComponent(ButtonStyle.Danger, P1, "Esperando jogador 2", true);

            //Butoes.Add(component);

            Random random = new Random();

            Timer tm = new Timer();


            tm.Elapsed += new ElapsedEventHandler(Esperatimeout);
            tm.AutoReset = false;
            tm.Interval = 30000;
            tm.Enabled = true;
            tm.Start();

            return component;
        }

        private async void Esperatimeout(object source, ElapsedEventArgs e)
        {
            BotTimers.vivoMortos.Remove(this);
        }

    }
}