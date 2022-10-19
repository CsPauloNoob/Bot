using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Bot_Manager.Models;
using System.Security.Cryptography.X509Certificates;

namespace Bot_Manager.Domains.Operacoes_da_Loja
{
    public class Itens_Loja
    {
        private List<string> ClassicNitro = new List<string>();

        private List<string> InactiveNitro = new List<string>();

        public List<ItemVariado> Variados = new List<ItemVariado>();

        private int Total_Itens;




        public Itens_Loja()
        {
            CarregarItensDB();
        }


        public void RestartItens()
        {
            List<string> Initro = new List<string>();
            List<string> classicnitro = new List<string>();
            List<ItemVariado> items = new List<ItemVariado>();

            StartBotServices.ItensDAL.GetInactiveNitro(ref Initro);
            StartBotServices.ItensDAL.GetclassicNitro(ref classicnitro);
            StartBotServices.ItensDAL.GetItens(ref items);

            //Limpa tudo antes de buscar novos itens
            ClassicNitro.Clear();
            InactiveNitro.Clear();
            Variados.Clear();

            ClassicNitro = classicnitro != null ? classicnitro : ClassicNitro;
            InactiveNitro = Initro != null ? Initro : InactiveNitro;
            Variados = items != null ? items : Variados;

            //ItemsPreco();

            StartBotServices.ItensValue.ValorItensV.Clear();

            ItemsPreco();

            Total_Itens = 0;

            Total_Itens += ClassicNitro.Count;
            Total_Itens += InactiveNitro.Count;
            Total_Itens += Variados.Count;
        }

        private void ItemsPreco()
        {
            if(Variados.Count>0)
            {
                foreach(var x in Variados)
                {
                    StartBotServices.ItensValue.ValorItensV.Add(x.Id, new int[] { x.Jcash, x.Scash });
                }
            }
        }

        private void CarregarItensDB()
        {
            List<string> Initro = new List<string>();
            List<string> classicnitro = new List<string>();
            List<ItemVariado> items = new List<ItemVariado>();

            StartBotServices.ItensDAL.GetInactiveNitro(ref Initro);
            StartBotServices.ItensDAL.GetclassicNitro(ref classicnitro);
            StartBotServices.ItensDAL.GetItens(ref items);

            var aux = 3;
            

            ClassicNitro = classicnitro != null ? classicnitro : ClassicNitro;
            InactiveNitro = Initro != null ? Initro : InactiveNitro;
            Variados = items != null ? items : Variados;


            if (ClassicNitro.Count>0)
            {
                Total_Itens += ClassicNitro.Count;
            }


            if (InactiveNitro.Count > 0)
            {
                Total_Itens += InactiveNitro.Count;
            }


            if (Variados.Count > 0)
            {
                Total_Itens += Variados.Count;
            }

            ItemsPreco();

        }

        public string darItem(int item)
        {
            string prize = "";

            if (item == 1 && ClassicNitro.Count != 0)
            {
                prize = ClassicNitro[0];
            }


            if (item == 2 && InactiveNitro.Count != 0)
            {
                prize = InactiveNitro[0];
            }

            if (item >= 3 && Variados.Count != 0)
            {
                prize = Variados.Find(c => c.Id == item.ToString()).conteudo;
            }

            return prize;
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


        public async Task<bool> Adcionaritem(ItemVariado item)
        {
            try
            {
                Variados.Add(item);

                Total_Itens++;

                return true;
            }

            catch(Exception)
            {
                return false;
            }
        }

        #endregion


        public int TotalItens(uint item)
        {
            if(item < 3)
            {
                if (item==1)
                    return ClassicNitro.Count;

                if (item == 2)
                    return InactiveNitro.Count;
            }

            return Variados.Count;
        }


        public bool ItemAtivo(string item)
        {
            object resultado;

            if (int.Parse(item) > 2)
                return Variados.Exists(c => c.Id == item);

            if (item == "1" && ClassicNitro.Count > 0)
                return true;

            if (item == "2" && InactiveNitro.Count > 0)
                return true;

            return false;
        }


        public string NomeDe(string item)
        {
            string resultado="";
            if (item == "1")
                return "Nitro Clássico";

            else if (item == "2")
                return "Nitro novos assinantes";

            else
            resultado = Variados.Find(c => c.Id == item).Nome;

            return resultado;
        }


        public async Task RemoverItem(string item, string nome = "")
        {
            try
            {
                if (item == "1" && ClassicNitro.Count > 0)
                    ClassicNitro.RemoveAt(0);

                else if (item == "2" && InactiveNitro.Count > 0)
                    InactiveNitro.RemoveAt(0);

                else if (Variados.Count > 0)
                    Variados.Remove(Variados.Where(c => c.Id == item).First());

                Total_Itens--;
            }


            catch(Exception)
            {

            }
        }
    }
}