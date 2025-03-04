namespace Common.Extensions.Logic
{
    public static class DecimalConvertExtension
    {
        public static decimal ConverterStringToDecimal(string valor)
        {
            try
            {
                if (string.IsNullOrEmpty(valor) || valor == "0")
                {
                    return 0.00M;
                }
                else
                {
                    valor = valor.Insert(valor.Length - 2, ",");
                }
            }
            catch (Exception)
            {
            }

            return Convert.ToDecimal(valor);
        }
    }
}
