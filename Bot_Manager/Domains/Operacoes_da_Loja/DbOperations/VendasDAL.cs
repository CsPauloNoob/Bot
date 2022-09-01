using System;
using System.Data.SQLite;
using System.Collections.Generic;
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

                    else
                    SqliteCon.Close();
                }
            }

            catch(Exception ex)
            {
                SqliteCon.Close();
                throw ex;
            }

            return Task.FromResult(result);
        }

    }
}
