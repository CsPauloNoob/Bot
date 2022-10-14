using System;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Bot_Manager.Domains.Operacoes_da_Loja.DbOperations
{
    public class UserWalletDAL
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
                    SqliteCon.OpenAsync().Wait();
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


        public async Task<bool> AdcionarSaldo(ulong UserId, string valor, string moneyType)
        {
            OpenConn();

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_UPDATE_Cash + moneyType +
                    " = " + valor + " WHERE Id = " + UserId.ToString(), SqliteCon))
                {
                    if (cmd.ExecuteNonQuery() != 0)
                        return true;
                }

                return false;

            }

            catch(Exception ex)
            {
                var local = this.GetType().Name;
                local += "." + MethodBase.GetCurrentMethod().Name;
                await StartBotServices.Client.SendMessageAsync(
                    StartBotServices.Client.GetChannelAsync(
                    StartBotServices.CanalExceptions).Result, ex.Message + " in " + $"```{local}```");

                return false;
            }
        }


            public async Task<bool> DebitarSaldo(ulong UserId, string valor, string moneyType)
        {
            OpenConn();

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_UPDATE_Cash + moneyType +
                    " = "+valor+ " WHERE Id = "+ UserId.ToString(), SqliteCon))
                {
                    if(cmd.ExecuteNonQuery() != 0)
                        return true;
                }

                return false;
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


        public async Task<int[]> FundosCarteira(ulong userId)
        {
            OpenConn();
            int[] fundosCarteira = {0, 0};
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_cash_Read +
                        $"'{userId.ToString()}'", SqliteCon))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        fundosCarteira[0] = int.Parse(dr["Jcash"].ToString());
                        fundosCarteira[1] = int.Parse(dr["Scash"].ToString());
                    }
                    SqliteCon.Close();

                    return await Task.FromResult(fundosCarteira);
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

            return fundosCarteira;
        }

    }
}