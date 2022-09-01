using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;



namespace Bot_Manager.Quests
{
    public class Invite
    {
        public async Task<string> GerarConvite(DiscordChannel channel)
        {
            try
            {
                var a = await channel.CreateInviteAsync(600, 1);
                return await Task.FromResult(a.ToString());
            }

            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
