using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bot_Manager.Domains;
using DSharpPlus;

namespace Bot_Manager
{
    public interface ILogInformations
    {

        public void GetMessageEvents(DiscordClient client);

    }
}
