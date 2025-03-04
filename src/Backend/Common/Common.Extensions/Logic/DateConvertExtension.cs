namespace Common.Extensions.Logic
{
    public static class DateConvertExtension
    {
        public static DateTime? ConvertStringToDateTime(string dataString)
        {
            try
            {
                if (string.IsNullOrEmpty(dataString))
                    return null;

                var ano = Convert.ToInt32(dataString.Substring(0, 4));
                var mes = Convert.ToInt32(dataString.Substring(4, 2));
                var dia = Convert.ToInt32(dataString.Substring(6, 2));

                return new DateTime(ano, mes, dia);
            }
            catch (Exception)
            {
                try
                {
                    return Convert.ToDateTime(dataString);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
