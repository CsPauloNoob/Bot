using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Manager.Domains.Operacoes_da_Loja.DbOperations
{
    public class DbConfig
    {
        private string ConnStr = StartBotServices.ConnString;

        SQLiteConnection SqliteCon;

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
                var local = this.GetType().Name;
                local += "." + MethodBase.GetCurrentMethod().Name;
                StartBotServices.Client.SendMessageAsync(
                    StartBotServices.Client.GetChannelAsync(
                    StartBotServices.CanalExceptions).Result, ex.Message + " in " + $"```{local}```");

                SqliteCon?.Close();
            }
        }


        public async Task<bool> RealizarAltTable(string command)
        {
            OpenConn();

            using (SQLiteCommand cmd = new SQLiteCommand(command, SqliteCon))
            {
                try
                {
                    await SqliteCon.ExecuteAsync("drop table vendasok");

                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }

                catch(Exception ex)
                {
                    var local = this.GetType().Name;
                    local += "." + MethodBase.GetCurrentMethod().Name;
                   await StartBotServices.Client.SendMessageAsync(
                        StartBotServices.Client.GetChannelAsync(
                        StartBotServices.CanalExceptions).Result, ex.Message + " in " + $"```{local}```");

                    SqliteCon?.Close();

                    return false;
                }
            }
        }


    }
}
