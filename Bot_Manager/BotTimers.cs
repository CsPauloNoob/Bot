using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Bot_Manager.Quests.Diarias;
using Bot_Manager.Quests_andGames.Games;

namespace Bot_Manager
{
    public class BotTimers
    {

        public Timer DayTimer = new Timer();

        public static List<VivoMorto> vivoMortos = new List<VivoMorto>();

        public BotTimers()
        {
            StartTimer().GetAwaiter().GetResult();

        }


        private async Task StartTimer()
        {

            DayTimer.Elapsed += new ElapsedEventHandler(ResetDiario);
            DayTimer.AutoReset = true;
            DayTimer.Interval = 86400000;
            DayTimer.Enabled = true;
            DayTimer.Start();
            

            Task.Delay(-1);
        }

        private async void ResetDiario(object sender, ElapsedEventArgs e)
        {
            
            await StartBotServices.Loja.ResetLoja();
            StartBotServices.Diarias.JaResgatou.Clear();

        }


        public async static Task FimJogo(object source, ElapsedEventArgs e)
        {
            
        }

    }
}