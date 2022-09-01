using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;

namespace Bot_Manager.Quests.Diarias
{
    public class Cota_Diaria
    {

        public List<string> JaResgatou = new List<string>();

        private int valorResgate;

        public Cota_Diaria(int valor_resgate)
        {
            valorResgate = valor_resgate;
        }



        public async Task<bool> DarCota(ulong id)
        {
            var resultado = false;
            var valor = StartBotServices.UserWalletDAL.FundosCarteira(id).Result[1];

            valor += valorResgate;


            if (!JaResgatou.Contains(id.ToString()))
            {
                resultado = StartBotServices.OpCashQuestsDAL.
                    AdcionarValorCmdDiario(id, valor.ToString());

                JaResgatou.Add(id.ToString());
            }

            return resultado;

        }

    }

}