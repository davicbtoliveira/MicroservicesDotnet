namespace ConsoleAppUnitOfWork.Logic
{
    public class CentralConsole
{

    Dictionary<string, Action> Controle;

    public CentralConsole(Dictionary<string, Action> controle)
    {
        Controle = controle;
    }

    public void SelecionarExecutar()
    {
        Console.Clear();
        int i = 1;

        foreach (var controle in Controle)
        {
            Console.WriteLine("{0}) {1}", i, controle.Key);
            i++;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nDigite o número do método (ou vazio para o último): ");
        Console.ResetColor();


        int.TryParse(Console.ReadLine(), out int num);
        bool numValido = num > 0 && num <= Controle.Count;
        num = numValido ? num - 1 : Controle.Count - 1;
        string nomeDoMetodo = Controle.ElementAt(num).Key;
        Thread.Sleep(1000);
        Console.Clear();
        Console.WriteLine("\x1b[3J");


        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Executando método ");
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(nomeDoMetodo);
        Console.ResetColor();
        Thread.Sleep(1000);
        Console.WriteLine();

        Action executar = Controle.ElementAt(num).Value;

        try
        {
            executar();
        }

        catch (Exception e)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Ocorreu um erro: {0}", e.Message);
            Console.ResetColor();
            Console.WriteLine(e.StackTrace);
        }

        Console.Clear();
        Console.WriteLine("\x1b[3J");
    }
}
}
