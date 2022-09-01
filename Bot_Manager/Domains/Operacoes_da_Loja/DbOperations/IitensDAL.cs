using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Bot_Manager.Domains.Operacoes_da_Loja.DbOperations
{
    public interface IitensDAL
    {

        public void GetclassicNitro(ref List<string> list) { }

        public void GetInactiveNitro(ref List<string> list) { }

        public void RemoverLoja() { }

    }
}
