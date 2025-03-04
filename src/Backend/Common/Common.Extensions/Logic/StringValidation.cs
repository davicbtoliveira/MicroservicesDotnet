namespace Common.Extensions.Logic
{
    public static class StringValidation
    {
        static string typestring => "string";
        public static bool ValidString(string value)
        {
            if (value == typestring) return false;
            return true;
        }

    }
}
