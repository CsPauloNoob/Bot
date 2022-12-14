using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;

namespace Bot_Manager.Domains
{
     public class GuildDAL : IGuildDAL
    {
        private string ConnStr = StartBotServices.ConnString;

        private static SQLiteConnection SqliteCon;

        public GuildDAL()
        {


        }


        private void OpenConn()
        {
            try
            {
                SqliteCon = new SQLiteConnection(ConnStr);
                if (SqliteCon.State == ConnectionState.Closed)
                {
                    SqliteCon.Open();
                }
            }

            catch (Exception ex)
            {
                SqliteCon?.Close();
            }
        }


        //relaciona todos os canais de log
        public async Task<Dictionary<string, string?>> GetAllGuildsChannel()
        {

            OpenConn();
            try
            {
                Dictionary<string, string?> dict = new Dictionary<string, string?>();

                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_Id_Log, SqliteCon))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dict.Add(reader["Id"].ToString(), reader["Log_Channel"].ToString());
                        }
                        SqliteCon.Close();
                        return dict;
                    }
                }
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;

        }



        public async Task AddNewGuild(ulong id, ulong owner)
        {

            OpenConn();
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_ADD_GUILD 
                    + $"('{id}', '{owner}', NULL)", SqliteCon))
                {
                    await cmd.ExecuteNonQueryAsync();

                    SqliteCon.Close();
                }
            }

            catch(Exception ex)
            {
                var local = this.GetType().Name;
                local += "." + MethodBase.GetCurrentMethod().Name;
                await StartBotServices.Client.SendMessageAsync(
                    StartBotServices.Client.GetChannelAsync(
                    StartBotServices.CanalExceptions).Result, ex.Message + " in " + $"```{local}```");

                SqliteCon.Close();
            }
        }

        public async Task DeleteGuild(ulong id)
        {

        }


        public async Task SaveLogChannel(ulong guild, ulong log)
        {

            OpenConn();
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_addLog1 +
                    $" '{log.ToString()}' " + Test.SQL_addLog2 + $" '{guild.ToString()}'", SqliteCon))
                {
                    await cmd.ExecuteNonQueryAsync();

                    SqliteCon.Close();

                    StartBotServices.ChannelLog[guild.ToString()] = log.ToString();

                }
            }

            catch (Exception ex)
            {
                var local = this.GetType().Name;
                local += "." + MethodBase.GetCurrentMethod().Name;
                await StartBotServices.Client.SendMessageAsync(
                    StartBotServices.Client.GetChannelAsync(
                    StartBotServices.CanalExceptions).Result, ex.Message + " in " + $"```{local}```");

                SqliteCon.Close();
            }

        }

    }
}
