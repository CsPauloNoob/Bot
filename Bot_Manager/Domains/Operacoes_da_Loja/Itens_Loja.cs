using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Manager.Domains.Operacoes_da_Loja
{
    public class Itens_Loja
    {
        private List<string> ClassicNitro = new List<string>();

        private List<string> InactiveNitro = new List<string>();

        public Dictionary<string, int> IndexItem = new Dictionary<string, int>();

        public Dictionary<string, string> Variados = new Dictionary<string, string>();

        private int Total_Itens;




        public Itens_Loja()
        {
            CarregarItensDB();
        }


        public void RestartItens()
        {
            List<string> Initro = new List<string>();
            List<string> classicnitro = new List<string>();


            StartBotServices.ItensDAL.GetInactiveNitro(ref Initro);
            StartBotServices.ItensDAL.GetclassicNitro(ref classicnitro);

            //Limpa tudo antes de buscar novos itens
            ClassicNitro.Clear();
            InactiveNitro.Clear();
            IndexItem.Clear();
            Variados.Clear();

            ClassicNitro = classicnitro != null ? classicnitro : ClassicNitro;
            InactiveNitro = Initro != null ? Initro : InactiveNitro;


            if (ClassicNitro.Count > 0)
            {
                IndexItem.Add("1", ClassicNitro.Count);
            }

            if (InactiveNitro.Count > 0)
            {
                IndexItem.Add("2", InactiveNitro.Count);
            }

            Total_Itens = 0;

            Total_Itens += ClassicNitro.Count;
            Total_Itens += InactiveNitro.Count;
        }



        private void CarregarItensDB()
        {
            List<string> Initro = new List<string>();
            List<string> classicnitro = new List<string>();


            StartBotServices.ItensDAL.GetInactiveNitro(ref Initro);
            StartBotServices.ItensDAL.GetclassicNitro(ref classicnitro);

            
            ClassicNitro = classicnitro != null ? classicnitro : ClassicNitro;
            InactiveNitro = Initro != null ? Initro : InactiveNitro;


            if(ClassicNitro.Count>0)
            {
                IndexItem.Add("1", ClassicNitro.Count);
            }


            if (InactiveNitro.Count > 0)
            {
                IndexItem.Add("2", InactiveNitro.Count);
            }

            Total_Itens += ClassicNitro.Count;
            Total_Itens += InactiveNitro.Count;

        }

        public string darItem(string item)
        {
            string nitro = "";
            if (item == "1" && ClassicNitro.Count != 0)
            {
                nitro = ClassicNitro[0];
            }


            if (item == "2" && InactiveNitro.Count != 0)
            {
                nitro = InactiveNitro[0];
            }

            if (item == "3" && Variados.Count != 0)
            {
                nitro = Variados.Values.First();
            }

            return nitro;
        }


        public async Task<List<string>> TodosOsItens()
        {
            List<string> resultado = new List<string>();

            int aux = 0;

            foreach(var x in ClassicNitro)
            {
                resultado.Add("Nitro Clássico");
                break;
            }

            foreach (var x in InactiveNitro)
            {
                resultado.Add("Nitro novos assinantes");
                break;
            }

            foreach(var x in Variados.Keys)
            {
                resultado.Add(x);

            }

            return resultado;

        }


        #region Novos_itens a loja

        //Metodos para adcionar novos nitros e itrns variados a loja


        public async Task AdcionarCnitro(string link)
        {
            ClassicNitro.Add(link);

            Total_Itens++;
        }


        public async Task AdcionarInitro(string link)
        {
            InactiveNitro.Add(link);

            Total_Itens++;
        }


        public async Task<bool> Adcionaritem(string nome, string item)
        {

            Variados.Add(nome, item);

            Total_Itens++;

            if (IndexItem.Count < 3)
                IndexItem.Add("3", 1);

            return true;
        }

        #endregion


        public int TotalItens()
        {
            return Total_Itens;
        }


        public bool ItemAtivo(string item)
        {



            if(ClassicNitro.Count>0 && InactiveNitro.Count>0 && Variados.Count>0)
                return true;

            if (item == "2" && InactiveNitro.Count == 0)
                return false;

            if (item == "2" && InactiveNitro.Count > 0)
                return true;

            if (item == "1" && ClassicNitro.Count > 0)
                return true;

            if(item == "3" && Variados.Count>0)
            return true;

            return false;
        }

        public async Task RemoverItem(string item, string nome = "")
        {
            if (item == "1" && ClassicNitro.Count > 0)
                ClassicNitro.RemoveAt(0);

            else if (item == "2" && InactiveNitro.Count > 0)
                InactiveNitro.RemoveAt(0);

            else if (item == "3" && Variados.Count > 0)
                Variados.Remove(Variados.Keys.First());


            IndexItem[item]--;

            Total_Itens--;
        }
    }
}