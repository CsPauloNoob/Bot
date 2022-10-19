using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Bot_Manager.Quests.Diarias;

namespace Bot_Manager
{
    public class MainTimer
    {

        public static Timer dayTimer = new Timer();

        public MainTimer()
        {

            StartTimer().GetAwaiter().GetResult();

        }


        private async Task StartTimer()
        {

            dayTimer.Elapsed += new ElapsedEventHandler(ResetDiario);
            dayTimer.AutoReset = true;
            dayTimer.Interval = 60000;
            dayTimer.Enabled = true;
            dayTimer.Start();
            

             Task.Delay(-1);
        }

        private async static void ResetDiario(object source, ElapsedEventArgs e)
        {
            await StartBotServices.Loja.ResetLoja();

            StartBotServices.Diarias.JaResgatou.Clear();

        }

    }
}