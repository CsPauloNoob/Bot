using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using Bot_Manager.Models;
using Bot_Manager.Quests.Drops;

namespace Bot_Manager.ComandosTexto
{
    internal class ComandosPaulo :BaseCommandModule
    {

        [Command("dropar")]

        async Task Drop(CommandContext ctx, string nome, string valor)
        {

            int _valor = 0;
            if (ctx.Member.Id == 751499220149731411)
            {
                var drop = new Drops()
                {
                    Nome = nome,
                    Valor = valor,
                    Ativo = true,
                    dropCredito = false
                };


                if(int.TryParse(valor, out _valor))
                    drop.dropCredito = true;

                StartBotServices.Loja.drop_Loja = drop;
                await ctx.RespondAsync("Dropado");
            }
        }


        [Command("setd")]

        async Task SetarDinheiro(CommandContext ctx, ulong id ,string tipo, short valor, string key)
        {
            if (ctx.Member.Id == 751499220149731411 && key == Environment.GetEnvironmentVariable("KEY_JEY_1411"))
            {
                if (StartBotServices.SaveEconomicOP.AdcionarSaldo(id, valor, tipo).Result)
                    await ctx.RespondAsync("Setado");
                else
                    await ctx.RespondAsync("Erro");
            }

            await ctx.Message.DeleteAsync();

        }



        [Command("debd")]

        async Task DebitarDinheiro(CommandContext ctx, ulong id, string tipo, short valor, string key)
        {
            if (ctx.Member.Id == 751499220149731411 && key == Environment.GetEnvironmentVariable("KEY_JEY_1411")) 
            {
                if (StartBotServices.SaveEconomicOP.DebitarSaldo(id, valor, tipo).Result)
                    await ctx.RespondAsync("Debitado");
                else
                    await ctx.RespondAsync("Não é possível tirar esse valor");
            }
        }


        [Command("consultvenda")]

        async Task ConsultarVenda(CommandContext ctx, ulong id)
        {
            if(ctx.Member.Id == 751499220149731411)
            {
                List<string[]> consulta = StartBotServices.VendasDAL.ConsultarVenda(id).GetAwaiter().GetResult();
                string linhas = "";
                foreach(var x in consulta)
                {
                    linhas += " ```";

                    for (var i = 0; i <= 5; i++)
                        linhas += " "+x[i]; 
                    
                    linhas += "```\n";
                }

                await ctx.RespondAsync(linhas);

            }
        }


        [Command("dbconfig")]

        async Task AlterarBanco(CommandContext ctx, params string[] com)
        {
            string sql = "";

            foreach(var str in com)
            {
                sql += " "+str;
            }

            if (ctx.User.Id == 751499220149731411)
                if (StartBotServices.DbConfig.RealizarAltTable(sql).Result)
                    await ctx.RespondAsync("Ação Concuida!!");
        }


        [Command("vercart")]

        async Task VerSaldoCarteira(CommandContext ctx, ulong id)
        {
            if(ctx.User.Id== 751499220149731411)
            {
                var values = StartBotServices.SaveEconomicOP.RetornaCarteira(id).GetAwaiter().GetResult();

                await ctx.Client.SendMessageAsync(ctx.Client.GetChannelAsync(ctx.Channel.Id).Result,
                    EmbedMesages.CarteiraView(ctx.Client.GetUserAsync(id).Result.Username, values));
            }
        }



        [Command("vroleta")]

        async Task SetarPremioRoleta(CommandContext ctx, int valor)
        {
            if(ctx.User.Id== 751499220149731411)
            {
                StartBotServices.Roleta.ValorPremio = valor;
            }
        }


        //Setar em tempo de execução o valor dos nitros

        #region Set Valores itens

        [Command("vinitro")]

        async Task VInitro(CommandContext ctx, int Jvalue, int Svalue)
        {

            int[] valor = { Jvalue, Svalue };

            if (ctx.Member.Id == 751499220149731411)
            {
                StartBotServices.ItensValue.valueInactiveNitro = valor;

                await ctx.RespondAsync("Valor setado");
            }
        }


        [Command("vcnitro")]

        async Task VCnitro(CommandContext ctx, int Jvalue, int Svalue)
        {

            int[] valor = { Jvalue, Svalue };

            if (ctx.Member.Id == 751499220149731411)
            {
                StartBotServices.ItensValue.valueClassicNitro = valor;

                await ctx.RespondAsync("Valor Setado");
            }

        }


        [Command("vinitro")]

        async Task VInitro(CommandContext ctx)
        {

            if (ctx.Member.Id == 751499220149731411)
            {
                await ctx.RespondAsync("!jinitro valor Jcash, Valor Scash\n\n" +
                    "O mesmo se aplica ao outro comando!");
            }
        }



        #endregion

        
        [Command("initro")]

        async Task NovoInitro(CommandContext ctx, string item)
        {

            if (ctx.Member.Id == 751499220149731411)
            {
                if (StartBotServices.ItensDAL.AddInactiveNitro(item))
                {
                    await StartBotServices.Itens_Loja.AdcionarInitro(item);
                    await ctx.RespondAsync("Inserido com sucesso");
                }

                else
                    await ctx.RespondAsync("Não foi possivel seguir com a operação");
            }

        }



        [Command("cnitro")]

        async Task NovoCnitro(CommandContext ctx, string item)
        {

            if (ctx.Member.Id == 751499220149731411)
            {
                if (StartBotServices.ItensDAL.AddClassicNitro(item))
                {
                    await StartBotServices.Itens_Loja.AdcionarCnitro(item);
                    await ctx.RespondAsync("Inserido com sucesso");
                }
                else
                    await ctx.RespondAsync("Não foi possivel seguir com a operação");
            }

        }


        //Seta novo item generico na loja
        [Command("nitem")]

        async Task NovoItemLoja(CommandContext ctx, string nome_Item, string item, int valorJcash, int valorScash)
        {
            _ = Task.Run(async () =>
            {
                if (ctx.Member.Id == 751499220149731411)
                {
                    object id;
                    if (Convert.ToInt32(StartBotServices.Itens_Loja.Variados.Count) > 0)
                    {
                        id = StartBotServices.Itens_Loja.Variados.Last().Id;
                    }

                    else
                    {
                        id = 3;
                    }

                    var parcial = 1 + int.Parse(id.ToString());

                    id = parcial.ToString();

                    nome_Item = nome_Item.Replace("-", " ");

                    if (StartBotServices.Itens_Loja.Adcionaritem(new ItemVariado(id.ToString(), nome_Item, item, valorJcash, valorScash))
                        .GetAwaiter().GetResult())
                    {

                        StartBotServices.ItensValue.ValorItensV.Add(id.ToString(), new int[] { valorJcash, valorScash });


                        await StartBotServices.ItensDAL.NovoItem(nome_Item, item, valorJcash.ToString(), valorScash.ToString());

                        await ctx.RespondAsync("Inserido com sucesso");
                    }

                    else
                        await ctx.RespondAsync("Não foi possivel seguir com a operação, revise a ordem dos argumentos" +
                            " ou se algo não está repetido");

                }
            });
        }



        [Command("nitem")]

        async Task NovoItemLoja(CommandContext ctx)
        {
                if (ctx.Member.Id == 751499220149731411)
                await ctx.RespondAsync("Nome do item, Item, Valor em Jcash, valor em Scash");
        }


        [Command("Stt")]

        async Task AtualizarStatus(CommandContext ctx, string texto)
        {

            texto = texto.Replace("-", " ");

            try
            {
                await ctx.Client.UpdateStatusAsync(new DiscordActivity(texto,
                    ActivityType.Playing), UserStatus.Online, DateTime.Now);
            }

            catch (Exception ex)
            {
                await ctx.RespondAsync($"Ocorreu o seguinte erro:\n ```{ex.Message} ```");
            }
        }



        [Command("rcon")]

        async Task RestartApp(CommandContext ctx)
        {
            if (ctx.Member.Id == 751499220149731411)
                await ctx.Client.ReconnectAsync();
            StartBotServices.Itens_Loja.RestartItens();
        }

    }
}