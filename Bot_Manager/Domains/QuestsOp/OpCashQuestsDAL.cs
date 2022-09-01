using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace Bot_Manager.Domains.QuestsOp
{
    public class OpCashQuestsDAL
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

        public bool AdcionarValorCmdDiario(ulong id, string valor)
        {
            OpenConn();
            using (SQLiteCommand cmd = new SQLiteCommand(Test.SQL_UPDATE_Cash+" Scash ="+valor.ToString()+
                " WHERE Id = "+id.ToString(), SqliteCon))
            {
                if (cmd.ExecuteNonQuery() != 0)
                {
                    SqliteCon.Close();
                    return true;
                }
            }

            return false;
        }

    }
}