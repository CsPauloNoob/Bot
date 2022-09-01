using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot_Manager
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        
        static void Main()
        {
                new BotController();
                Task.Delay(-1);
        }
    }
}