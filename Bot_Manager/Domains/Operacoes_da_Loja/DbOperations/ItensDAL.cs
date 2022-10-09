using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;
using System.Data.SQLite;
using System.Data;
using System.Threading.Tasks;
using System.Reflection;

namespace Bot_Manager.Domains.Operacoes_da_Loja.DbOperations
{
    public class ItensDAL: IitensDAL
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
                SqliteCon?.Close();
            }
        }





        public async Task<bool> NovoItem(string item)
        {
            try
            {
                OpenConn();

                using( SQLiteCommand cmd = new SQLiteCommand(Test.SQL_ADD_Item+
                    $"('{item}', 'false')", SqliteCon))
                {
                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        SqliteCon.Close();

                        return false;
                    }

                    else
                    {
                        SqliteCon.Close();

                        return true;
                    }

                }
            }

            catch(Exception ex)
            {
                var local = this.GetType().Name;
                local += "." + MethodBase.GetCurrentMethod().Name;
                StartBotServices.Client.SendMessageAsync(
                    StartBotServices.Client.GetChannelAsync(
                    StartBotServices.CanalExceptions).Result, ex.Message + " in " + $"```{local}```");

                SqliteCon.Close();

                return false;
            }
        }



        //retorna lista de links para nitros classicos(gifts do discord)

        public void GetclassicNitro(ref List<string>list)
        {

            OpenConn();

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_GET_CNitro, SqliteCon))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(reader["Link"].ToString());
                    }
                }

            }

            catch (Exception ex)
            {
                SqliteCon.Close();
            }
        }

        //retorna lista de links para nitros Inativos

        public void GetInactiveNitro(ref List<string> list)
        {

            OpenConn();

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_GET_Initro, SqliteCon))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(reader["Link"].ToString());
                    }
                }

            }

            catch (Exception ex)
            {
                SqliteCon.Close();
            }

        }


        public bool AddInactiveNitro(string link)
        {

            OpenConn();

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_ADD_INITRO+$"('{link}')", SqliteCon))
                {
                    if(cmd.ExecuteNonQueryAsync().Result>0)
                        return true;
                }

            }

            catch (Exception ex)
            {
                var local = this.GetType().Name;
                local += "." + MethodBase.GetCurrentMethod().Name;
                StartBotServices.Client.SendMessageAsync(
                    StartBotServices.Client.GetChannelAsync(
                    StartBotServices.CanalExceptions).Result, ex.Message + " in " + $"```{local}```");
                SqliteCon.Close();
            }

            return false;
        }


        public bool AddClassicNitro(string link)
        {

            OpenConn();

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_ADD_CNITRO + $"('{link}')", SqliteCon))
                {
                    if (cmd.ExecuteNonQuery() > 0)
                        return true;
                }

            }

            catch (Exception ex)
            {
                var local = this.GetType().Name;
                local += "." + MethodBase.GetCurrentMethod().Name;
                StartBotServices.Client.SendMessageAsync(
                    StartBotServices.Client.GetChannelAsync(
                    StartBotServices.CanalExceptions).Result, ex.Message + " in " + $"```{local}```");
                SqliteCon.Close();
            }

            return false;
        }

        public async Task RemoverLoja(string item, string tipo)
        {
            OpenConn();

            try
            {
                if (tipo == "1")
                    using (SQLiteCommand cmd = new SQLiteCommand($"delete from classic_nitro where link ='{item}'", SqliteCon))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }

               else if(tipo == "2")
                    using (SQLiteCommand cmd = new SQLiteCommand($"delete from inactive_nitro where link ='{item}'", SqliteCon))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }

                else
                    using (SQLiteCommand cmd = new SQLiteCommand($"delete from inactive_nitro where link ='{item}'", SqliteCon))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }


                await SqliteCon.CloseAsync();
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

    }
}