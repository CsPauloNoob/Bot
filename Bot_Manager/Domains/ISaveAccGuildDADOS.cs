using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Manager.Logs_e_Coleta_de_Informacoes
{
    public interface ISaveAccGuildDADOS
    {
        public Task<bool> RegisterMenber(ulong UserId);
        public Task RegisterLogChannel(ulong GuildId, ulong logchannel);
        public Task RegisterNewGuild(ulong Id, ulong Owner);
    }
}