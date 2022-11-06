using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Manager.Quests_andGames.Games
{

    public class Roleta
    {

        Dictionary<int, string> Ganhadores = new Dictionary<int, string>();

        Dictionary<ulong, int> JogadasPlayers = new Dictionary<ulong, int>();

        public int ValorPremio = 300;


        public async Task NovoGanhador(string name)
        {
            Ganhadores.Add(Ganhadores.Count + 1, name);
        }


        public async Task<bool> Girar(uint num)
        {
            var r = new Random();

            if (r.Next(9) == num)
                return true;

            return false;
        }


        public async Task SetValorPremio(int valor)
        {
            ValorPremio = valor;
        }

    }

}