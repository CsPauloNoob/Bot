using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SQLite;
using Bot_Manager.Domains;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
                new StartBotServices();
                Task.Delay(-1);
        }
    }
}