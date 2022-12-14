using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bot_Manager.Models;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus;
using System.Reflection;

namespace Bot_Manager.Domains.Operacoes_da_Loja
{
    public class ComercioUsuarios
    {
        //substituir Dicionario de strings por um tipo anuncio

        List<Anuncio> Anuncios = new List<Anuncio>();

        //Dictionary<string, string[]> Anuncios_ON = new Dictionary<string, string[]>();

        public List<string> MaxAnuncioUser = new List<string>();


        public ComercioUsuarios()
        {
            Anuncios =  StartBotServices.AnunciosDAL.CarregarAnuncios().GetAwaiter().GetResult();
        }



        public async Task<bool> AnuncioAtivo(int id)
        {
            if(Anuncios.Where(p => p.Id_Anuncio == id.ToString()).Any())
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);

        }



        public async Task<int> ControleAnuncios(string id)
        {
            //Máx. de anuncios diários: 2/usuário

            var index = MaxAnuncioUser.FindAll(x => x.Equals(id));

            if (index.Count == 1)
            {
                MaxAnuncioUser.Add(id);

                return 1;
            }


            else if (index.Count == 0)
            {
                MaxAnuncioUser.Add(id);

                return 0;
            }

            else
                return 2;

        }




        public async Task<DiscordEmbed> View(DiscordClient client)
        {
            string linhas = "";
            string user = "";
            string Id = "";
            int id_count = 0;

            try
            {
                foreach(var anuncio in Anuncios)
                {
                    id_count = anuncio.Id_Vendedor.Count();
                    Id = anuncio.Id_Vendedor;

                    user = client.GetUserAsync(ulong.Parse(Id)).GetAwaiter().GetResult().Username;


                    linhas += $"\n> {anuncio.Id_Anuncio}. *{user}* V: **{anuncio.Quantidade}" +
                        $" {anuncio.Moeda} Por {anuncio.Valor_Receber} {anuncio.Moeda_pagamento}**";
                }



                /*foreach (var a in AnunciosON)
                {
                    id = a.Key.Length;

                    if(a.Key.Length > 18)
                        user = client.GetUserAsync(ulong.Parse(a.Key.Remove(id-1, 1))).GetAwaiter().GetResult().Username;

                    else
                        user = client.GetUserAsync(ulong.Parse(a.Key)).GetAwaiter().GetResult().Username;


                    linhas += $"\n> {a.Value[0]}. *{user}* V: **{a.Value[2]} {a.Value[1]} Por {a.Value[3]} {a.Value[4]}**";
                }*/


                return EmbedMesages.LojaUsuariosView(linhas);
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return EmbedMesages.LojaUsuariosView(linhas);
        }



        public async Task<bool> Anunciar(ulong id, string Nomemoeda, int valor, string valorAreceber, string moedapag)
        {
            int a= 0;


            if(Anuncios.Any())
                a = int.Parse(Anuncios.Last().Id_Anuncio);

            a = a + 1;

            var numAnuncio = ControleAnuncios(id.ToString()).Result;

            var novo_anuncio = new Anuncio(a.ToString(), id.ToString(), Nomemoeda,
                valor.ToString(), valorAreceber, moedapag);

            try
            {
                if(StartBotServices.AnunciosDAL.AdcionarAnuncio(novo_anuncio).GetAwaiter().GetResult())
                {
                    await StartBotServices.SaveEconomicOP.DebitarSaldo(id, valor, Nomemoeda);

                    Anuncios.Add(novo_anuncio);

                    return true;
                }
            }

            catch(Exception ex)
            {
                var local = this.GetType().Name;
                local += "." + MethodBase.GetCurrentMethod().Name;
                await StartBotServices.Client.SendMessageAsync(
                    StartBotServices.Client.GetChannelAsync(
                    StartBotServices.CanalExceptions).Result, ex.Message + " in " + $"```{local}```");
            }

            return false;







            /*
            if(Anuncios_ON.Count!=0)
             a = int.Parse(Anuncios_ON.Last().Value[0]);

            a = a + 1;

            var numAnuncio = ControleAnuncios(id.ToString()).Result;

            try
            {
                if (id.ToString() != null && valor.ToString() != null &&
                    StartBotServices.AnunciosDAL.AdcionarAnuncio(
                    id.ToString(), Nomemoeda, valor.ToString(), valorAreceber,
                    moedapag).GetAwaiter().GetResult() && numAnuncio!=2)

                {
                    string[] array = new string[5] { a.ToString(), Nomemoeda, valor.ToString(), valorAreceber, moedapag };

                    await StartBotServices.SaveEconomicOP.DebitarSaldo(id, valor, Nomemoeda);



                    if (numAnuncio == 0)
                        Anuncios_ON.Add(id.ToString(), array);

                    else
                        Anuncios_ON.Add(id.ToString() + "2", array);

                    return true;
                }
                else
                    return false;
            }

            catch(Exception)
            {
                throw;
            }*/

        }



        public async Task<bool> ItemVendido(ulong idC, string id_anuncio)
        {

            var item = Anuncios.Find(i => i.Id_Anuncio == id_anuncio);

            if(StartBotServices.SaveEconomicOP.DebitarSaldo(idC, int.Parse(item.Valor_Receber),
                item.Moeda_pagamento).GetAwaiter().GetResult() &&
                StartBotServices.SaveEconomicOP.AdcionarSaldo(idC, int.Parse(item.Quantidade),
                item.Moeda).GetAwaiter().GetResult())
            {

                await StartBotServices.SaveEconomicOP.AdcionarSaldo(ulong.Parse(item.Id_Vendedor),
                    int.Parse(item.Valor_Receber), item.Moeda_pagamento);

                await StartBotServices.AnunciosDAL.RemoverAnuncio(ulong.Parse(item.Id_Vendedor));

                Anuncios.Remove(item);

                return true;
            }


            return false;




            /*
            string idV = "";
            string[] item = new string[5];


            foreach(var x in Anuncios_ON)
            {
                if(x.Value[0]==id_item)
                {
                    idV = x.Key;
                    item = x.Value;
                    break;
                }
            }


            idV = idV.Length>18 ? idV.Remove(idV.Length-1, 1) : idV;

            if(idV!="")
            {
                //consumar compra e venda
                if (StartBotServices.SaveEconomicOP.DebitarSaldo(idC, int.Parse(item[3]), item[4]).GetAwaiter().GetResult() &&
                    StartBotServices.SaveEconomicOP.AdcionarSaldo(idC, int.Parse(item[2]), item[1]).GetAwaiter().GetResult())
                {
                        await StartBotServices.SaveEconomicOP.AdcionarSaldo(ulong.Parse(idV), int.Parse(item[3]), item[4]);

                        await StartBotServices.AnunciosDAL.RemoverAnuncio(ulong.Parse(idV));


                        Anuncios_ON.Remove(idV.ToString());

                    return true;
                }
                    
            }

            return false;*/

        }



        public Task<bool> Saldo_Sufciente(ulong id, string id_Anuncio)
        {

            var anuncio = Anuncios.Find(i => i.Id_Anuncio == id_Anuncio);
            var carteira = StartBotServices.UserWalletDAL.FundosCarteira(id).GetAwaiter().GetResult();

            if (anuncio.Moeda == "Scash" && carteira[1] >= int.Parse(anuncio.Quantidade))
                return Task.FromResult(true);

            else if (anuncio.Moeda == "Jcash" && carteira[0] >= int.Parse(anuncio.Quantidade))
                return Task.FromResult(true);

            else
                return Task.FromResult(false);


            /*
            string[] anuncio = new string[5];

            foreach(var x in Anuncios_ON)
            {
                if(x.Value[0]==id_item)
                {
                    anuncio = x.Value;
                    break;
                }
            }

            if (anuncio != null)
            {
                var carteira = StartBotServices.UserWalletDAL.FundosCarteira(id).GetAwaiter().GetResult();

                if (anuncio.Last() == "Scash" && carteira[1] >= int.Parse(anuncio[3]))
                    return Task.FromResult(true);

                else if (anuncio.Last() == "Jcash" && carteira[0] >= int.Parse(anuncio[3]))
                    return Task.FromResult(true);

                else
                    return Task.FromResult(false);
            }

            else
                return Task.FromResult(false);*/
        }



        public async Task<bool> TemAnuncios()
        {
            if(Anuncios.Count==0)
                return await Task.FromResult(false);
            else
                return await Task.FromResult(true);
        }


        public void ResetaAnuncios()
        {
            try
            {
                Anuncios.Clear();


                MaxAnuncioUser.Clear();

                StartBotServices.AnunciosDAL.RemoverTodosAnuncio();
            }


            catch(System.NullReferenceException ex)
            {
                Console.Clear();

                Console.WriteLine(ex.ToString()+"\n"+ex.TargetSite.ToString());

                
            }
        }

    }
}