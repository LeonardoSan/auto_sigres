using DBManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigresSom_Console.Class
{
    public class Log
    {
        public string cadena = "";
        string entorno = ConfigurationManager.AppSettings["Entorno"].ToString();
        Dictionary<string, string> dicParameter = new Dictionary<string, string>();

        public Log()
        {
            if (entorno == "0")
                cadena = "DEV";
            else if (entorno == "1")
                cadena = "TEST";
            else if (entorno == "2")
                cadena = "PROD";
            else cadena = "DEV";
        }

        public void insertLog(string id, int estado, string descripcion = "")
        {

            autosgrsm_Log logger = new autosgrsm_Log();
            logger.cadena = EncryptionManager.Decrypt(ConfigurationManager.ConnectionStrings["DB_CONNECTION_" + cadena].ToString());
            logger.id_som = id;
            logger.fecha = DateTime.Now;
            logger.estado = estado;            
            logger.descripcion = descripcion;
            logger.insert();

            Console.WriteLine("Se agregó al log general: " + descripcion);
        }

        public void insertLogGeneral(int estado, string descripcion)
        {
            autosgrsm_Log logger = new autosgrsm_Log();
            logger.cadena = EncryptionManager.Decrypt(ConfigurationManager.ConnectionStrings["DB_CONNECTION_" + cadena].ToString());
            logger.fecha = DateTime.Now;
            logger.estado = estado;
            logger.descripcion = descripcion;
            logger.insert();

            Console.WriteLine("Se agregó al log general: " + descripcion);
        }
    }
}
