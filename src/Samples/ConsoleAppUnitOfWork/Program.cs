using ConsoleAppUnitOfWork.Logic;
using ConsoleAppUnitOfWork.Logic.Repository;

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
                    {"GetAll", SampleGetAll.Executar},
                    {"GetAllPredicateDisableTrackingOrderBy", SampleGetAllPredicateDisableTrackingOrderBy.Executar},
                    {"GetAllSelector", SampleGetAllSelector.Executar},
                    {"GetAllFilter", SampleGetAllFilter.Executar},
                    {"GetFirstOrDefault", SampleGetFirstOrDefault.Executar},
                    {"GetFirstOrDefaultFilter", SampleGetFirstOrDefaultFilter.Executar},
                    {"GetFirstOrDefaultSelector", SampleGetFirstOrDefaultSelector.Executar},
                    {"GetPagedList", SampleGetPagedList.Executar},
                    {"GetPagedListFilter", SampleGetPagedListFilter.Executar},
                    {"Exists", SampleExists.Executar},
                    {"Find", SampleFind.Executar},
                    {"FromSql", SampleFromSql.Executar},
                    {"Count", SampleCount.Executar},
                    {"LongCount", SampleLongCount.Executar},
                    {"Average", SampleAverage.Executar},
                    {"Max", SampleMax.Executar},
                    {"Min", SampleMin.Executar},
                    {"Sum", SampleSum.Executar},
                    {"Update", SampleUpdate.Executar},
                    {"Insert", SampleInsert.Executar},

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
