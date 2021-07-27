
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using ProjetoTI3.DomainModels;
using ProjetoTI3.Models;

namespace ProjetoTI3
{
    public class Program
    {
        public static string _conexao = "";
        public static string _uidDefault = "00000000-0000-0000-0000-000000000000";
        public static string _passwordDefault = "Password.01";

        public static Colaborador getSessionColaborador(string uid)
        {
            Colaborador_Helper ch = new Colaborador_Helper(_conexao);
            return ch.getConta(uid);
        }

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
