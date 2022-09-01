using System;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Diagnostics;
using System.Threading.Tasks;
using Bot_Manager.Logs_e_Coleta_de_Informacoes;
using Bot_Manager.ComandosTexto;
using DSharpPlus.CommandsNext;

namespace Bot_Manager
{
    public class BotController
    {       

        public BotController()
        {
            new StartBotServices();
        }

    }
}