using ConsoleAppAPINorthwind.Logic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

try
{
    var serviceCollection = new ServiceCollection();

    IConfiguration Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .AddCommandLine(args)
               .Build();

    ConsoleAppAPINorthwind.Configuration.DependencyInjectionConfig.ResolveDependencies(serviceCollection, Configuration);

    var builder = serviceCollection.BuildServiceProvider();


    bool sair = false;
   
    while (!sair)
    {

        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Escolha o menu que deseja acessar:");
        Console.WriteLine();
        Console.ResetColor();
        Console.WriteLine("1 - Menu de Métodos");
        Console.WriteLine("2 - Sair");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nDigite a sua escolha: ");
        Console.ResetColor();

        var opcao = Console.ReadLine();

        Console.Clear();

        switch (opcao)
        {
            case "1":
                Console.WriteLine();
                var central = new CentralConsole(new Dictionary<string, Action>()
                {
                    {"ApiNorthwindObterAsync", SamplesApiNorthwindObterAsync.Executar }
                });

                central.SelecionarExecutar();
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                break;

            case "2":
                sair = true;
                break;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOpção inválida. Pressione qualquer tecla para tentar novamente.");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
                break;
        }
    }

}
catch (Exception ex)
{

    throw;
}





