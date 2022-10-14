using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace Bot_Manager.Domains.Operacoes_da_Loja.DbOperations
{
    public class ItensValue
    {

        public int[] valueInactiveNitro;

        public int[] valueClassicNitro;

        public ItensValue(int[] classicNitro, int[] Initro)
        {
            valueInactiveNitro = Initro;
            valueClassicNitro = classicNitro;
        }




        public async Task<Dictionary<string, List<int>>> TOdosOsValores()
        {
            int aux = 0;
            int aux2 = 0;

            var lista = new List<int>();

            var resultado = new Dictionary<string, List<int>>();

            foreach(var valor in valueClassicNitro)
            {
                if(aux == 0)
                aux = valor;
                else
                    aux2 = valor;
            }

            lista.Add(aux);
            lista.Add(aux2);
            aux = 0;
            aux2 = 0;
            resultado.Add("Classico", lista);

            lista.Clear();

            foreach (var valor in valueInactiveNitro)
            {
                if (aux == 0)
                    aux = valor;
                else
                    aux2 = valor;
            }

            lista.Add(aux);
            lista.Add(aux2);
            resultado.Add("Inativo", lista);

            return resultado;
        }





        public int[] ValorDe(string item)
        {
            if (item == "1")
                return valueClassicNitro;
            else if (item == "2")
                return valueInactiveNitro;
            else if (item == "3")
                return StartBotServices.Loja.valorItemG;
            else
                return null;
        }





        public int ValorDe(string item, string money)
        {
            if (item == "1" && money == "Jcash")
                return valueClassicNitro[0];
            else if (item == "2" && money == "Jcash")
                return valueInactiveNitro[0];
            else if (item == "1" && money == "Scash")
                return valueClassicNitro[1];
            else if (item == "2" && money == "Scash")
                return valueInactiveNitro[1];
            else if (item == "3" && money == "Scash")
                return StartBotServices.Loja.valorItemG[1];
            else if (item == "3" && money == "Jcash")
                return StartBotServices.Loja.valorItemG[0];

                return 0;
        }
    }
}