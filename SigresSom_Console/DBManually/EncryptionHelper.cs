using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;

namespace Logica
{
    /// <summary>
    /// Encriptador
    /// </summary>
    public static class EncryptionHelper
    {
        /// <summary>
        /// Llave para realizar la encriptacion y desencriptacion
        /// </summary>
        private static string value;

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
            //cipherText = "zj1+ENZE5UJmdxfLGiaZQczpP4IRNssXf70KmzFxKmocA8kQmoU3J8MPmI7cxLIgSehIJLVEo2OshN/ZVd33WLUGzr1YChFfzl7sSPxlikXI7SkDuPdjKa7cCiPS+pXLmTMfRFJh3etQalldOYB2OI1xMi0FhsuKE+rBhadFTAPC4k1jMaEjTu/DWZqTtiVEZqeX5qWay1jVvpVewRFFLAj8GoxWccL1BQasOXMz7/mU8iYuIMKu5icEEurnjzzEiUlIcAJbapfDA55SZP8SbXFTnPkrr0UNl9DY6IvA2ahaZ0ALt38LUGul08At4AKjzNv1I7HD4da5iU7uDhrTxzwLSXmMp2MBQxZvLJpX5eo7o8kE2I2Ybsz1WmmnNPW67t0NB5XFZLoEfkUnu3/xmPjzCxfcHWYDG5jqf7S+/NpP7bRUxHHGm/bX/XmPmIyeo+UxqZrYdAcgqSvHep8mUu63ukcudKZw9HnX981daU+hBYo3dyj4TnBpeqbzZwPSpBVaVE+fZsyjWjSCN83cNU4iE2TH+VzlnVwsaoRksUNJqRDhKqo10Lonrfg0dKaGVNmvXDDk1jGKYxYzhzgCWIXLbDlSSkHFPEwqfKMWzbKp2WUzknqJT2W7LM6Eybel5UvEUi6XA5uvsnCZWZcGwM2rodYxnCivXWeyH5Sk6QKDAwzsR+2J80WGizEcPQlrgATJ+EFRbNKxkfpA+VW5zsSfbjGLzRFbOJ3MiJANhN8OaPXT6uYJSZPXRM+9jJA5svQCBMHkELLAW+C4Nar1XGsxh0lxSdZOZkd8XHp/zk1VcDMoeNmwgUnxPgqXIqIu0FXV5lJQ3qDRXlZ/P2IuRY8u1gIybFnC8dM8oSAUqIQdO/fmIAhzuITZMlvZqj10HBTqooRyqHpJKFYvdZjz5KpRiYzPkHOUlfyhPpFIvRGdBRcFe0RjIe10Bu4sirPpzTvOcY5kaYJ3OIQaFTD/bjCOEPO/+A+NEXlkutYjOw8=";
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

        public static string dbConnectionStringb()
        {
            var connectionStringb = ConfigurationManager.AppSettings["DB_CONNECTION_B"];
            return Decrypt(connectionStringb);
            
        }

        public static string dbConnectionStringc()
        {
            var connectionString = ConfigurationManager.AppSettings["DB_CONNECTION_C"];
            return Decrypt(connectionString);
        }

        public static string dbConnectionStringHermes()
        {
            var connectionString = ConfigurationManager.AppSettings["DB_CONNECTION_D"];
            return Decrypt(connectionString);
        }

        //public static string dbConnectionStringb()
        //{
        //    var connectionStringb = ConfigurationManager.AppSettings["DB_CONNECTION_B"];
        //    connectionStringb = "metadata=res://*/ofer_convergente.csdl|res://*/ofer_convergente.ssdl|res://*/ofer_convergente.msl;provider=System.Data.SqlClient;provider connection string='; data source=10.80.3.147\\SQLINSTPROY;initial catalog=E2E_OfertadorConvergente_test;persist security info=True; Integrated Security=True; Timeout=300;MultipleActiveResultSets=True;App=EntityFramework';";
        //    //var d = Decrypt(connectionStringb);
        //    //return Decrypt(connectionStringb);
        //    return connectionStringb;

        //}
        //public static string dbConnectionStringc()
        //{
        //    var connectionStringc = ConfigurationManager.AppSettings["DB_CONNECTION_C"];
        //    var d = Decrypt(connectionStringc);
        //    return Decrypt(connectionStringc);
        //    //return @"metadata=res://*/ofer_convergente.csdl|res://*/ofer_convergente.ssdl|res://*/ofer_convergente.msl;provider=System.Data.SqlClient;provider connection string=';   data source=10.80.3.147\SQLINSTPROY;initial catalog=E2E_OfertadorConvergente_test;persist security info=True;user id=E2E_OfertadorConvergente;password=7SrD2TA$;MultipleActiveResultSets=True;App=EntityFramework';";
        //}
    }
}
