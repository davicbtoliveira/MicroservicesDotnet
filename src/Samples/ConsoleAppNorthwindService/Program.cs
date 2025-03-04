using ConsoleAppNorthwindService.Logic.Services;
using ConsoleAppUnitOfWork.Logic;

try
{

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
                    {"Obter registro por ID", SamplesObterAsync.Executar},
                    {"Obter paginado", SampleObterPaginadoAsync.Executar},
                    {"Obter todos os registros", SampleObterTodosAsync.Executar},
                    {"Obter por nome", SampleObterPorNomeAsync.Executar},
                    {"Alterar registro", SampleAlterarAsync.Executar},
                    {"Inserir registro", SampleInserirRegistroAsync.Executar},
                    {"Deletar registro", SampleDeletarAsync.Executar}
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
