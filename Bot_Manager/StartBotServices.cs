
#region Diretivas
using Bot_Manager.ComandosTexto;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.IO;
using System.Data.SQLite;
using Bot_Manager.Quests;
using Bot_Manager.Logs_e_Coleta_de_Informacoes;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot_Manager.Domains;
using Bot_Manager.Logs_e_Eventos;
using Bot_Manager.Domains.Operacoes_da_Loja;
using Bot_Manager.Domains.Guilda_e_User;
using Bot_Manager.Domains.Operacoes_da_Loja.DbOperations;
using Bot_Manager.Domains.QuestsOp;
using Bot_Manager.Quests.Diarias;
using Bot_Manager.Quests.Drops;
using Bot_Manager.ComandosBarra;
using Bot_Manager.ultilitarios;
using Bot_Manager.Quests_andGames.Games;
using DSharpPlus.SlashCommands;

#endregion

namespace Bot_Manager
{
    public class StartBotServices
    {

        private readonly ILogInformations _logInformations;


        //private static readonly Lazy<StartBotServices>
        //    lazy = new Lazy<StartBotServices>(() => new StartBotServices());

        //public static StartBotServices Instance { get { return lazy.Value; } }

        //..
        //Propriedades publiscos para uso
        public static DiscordClient Client;

        public static readonly string ConnString = "Data Source="+@Environment.CurrentDirectory + @"//DbBot.db";
        public static readonly ulong CanalExceptions = 1025977757324296212;


        #region Prop publicas
        public static ISaveAccGuildDADOS SaveInfo { get; set; }
        public static IGuildDAL GuildDAL { get; set; }
        public static Resposta_Eventos Resposta_Eventos;
        public static IUserDAL UserDAL { get; set; }
        public static ItensDAL ItensDAL { get; set; }
        public static SaveEconomicOP SaveEconomicOP { get; set; }
        public static UserWalletDAL UserWalletDAL { get; set; }
        public static ItensValue ItensValue { get; set; }
        public static VendasDAL VendasDAL { get; set; }
        public static Itens_Loja Itens_Loja { get; set; }
        public static Invite Invite { get; private set; }
        public static Loja Loja { get; private set; }
        public static OpCashQuestsDAL OpCashQuestsDAL { get; private set; }
        public static Cota_Diaria Diarias { get; private set; }
        public static Drops Drops { get; private set; }
        public static Roleta Roleta { get; private set; }
        public static ComercioUsuarios ComercioUsuarios { get; private set; }
        public static AnunciosDAL AnunciosDAL { get; private set; }

        #endregion

        //Atributos
        public static Dictionary<string, string?> ChannelLog;

        public static List<string> Users;




        //.
        //configura A conexão e inicia a classe em geral
        public StartBotServices()

        {
            var a = Environment.CurrentDirectory;
            _logInformations = new LogInformations() ??
                throw new NullReferenceException(nameof(LogInformations));

            Client = new DiscordClient(new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                AutoReconnect = true,
                Token = "ODk2MjA4NjU0NjcwMzc3MDAz.GPy7p-.JaQtRc22Svb40o4yF83tZPX687Og5VfTbIk86s",
                TokenType = TokenType.Bot
            });

            var ss = @Environment.CurrentDirectory + "DbBot.db";

            if (!File.Exists(Environment.CurrentDirectory + @"//DbBot.db"))
                CriarBanco();

            InstanciarDALobj();
            InstanciarObj();
            Start().GetAwaiter().GetResult();
            
        }



        //Inicia a conexão com o discord e registra comandos para uso
        private async Task Start()
        {
            #region config client

            /*var service = new ServiceCollection()
            .AddTransient<ComandosPunicao>()
            .AddTransient<ComandosReg>()
            .BuildServiceProvider();*/



            var commands = Client.UseCommandsNext(new CommandsNextConfiguration()
            {
                //Services = service,
                StringPrefixes = new[] { "!j", "!J", "!j " }
            });

            //var slash = Client.UseSlashCommands();

            try
            {
                commands.RegisterCommands<ComandosReg>();
                commands.RegisterCommands<ComandosLoja>();
                commands.RegisterCommands<ComandosPunicao>();
                commands.RegisterCommands<ComandosQuests>();
                commands.RegisterCommands<ComandosPaulo>();
                commands.RegisterCommands<ComandosGames>();
                commands.RegisterCommands<ComandosInfo>();

               //slash.RegisterCommands<ComandosBarra1>();


                commands.SetHelpFormatter<CustomHelp>();
            }

            catch (Exception) { };

            #endregion

            Task.WaitAny(Client.ConnectAsync());

            Console.WriteLine("\n \n\tBot Conectado em " + 
                Client.GatewayInfo.Url +"\n Dir. Banco de dados: "+ Environment.CurrentDirectory);

            InitializeLogServices();

            await Task.Delay(-1);
        }


        private void InitializeLogServices()
        {
            Resposta_Eventos = new Resposta_Eventos(Client);
            OpMessages.Client = Client;
            _logInformations.GetMessageEvents(Client);
        }


        private void InstanciarDALobj()
        {
            GuildDAL = new GuildDAL();
            UserDAL = new UserDAL();
            ItensDAL = new ItensDAL();
            UserWalletDAL = new UserWalletDAL();
            VendasDAL = new VendasDAL();
            OpCashQuestsDAL = new OpCashQuestsDAL();
            AnunciosDAL = new AnunciosDAL();

        }



        private void InstanciarObj()
        {
            //valor em J e Scash dos nitrosC e I
            int[] a = { 10, 8000};
            int[] b = { 5, 4800};

            try
            {

                SaveInfo = new SaveAccGuildDADOS();
                SaveEconomicOP = new SaveEconomicOP();
                ItensValue = new ItensValue(a, b);
                Loja = new Loja();
                Itens_Loja = new Itens_Loja();
                Invite = new Invite();
                Diarias = new Cota_Diaria(300);
                Drops = new Drops();
                Roleta = new Roleta();
                ComercioUsuarios = new ComercioUsuarios();

                new MainTimer();

                ChannelLog = GuildDAL.GetAllGuildsChannel().GetAwaiter().GetResult();
                Users = UserDAL.GetAllUsers().GetAwaiter().GetResult();
            }


            catch(NullReferenceException ex)
            {
                Console.WriteLine(ex.Message + ex.Source);
            }
        }


        public void CriarBanco()
        {
            SQLiteConnection.CreateFile("DbBot.db");

            SQLiteConnection dbCon = new SQLiteConnection(@"Data Source=DbBot.db;Version=3");
            dbCon.Open();

            SQLiteCommand cmd = new SQLiteCommand(Test.SQL_TB_AN, dbCon);
            cmd.ExecuteNonQuery();


            SQLiteCommand cmd01 = new SQLiteCommand(Test.SQL_TB_USER, dbCon);
            cmd01.ExecuteNonQuery();


            SQLiteCommand cmd02 = new SQLiteCommand(Test.SQL_TB_GUILD, dbCon);
            cmd02.ExecuteNonQuery();


            SQLiteCommand cmd03 = new SQLiteCommand(Test.SQL_TB_CNITRO, dbCon);
            cmd03.ExecuteNonQuery();


            SQLiteCommand cmd04 = new SQLiteCommand(Test.SQL_TB_INITRO, dbCon);
            cmd04.ExecuteNonQuery();


            SQLiteCommand cmd05 = new SQLiteCommand(Test.SQL_TB_IVAR, dbCon);
            cmd05.ExecuteNonQuery();


            SQLiteCommand cmd06 = new SQLiteCommand(Test.SQL_TB_VENDASOK, dbCon);
            cmd06.ExecuteNonQuery();


            dbCon.Close();
        }
    }
}