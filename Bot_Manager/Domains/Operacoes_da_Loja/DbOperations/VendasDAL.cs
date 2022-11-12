using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Bot_Manager.Domains.Operacoes_da_Loja.DbOperations
{
    public class VendasDAL
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



        public Task<bool> ComitarVenda(string item, string link, 
            int preco, ulong idComprador, string moeda)
        {
            var result = false;
            OpenConn();
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_ADD_VENDA +
                    $"('{item}', '{link}', '{preco}', '{DateTime.Now.ToString("dd/MM/yyyy")}'" +
                    $", '{idComprador}', '{moeda}')", SqliteCon))
                {
                    if (cmd.ExecuteNonQuery() > 0)
                        result = true;
                    SqliteCon.Close();
                }
            }

            catch(Exception ex)
            {
                var local = this.GetType().Name;
                local += "."+MethodBase.GetCurrentMethod().Name;
                StartBotServices.Client.SendMessageAsync(
                    StartBotServices.Client.GetChannelAsync(
                    StartBotServices.CanalExceptions).Result, ex.Message + " in " + $"```{local}```");

                SqliteCon.Close();
                throw ex;
            }

            return Task.FromResult(result);
        }


        public async Task<List<string[]>> ConsultarVenda(ulong id)
        {
            
                OpenConn();

                List<string[]> result = new List<string[]>();
                string[] temp = new string[6];

                try {
                using (SQLiteCommand cmd = new SQLiteCommand($"select * from vendasok where vendido_para = {id}" +
                    $" order by(dt_venda) desc", SqliteCon))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.ReadAsync().Result)
                        {
                            result.Add(new string[] {
                            reader["Item_Name"].ToString(),
                            reader["Link"].ToString(),
                            reader["Price"].ToString(),
                            reader["dt_venda"].ToString(),
                            reader["Vendido_para"].ToString(),
                            reader["Tipo_moeda"].ToString()
                        });
                        }

                        await SqliteCon.CloseAsync();
                    }

                }
            }

            catch(Exception ex)
            {

            }

            return result;
        }

    }
}