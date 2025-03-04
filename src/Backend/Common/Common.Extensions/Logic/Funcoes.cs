using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Extensions.Logic
{
    public class Funcoes
    {
        public static string PrimeirasPalavras(String frase, Int32 quantidade)
        {
            if (!string.IsNullOrEmpty(frase))
            {
                int palavras = quantidade;

                for (int i = 0; i < frase.Length; i++)
                {
                    if (frase[i] == ' ')
                        palavras--;

                    if (palavras == 0)
                        return frase.Substring(0, i);
                }
            }

            return string.Empty;
        }

        public static string RemoverMascara(string texto)
        {
            return texto == null ? string.Empty : Regex.Replace(texto, "[?\\)?\\(_./-]", "");
        }

        public static string RetornarApenasNumeros(string toNormalize)
        {
            if (toNormalize == null)
                return string.Empty;

            var numbers = new List<char>("0123456789");
            var toReturn = new StringBuilder(toNormalize.Length);
            var enumerator = toNormalize.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (numbers.Contains(enumerator.Current))
                    toReturn.Append(enumerator.Current);
            }

            return toReturn.ToString();
        }

        public static bool ValidarCpf(string sNumero)
        {
            try
            {
                string tempCpfCnpj;
                string digito;
                int soma;
                int resto;
                int[] multiplicador1;
                int[] multiplicador2;

                sNumero = RetornarApenasNumeros(sNumero);

                multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                sNumero = sNumero.PadLeft(14, '0');

                if (sNumero.Substring(3, 11).Equals("00000000000") || sNumero.Substring(3, 11).Equals("11111111111") ||
                        sNumero.Substring(3, 11).Equals("22222222222") || sNumero.Substring(3, 11).Equals("33333333333") ||
                        sNumero.Substring(3, 11).Equals("44444444444") || sNumero.Substring(3, 11).Equals("55555555555") ||
                        sNumero.Substring(3, 11).Equals("66666666666") || sNumero.Substring(3, 11).Equals("77777777777") ||
                        sNumero.Substring(3, 11).Equals("88888888888") || sNumero.Substring(3, 11).Equals("99999999999"))
                    return false;


                tempCpfCnpj = sNumero.Substring(3, 9);
                soma = 0;

                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpfCnpj[i].ToString()) * multiplicador1[i];

                resto = soma % 11;

                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();
                tempCpfCnpj = tempCpfCnpj + digito;
                soma = 0;

                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpfCnpj[i].ToString()) * multiplicador2[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();

                return sNumero.EndsWith(digito);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ValidarCnpj(string sNumero)
        {
            try
            {
                string tempCpfCnpj;
                string digito;
                int soma;
                int resto;
                int[] multiplicador1;
                int[] multiplicador2;

                sNumero = RetornarApenasNumeros(sNumero);

                multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

                if (sNumero.Length != 14)
                    return false;

                if (sNumero.Substring(0, 8).Equals("00000000") || sNumero.Substring(0, 8).Equals("11111111") ||
                    sNumero.Substring(0, 8).Equals("22222222") || sNumero.Substring(0, 8).Equals("33333333") ||
                    sNumero.Substring(0, 8).Equals("44444444") || sNumero.Substring(0, 8).Equals("55555555") ||
                    sNumero.Substring(0, 8).Equals("66666666") || sNumero.Substring(0, 8).Equals("77777777") ||
                    sNumero.Substring(0, 8).Equals("88888888") || sNumero.Substring(0, 8).Equals("99999999"))
                    return false;

                tempCpfCnpj = sNumero.Substring(0, 12);
                soma = 0;

                for (int i = 0; i < 12; i++)
                    soma += int.Parse(tempCpfCnpj[i].ToString()) * multiplicador1[i];

                resto = (soma % 11);

                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();
                tempCpfCnpj = tempCpfCnpj + digito;

                soma = 0;
                for (int i = 0; i < 13; i++)
                    soma += int.Parse(tempCpfCnpj[i].ToString()) * multiplicador2[i];

                resto = (soma % 11);

                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();

                return sNumero.EndsWith(digito);

            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ValidarCpfCnpj(string sNumero)
        {
            try
            {
                if (Funcoes.RemoverMascara(sNumero).Trim().Length <= 11)
                    return ValidarCpf(sNumero);
                else
                    return ValidarCnpj(sNumero);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ValidarEmail(string email)
        {
            string regex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            if (email != null && email != "")
                return Regex.IsMatch(email, regex);
            else
                return false;
        }

        public static bool ValidarTelefone(string numero)
        {
            if (!string.IsNullOrEmpty(numero) && numero.Length >= 8)
            {
                var regex = @"^1\d\d(\d\d)?$|^0800 ?\d{3} ?\d{4}$|^(\(0?([1-9a-zA-Z][0-9a-zA-Z])?[1-9]\d\) ?|0?([1-9a-zA-Z][0-9a-zA-Z])?[1-9]\d[ .-]?)?(9|9[ .-])?[2-9]\d{3}[ .-]?\d{4}$";
                var rgx = new Regex(regex);

                if (rgx.IsMatch(numero) == true)
                {
                    var regexsequencial = @"(?!(\d)\1{8})\d{9}";

                    var rgxseq = new Regex(regexsequencial);
                    if (rgxseq.IsMatch(numero) == true)
                        return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }


        public static Object PreencherDadosComuns(Object obj_origem, Object obj_destino, String excecoes = null, String campos_ignorados = null)
        {
            if (obj_origem == null)
                return null;

            Type[] types = new Type[] { typeof(string), typeof(int), typeof(int?), typeof(double), typeof(double?),
                typeof(long), typeof(long?), typeof(char), typeof(char?), typeof(DateTime), typeof(DateTime?),
                typeof(bool), typeof(bool?), typeof(float), typeof(float?), typeof(decimal), typeof(decimal?), typeof(short), typeof(short?), typeof(byte[]) };

            IEnumerable<String> propriedades_comuns = obj_origem.GetType().GetMethods()

                .Where(method => method.Name.StartsWith("get_") &&
                    (String.IsNullOrEmpty(excecoes) || method.Name.StartsWith(excecoes) == false) &&
                    types.Contains(method.ReturnType))

                .Select(method => method.Name.Replace("get_", ""))

                .Intersect(obj_destino.GetType().GetMethods().Select(method => method.Name.Replace("get_", "")));

            foreach (String propriedade in propriedades_comuns)
            {
                if (!String.IsNullOrEmpty(campos_ignorados))
                {
                    bool continuar = false;
                    string[] campos = campos_ignorados.Split(',');

                    foreach (String campo in campos)
                    {
                        if (campo == propriedade)
                        {
                            continuar = true;
                            break;
                        }
                    }

                    if (continuar == true)
                        continue;
                }

                String getter = "get_" + propriedade;
                String setter = "set_" + propriedade;

                MethodInfo mi_origem = GetMethodInfo(obj_origem, getter);
                MethodInfo mi_destino = GetMethodInfo(obj_destino, setter);

                try
                {
                    if (mi_origem != null && mi_destino != null)
                    {
                        mi_destino.Invoke(obj_destino, new Object[1] { mi_origem.Invoke(obj_origem, null) });
                    }
                }

                catch (Exception)
                {
                }
            }

            return obj_destino;
        }

        public static MethodInfo GetMethodInfo(object entidade, string nomeMeth)
        {
            try
            {
                return entidade.GetType().GetMethod(nomeMeth);
            }
            catch (Exception)
            { }

            return null;
        }


        /// <summary>
        /// Retorna quantos erros há na ModelState (porque sempre haverá chaves)
        /// </summary>
        /// <param name="mState">ModelState do Controller</param>
        /// <returns></returns>
        public static string descriptografar(string textoCriptografado)
        {
            try
            {
                string textoDescriptografado = descriptografarString(textoCriptografado);
                string[] allKeys = textoDescriptografado.ToString().Split('&');
                foreach (string mKeys in allKeys)
                {
                    if (!string.IsNullOrEmpty(mKeys))
                    {
                        string[] key = mKeys.Split('=');
                        if (key[0] == "IP")
                        {
                            return key[1].ToString();
                            break;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }


        public static string descriptografarLoginAD(string textoCriptografado)
        {
            try
            {
                string textoDescriptografado = descriptografarString(textoCriptografado);
                string[] allKeys = textoDescriptografado.ToString().Split('&');

                var result = new StringBuilder();
                foreach (var item in allKeys)
                {
                    if (!item.StartsWith("MOD") && !item.StartsWith("IP") && !item.StartsWith("NIV") && !item.StartsWith("DTHR"))
                    {
                        result.Append(item);
                    }
                }
                return result.ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }


        public static string descriptografarString(string Texto)
        {
            try
            {
                Byte[] b = Convert.FromBase64String(Texto);
                string dc = ASCIIEncoding.ASCII.GetString(b);
                return dc;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static String GetHash(String text, String key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }


    }
}
