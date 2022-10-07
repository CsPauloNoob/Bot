using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Manager.Domains
{
    public interface IGuildDAL
    {

        Task<Dictionary<string, string>> GetAllGuildsChannel();

        Task AddNewGuild(ulong id, ulong owner);

        Task DeleteGuild(ulong id);

        public Task SaveLogChannel(ulong guild, ulong log);
    }
}