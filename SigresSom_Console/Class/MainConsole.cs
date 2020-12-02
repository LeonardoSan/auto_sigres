using DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SigresSom_Console.Class
{
    public class MainConsole
    {

        public string cadena = "";
        [Obsolete]
        string entorno = System.Configuration.ConfigurationSettings.AppSettings["Entorno"].ToString();
        Dictionary<string, string> dicParameter = new Dictionary<string, string>();

        public MainConsole()
        {
            if (entorno == "0")
                cadena = "DEV";
            else if (entorno == "1")
                cadena = "TEST";
            else if (entorno == "2")
                cadena = "PROD";
            else cadena = "DEV";
        }

        public void Main()
        {
            
            autosgrsm_Parametros parametros = new autosgrsm_Parametros();
            var connection = System.Configuration.ConfigurationSettings.AppSettings["DB_CONNECTION_" + cadena].ToString();
            parametros.cadena = EncryptionManager.Decrypt(connection);
            parametros.querySelect = "WHERE Nombre_Parametro = @Nombre_Parametro";
            dicParameter = new Dictionary<string, string>();
            dicParameter.Add("@Nombre_Parametro", "API_LIST_SOM");
            DataTable tbParametros = parametros.select(dicParameter).Tables[0];
            string consultaApi = tbParametros.Rows[0]["Valor_Parametro"].ToString();
            List<autosgrsm_SOM> jsonSOM = null;

            var request = (HttpWebRequest)WebRequest.Create(consultaApi);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string json = objReader.ReadToEnd();
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            jsonSOM = js.Deserialize<List<autosgrsm_SOM>>(json);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle error

            }

            if(jsonSOM.Count > 0)
            {

                foreach (autosgrsm_SOM item in jsonSOM)
                {
                    item.cadena = EncryptionManager.Decrypt(connection);
                    item.CREATED = DateTime.Now;
                    item.STATUS_FLOW = "CARGUE INICIAL";
                    item.insert();
                }
            }
            else
            {
                Console.WriteLine("La consulta SOM no trajo resultados");
            }

            //Consultar lineas con CARGUE INICIAL


            Console.WriteLine(consultaApi);
            Console.ReadLine();

        }
    }
}
