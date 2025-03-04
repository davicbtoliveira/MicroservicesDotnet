using System.ComponentModel;

namespace Common.Extensions.Logic
{
    public static class EnumDescriptionExtension
    {
        public static string GetDescription<T>(this T? enumerador) where T : struct, Enum
        {
            if (enumerador == null)
                return "";

            var fi = enumerador.GetType().GetField(enumerador.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return enumerador.ToString();
        }
    }
}
