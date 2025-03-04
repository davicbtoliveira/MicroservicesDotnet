using Northwind.Data.Northwind.Entity;

namespace ConsoleAppUnitOfWork.Logic
{
    public class Util
    {

        public static void CategoriaToString(Categories categories)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================");
            Console.WriteLine("CategoryID : " + categories.CategoryID);
            Console.WriteLine("CategoryName : " + categories.CategoryName);
            Console.WriteLine("Description : " + categories.Description);
        }

    }
}
