namespace Common.Extensions.Logic
{
    public static class ExceptionsExtensions
    {
        public static List<string> GetAllExceptions(Exception ex)
        {
            List<string> list = new List<string>();
            list.Add(ex.ToString());
            bool flag = ex.InnerException != null;
            if (flag)
            {
                list.AddRange(GetAllExceptions(ex.InnerException));
            }
            return list;
        }
    }
}
