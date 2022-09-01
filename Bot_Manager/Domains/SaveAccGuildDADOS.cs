using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Manager.Logs_e_Coleta_de_Informacoes
{
    public class SaveAccGuildDADOS:ISaveAccGuildDADOS
    {
        DiscordClient client;

        public SaveAccGuildDADOS()
        {

        }

        #region Guild
        public async Task RegisterNewGuild(ulong Id, ulong Owner, ulong UserChannel)
        {
            if(IsANewGuild(Id))
            {
                await StartBotServices.GuildDAL.AddNewGuild(Id, Owner, UserChannel);
            }
            else
                OpMessages.GenericMessage(null, "Esse servidor já está registrado", UserChannel).GetAwaiter().GetResult();

        }

        public async Task RegisterLogChannel(ulong GuildId, ulong LogChannel, ulong UserChannel)
        {
                await StartBotServices.GuildDAL.SaveLogChannel(GuildId, LogChannel, UserChannel);
        }


        /*public bool IsANewLog(ulong id)
        {
            foreach (var i in StartBotServices.ChannelLog.Values)
            {
                if (i == id.ToString())
                {
                    return false;
                }
            }
            return true;
        }*/

         bool IsANewGuild(ulong id)
        {
            foreach (var i in StartBotServices.ChannelLog.Keys)
            {
                if (i == id.ToString())
                {
                    return false;
                }
            }
            return true;
        }
        #endregion


        public async Task RegisterMenber(ulong userId, ulong Tchannel)
        {
            if (IsANewUser(userId))
            {
                await StartBotServices.UserDAL.AddNewUser(userId, Tchannel);
            }
            else
                await OpMessages.GenericMessage(null, "Usuário já registrado", Tchannel);
        }

        
        bool IsANewUser(ulong Id)
        {
            if (StartBotServices.Users.Contains(Id.ToString()))
                return false;
            else
                return true;
        }

    }
}
