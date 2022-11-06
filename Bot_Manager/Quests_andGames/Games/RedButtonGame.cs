using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Bot_Manager.Quests_andGames.Games
{
    public class RedButtonGame
    {

        private string P1 { get; set; }

        private string P2 { get; set; }

        private TimeSpan tempo { get; set; }

        private List<DiscordButtonComponent> Butoes;


        public RedButtonGame(string p1)
        {
            P1 = p1;
        }


        public void IniciarGame()
        {
            List<DiscordButtonComponent> components = new List<DiscordButtonComponent>();

            components.Add(new DiscordButtonComponent(ButtonStyle.Success, "P1", P1, true));
            components.Add(new DiscordButtonComponent(ButtonStyle.Danger, "P2", P2, true));

            Butoes = components;

            Random random = new Random();

            Timer tm = new Timer();


            tm.Elapsed += new ElapsedEventHandler(LiberarButton);
            tm.AutoReset = true;
            tm.Interval = random.Next(5500, 15000);
            tm.Enabled = true;
            tm.Start();
        }

        private async void LiberarButton(object source, ElapsedEventArgs e)
        {
            Butoes[0].Enable();
            Butoes[1].Enable();
        }

    }
}