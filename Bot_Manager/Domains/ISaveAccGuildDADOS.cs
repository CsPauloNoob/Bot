using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Manager.Logs_e_Coleta_de_Informacoes
{
    public interface ISaveAccGuildDADOS
    {
        public Task RegisterMenber(ulong UserId, ulong TChannel);
        public Task RegisterLogChannel(ulong GuildId, ulong logchannel, ulong UserChannel);
        public Task RegisterNewGuild(ulong Id, ulong Owner, ulong UserChannel);
    }
}