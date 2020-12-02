using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;

namespace DBManager
{
    /// <summary>
    /// Encriptador
    /// </summary>
    public static class EncryptionManager
    { 

        public static byte[] randomKey()
        {
            var x = ConfigurationManager.AppSettings["ENCRYPTION_BYTE"];
            byte[] key = x.Split(new[] { ',' }).Select(s => Convert.ToByte(s, 16)).ToArray();
            return key;
        }

        /// <summary>
        /// Encriptar
        /// </summary>
        /// <param name="clearText">Mensaje a Encriptar</param>
        /// <returns></returns>
        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                var ENCRYPTION_KEY = ConfigurationManager.AppSettings["ENCRYPTION_KEY"];
                byte[] key = randomKey();
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ENCRYPTION_KEY, key);
                encryptor.Key = pdb.GetBytes(32); // 32 * 8
                encryptor.IV = pdb.GetBytes(16); // 16 * 8
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        /// <summary>
        /// Desencriptar
        /// </summary>
        /// <param name="cipherText">Mensaje a descriptar</param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {

            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                var ENCRYPTION_KEY = ConfigurationManager.AppSettings["ENCRYPTION_KEY"];
                byte[] key = randomKey();
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ENCRYPTION_KEY, key);
                encryptor.Key = pdb.GetBytes(32); // 32 * 8
                encryptor.IV = pdb.GetBytes(16); // 16 * 8
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public static string test(string text)
        { 
            return Decrypt(Encrypt(text));
        }
        public static string dbConnectionString()
        {
            var connectionString = ConfigurationManager.AppSettings["DB_CONNECTION"];
            return Decrypt(connectionString);
        }
    }
}
