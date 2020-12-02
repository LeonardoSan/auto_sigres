using DBManager;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;
using Oracle.ManagedDataAccess.Client;

namespace api_consulta_sigres_som.Controllers
{

    public class oSOM
    {
        public string Codigo { get; set; }
        public string Estado { get; set; }
    }

    [RoutePrefix("api/som")]
    public class SOMController : ApiController
    {
        public string cadena = "";

        string entorno = ConfigurationManager.AppSettings["Entorno"].ToString();
        Dictionary<string, string> dicParameter = new Dictionary<string, string>();

        public SOMController()
        {
            if (entorno == "0")
                cadena = "DEV";
            else if (entorno == "1")
                cadena = "TEST";
            else if (entorno == "2")
                cadena = "PROD";
            else cadena = "DEV";
        }

        [HttpGet]
        [Route("index")]
        public string index()
        {
            return "Bienvenido a la Api de SOM";
        }

        [HttpGet]
        [Route("consulta")]
        public oSOM consulta()
        {
            autosgrsm_Parametros parametros = new autosgrsm_Parametros();
            //string a = EncryptionManager.Encrypt(@"Server=10.80.3.147\SQLINSTPROY;Database=E2E_Salvalineas_base_dev;User Id=E2E_Salvalineas_base;Password=7SrD2TA$;");
            parametros.cadena = EncryptionManager.Decrypt(ConfigurationManager.ConnectionStrings["DB_CONNECTION_" + cadena].ToString());
            parametros.querySelect = "WHERE Nombre_Parametro = @Nombre_Parametro";
            dicParameter = new Dictionary<string, string>();
            dicParameter.Add("@Nombre_Parametro", "SQL_SELECT_SOM");
            DataTable tbParametros = parametros.select(dicParameter).Tables[0];
            string consultaSOM = tbParametros.Rows[0]["Valor_Parametro"].ToString();


            //string oradb = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = 10.203.109.84)(PORT = 1526))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = somdb)));User Id=SQL_AUTOMAT;Password=Kiweth3ns4sx;Connection Timeout=600;";
            //string oradb = oradb = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = 10.203.109.84)(PORT = 1526))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = somdb)));User Id=GMOVISTAR;Password=Tele2020*!Gvist#; Connection Timeout=600;";
            //string oradb = oradb = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = 10.203.109.84)(PORT = 1526))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = somdb)));User Id=SQL_LSANABRIAHO;Password=Tele2020*!Nov#; Connection Timeout=600;";
            //string encriptado = EncryptionManager.Encrypt(oradb);

            string db_som = EncryptionManager.Decrypt(ConfigurationManager.ConnectionStrings["DB_CONNECTION_SOM_" + cadena].ToString());
            DataTable query = new DataTable();

            using (OracleConnection conn = new OracleConnection(db_som))
            { 
                OracleCommand cmd = new OracleCommand();
                //  string Query = "SELECT COUNT(*) FROM  SOMDB.T_BPM_SERVICEIM_CPE_ACCOUNTID B WHERE B.FATHER_ACCOUNT IS NOT NULL";

                cmd.CommandText = consultaSOM;
                cmd.Connection = conn;
                conn.Open();
                OracleDataReader queryOracle = cmd.ExecuteReader();

                query.Load(queryOracle);
                conn.Close();
            }
            string a = "a";

            oSOM som = new oSOM()
            {
                Codigo = "00000000",
                Estado = "EN CREACION"
            };


            return som;
        }
    }
}
