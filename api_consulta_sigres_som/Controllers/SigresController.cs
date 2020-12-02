using DBManager;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api_consulta_sigres_som.Controllers
{
    [RoutePrefix("api/sigres")]
    public class SigresController : ApiController
    {
        public string cadena = "";

        string entorno = ConfigurationManager.AppSettings["Entorno"].ToString();
        Dictionary<string, string> dicParameter = new Dictionary<string, string>();

        public SigresController()
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
        [Route("consulta/{cai}")]
        public autosgrsm_SIGRES consulta(string CAI)
        {
            autosgrsm_SIGRES objetoSigres = new autosgrsm_SIGRES();

            autosgrsm_Parametros parametros = new autosgrsm_Parametros();
            //string a = EncryptionManager.Encrypt(@"Server=10.80.3.147\SQLINSTPROY;Database=E2E_Salvalineas_base_dev;User Id=E2E_Salvalineas_base;Password=7SrD2TA$;");
            parametros.cadena = EncryptionManager.Decrypt(ConfigurationManager.ConnectionStrings["DB_CONNECTION_" + cadena].ToString());
            parametros.querySelect = "WHERE Nombre_Parametro = @Nombre_Parametro";
            dicParameter = new Dictionary<string, string>();
            dicParameter.Add("@Nombre_Parametro", "SQL_SELECT_SIGRES");
            DataTable tbParametros = parametros.select(dicParameter).Tables[0];
            string consultaSOM = tbParametros.Rows[0]["Valor_Parametro"].ToString();

            string db_sigres = EncryptionManager.Decrypt(ConfigurationManager.ConnectionStrings["DB_CONNECTION_SIGRES_" + cadena].ToString());
            DataTable query = new DataTable();

            using (OracleConnection conn = new OracleConnection(db_sigres))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand();

                    cmd.CommandText = consultaSOM;
                    cmd.Connection = conn;
                    cmd.Parameters.Add("CAI", CAI);
                    conn.Open();
                    OracleDataReader queryOracle = cmd.ExecuteReader();

                    query.Load(queryOracle);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            if (query.Rows.Count > 0)
            {
                objetoSigres = new autosgrsm_SIGRES()
                {
                    SERVICESID = query.Rows[0]["SERVICESID"].ToString(),
                    ESTADO = query.Rows[0]["ESTADO"].ToString()
                }; 
            }
            else
            {
                objetoSigres = new autosgrsm_SIGRES();
            }

            return objetoSigres;
        }

    }
}