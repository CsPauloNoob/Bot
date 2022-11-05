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

        public static Timer DayTimer = new Timer();

        public MainTimer()
        {
            StartTimer().GetAwaiter().GetResult();

        }


        private async Task StartTimer()
        {

            DayTimer.Elapsed += new ElapsedEventHandler(ResetDiario);
            DayTimer.AutoReset = true;
            DayTimer.Interval = TimeSpan.Parse("23:59:59.999").Milliseconds;
            DayTimer.Enabled = true;
            DayTimer.Start();
            

            Task.Delay(-1);
        }

        private async static void ResetDiario(object source, ElapsedEventArgs e)
        {

            await StartBotServices.Loja.ResetLoja();
            StartBotServices.Diarias.JaResgatou.Clear();

        }

    }
}