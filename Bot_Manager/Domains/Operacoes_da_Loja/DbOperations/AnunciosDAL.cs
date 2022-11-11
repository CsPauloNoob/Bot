using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Reflection;
using Bot_Manager.Models;

namespace Bot_Manager.Domains.Operacoes_da_Loja.DbOperations
{
    public class AnunciosDAL
    {

        private string ConnStr = StartBotServices.ConnString;

        SQLiteConnection SqliteCon;

        public void OpenConn()
        {
            try
            {
                SqliteCon = new SQLiteConnection(ConnStr);
                if (SqliteCon.State == ConnectionState.Closed)
                {
                    SqliteCon.Open();
                }
            }

            catch (Exception)
            {
                SqliteCon?.Close();
            }

        }


        public async Task<List<Anuncio>> CarregarAnuncios()
        {
            try
            { 
                OpenConn();

                string[] arr = new string[5];

                var i = 0;
                int aux = 1;

                //Dictionary<string, string[]> result = new Dictionary<string, string[]>();

                List<Anuncio> anuncios = new List<Anuncio>();

                using (var cmd = new SQLiteCommand(Test.SQL_GET_AN, SqliteCon))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.ReadAsync().Result)
                        {
                            anuncios.Add(new Anuncio(
                                aux.ToString(),
                                reader["User"].ToString(),
                                reader["Item"].ToString(),
                                reader["Item_qtde"].ToString(),
                                reader["Item_valor"].ToString(),
                                reader["Pag_tipo"].ToString()
                                ));

                            /*arr[0] = aux.ToString();
                            arr[1] = reader["Item"].ToString();
                            arr[2] = reader["Item_qtde"].ToString();
                            arr[3] = reader["Item_valor"].ToString();
                            arr[4] = 

                            result.Add(reader["User"].ToString()+i, arr.ToArray());*/

                            i++;
                            aux++;
                            i = i == 9 ? 0 : i;
                        }

                    }
                }

                return await Task.FromResult(anuncios);
            }

            catch (Exception ex)
            {
                SqliteCon.Close();

                return null;
            }
        }


        public async Task<bool> AdcionarAnuncio(string id, string item, string item_qtde, string item_valor, string Pag_tipo)
        {
            try
            {
                OpenConn();

                using (var cmd = new SQLiteCommand(Test.SQL_ADD_AN+$"('{id}', " +
                    $"'{item}', '{item_qtde}', '{item_valor}', '{Pag_tipo}')", SqliteCon))
                {
                    if(cmd.ExecuteNonQuery() > 0)
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

            return false;
        }







        public async Task<bool> AdcionarAnuncio(Anuncio anuncio)
        {
            try
            {
                OpenConn();

                using (var cmd = new SQLiteCommand(Test.SQL_ADD_AN + $"('{anuncio.Id_Vendedor}', " +
                    $"'{anuncio.Moeda}', '{anuncio.Quantidade}', '{anuncio.Valor_Receber}', " +
                    $"'{anuncio.Moeda_pagamento}')", SqliteCon))
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        SqliteCon.Close();
                        return true;
                    }
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
                return false;
            }

            return false;
        }







        public async Task<bool> RemoverAnuncio(ulong id)
        {
            try
            {
                OpenConn();

                using (var cmd = new SQLiteCommand(Test.SQL_REMOVE_AN+$"'{id.ToString()}'", SqliteCon))
                {
                    if(cmd.ExecuteNonQuery() > 0)
                    {
                        SqliteCon.Close();
                        return true;
                    }
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
                return false;
            }

            return false;

        }


        public void RemoverTodosAnuncio()
        {
            try
            {
                OpenConn();

                using (var cmd = new SQLiteCommand("DELETE FROM ANUNCIOS", SqliteCon))
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        SqliteCon.Close();
                    }
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

        }


    }
}