using Bot_Manager.Domains.Guilda_e_User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Manager.Domains
{
    public class UserDAL : IUserDAL
    {
        private string ConnStr = StartBotServices.ConnString;

        private static SQLiteConnection SqliteCon;
        void OpenConn()
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

        public Task<List<string>> GetAllUsers()
        {
            var list = new List<string>();

            try
            {
                OpenConn();
                using (var cmd = new SQLiteCommand(Test.SQL_Id_User, SqliteCon))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())

                            list.Add(reader["Id"].ToString());

                        SqliteCon.Close();
                        return Task.FromResult(list);
                    }
                }
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);  
            }

            finally
            {
                SqliteCon.Close();
            }

            return null;
        }


        public async Task AddNewUser(ulong UserId, ulong TChannel)
        {

            OpenConn();
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_ADD_USER+
                    $"('{UserId.ToString()}', '1500', '{DateTime.Now.ToString("dd/MM/yyyy")}'," +
                    $" '0')", SqliteCon))
                {
                    if(cmd.ExecuteNonQuery() <= 0)
                    {
                        OpMessages.GenericMessage(null, "Não foi possivel salvar seus dados, tente novamente"
                            , TChannel).GetAwaiter().GetResult();
                    }
                    else
                        OpMessages.GenericMessage(null, "Parabéns, Seus dados foram salvos"
                            , TChannel).GetAwaiter().GetResult();

                    StartBotServices.Users.Add(UserId.ToString());
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
