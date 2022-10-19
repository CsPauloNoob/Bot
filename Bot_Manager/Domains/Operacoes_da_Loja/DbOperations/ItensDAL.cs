using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;
using System.Data.SQLite;
using System.Data;
using System.Threading.Tasks;
using System.Reflection;
using Bot_Manager.Models;

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





        public async Task<bool> NovoItem(string nome, string item, string Jcash, string Scash)
        {
            try
            {
                OpenConn();

                using( SQLiteCommand cmd = new SQLiteCommand(Test.SQL_ADD_Item+
                    $"('{nome}', '{item}', {Jcash}, {Scash})", SqliteCon))
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
                await StartBotServices.Client.SendMessageAsync(
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


        public void GetItens(ref List<ItemVariado> items)
        {
            OpenConn();

            try
            {
                var aux = 3;
                ItemVariado nitem = new ItemVariado();
                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_GET_ITENS, SqliteCon))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            nitem.Id = aux.ToString();
                            nitem.Nome = reader["Nome"].ToString();
                            nitem.conteudo = reader["item"].ToString();
                            nitem.Jcash = Convert.ToInt32(reader["Jprice"]);
                            nitem.Scash = Convert.ToInt32(reader["Sprice"]);
                            items.Add(nitem);

                            aux++;

                            nitem = new ItemVariado();
                        }

                        SqliteCon.Close();
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



        public async Task RemoverDb(string item, string tipo)
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
                    using (SQLiteCommand cmd = new SQLiteCommand($"delete from Itens_Variados where item ='{item}'", SqliteCon))
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