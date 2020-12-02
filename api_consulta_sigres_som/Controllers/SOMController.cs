using DBManager;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;
using Oracle.ManagedDataAccess.Client;
using System;

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

        public string index()
        {
            return "Bienvenido a la Api de SOM";
        }

        [HttpGet]
        [Route("consulta")]
        public List<autosgrsm_SOM> consulta()
        {
            List<autosgrsm_SOM> objetoSOM = new List<autosgrsm_SOM>();

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

            //using (OracleConnection conn = new OracleConnection(db_som))
            //{
            //    try
            //    {
            //        OracleCommand cmd = new OracleCommand();

            //        cmd.CommandText = consultaSOM;
            //        cmd.Connection = conn;
            //        conn.Open();
            //        OracleDataReader queryOracle = cmd.ExecuteReader();

            //        query.Load(queryOracle);
            //        conn.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //}

            query = new DataTable();
            DataColumn column = new DataColumn();
            column.ColumnName = "FLOWID";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "TECNOLOGIA";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "OPERACION_COMERCIAL";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "ESTADO_DE_LA_ORDEN";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "APPOINTMENT_ID";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "FLOWNO";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "ORDERCREATETIME";
            column.DataType = Type.GetType("System.DateTime");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "WORKCREATETIME";
            column.DataType = Type.GetType("System.DateTime");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "DATE_ERROR";
            column.DataType = Type.GetType("System.DateTime");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "FLOWTYPE";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "FLOWTYPEID";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "PROCESSINSTNAME";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "ACTIVITYDEFID";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "ACTIVITYINSTNAME";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "CRMNO";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "CUSTOMNAME";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "PRODUCT";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "OPERATION";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "ACCESSACCOUNT";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "INSTALL_ADDRESS";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "CODIGO_LOCALIDAD";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "ERROR_CODE";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);
            column = new DataColumn();
            column.ColumnName = "ERROR_DESC";
            column.DataType = Type.GetType("System.String");
            query.Columns.Add(column);

            for (int i = 0; i < 10; i++)
            {
                DataRow myNewRow;
                myNewRow = query.NewRow();

                myNewRow["FLOWID"] = "16065051850190100-" + i;
                myNewRow["TECNOLOGIA"] = "";
                myNewRow["OPERACION_COMERCIAL"] = "Posventa";
                myNewRow["ESTADO_DE_LA_ORDEN"] = "Executing";
                myNewRow["APPOINTMENT_ID"] = "FS202011271424130143";
                myNewRow["FLOWNO"] = "4531008";
                myNewRow["ORDERCREATETIME"] = "27/11/2020 14:26";
                myNewRow["WORKCREATETIME"] = "27/11/2020 14:26";
                myNewRow["DATE_ERROR"] = "27/11/2020 14:37";
                myNewRow["FLOWTYPE"] = "OM";
                myNewRow["FLOWTYPEID"] = "OM";
                myNewRow["PROCESSINSTNAME"] = "FMC Process-GOMEZ GOMEZ MORALES DEISY GOMEZ MORALES";
                myNewRow["ACTIVITYDEFID"] = "InvokeIM";
                myNewRow["ACTIVITYINSTNAME"] = "InvokeIM";
                myNewRow["CRMNO"] = "20000474329533";
                myNewRow["CUSTOMNAME"] = "GOMEZ GOMEZ MORALES DEISY GOMEZ MORALES";
                myNewRow["PRODUCT"] = "Broadband Service+Fixed Line Voice+IPTV+TV";
                myNewRow["OPERATION"] = "Keep+Keep+Install+Uninstall";
                myNewRow["ACCESSACCOUNT"] = "IPTV:640000181711+DTH:651340443462+LB:82692349+BB:2582692349";
                myNewRow["INSTALL_ADDRESS"] = "CL 4 KR 4-54 LA POLA";
                myNewRow["CODIGO_LOCALIDAD"] = "73001000";
                myNewRow["ERROR_CODE"] = "INV-SIGRES-00010001";
                myNewRow["ERROR_DESC"] = "Service CIP_00000000000000000000000004531008 already exists";
                query.Rows.Add(myNewRow);
            }

            if (query.Rows.Count > 0)
            {
                foreach (DataRow item in query.Rows)
                {
                    autosgrsm_SOM itemSom = new autosgrsm_SOM()
                    {
                        FLOWID = item["FLOWID"].ToString(),
                        TECNOLOGIA = item["TECNOLOGIA"].ToString(),
                        OPERACION_COMERCIAL = item["OPERACION_COMERCIAL"].ToString(),
                        ESTADO_DE_LA_ORDEN = item["ESTADO_DE_LA_ORDEN"].ToString(),
                        APPOINTMENT_ID = item["APPOINTMENT_ID"].ToString(),
                        FLOWNO = item["FLOWNO"].ToString(),
                        ORDERCREATETIME = Convert.ToDateTime(item["ORDERCREATETIME"].ToString()),
                        WORKCREATETIME = Convert.ToDateTime(item["WORKCREATETIME"].ToString()),
                        DATE_ERROR = Convert.ToDateTime(item["DATE_ERROR"].ToString()),
                        FLOWTYPE = item["FLOWTYPE"].ToString(),
                        FLOWTYPEID = item["FLOWTYPEID"].ToString(),
                        PROCESSINSTNAME = item["PROCESSINSTNAME"].ToString(),
                        ACTIVITYDEFID = item["ACTIVITYDEFID"].ToString(),
                        ACTIVITYINSTNAME = item["ACTIVITYINSTNAME"].ToString(),
                        CRMNO = item["CRMNO"].ToString(),
                        CUSTOMNAME = item["CUSTOMNAME"].ToString(),
                        PRODUCT = item["PRODUCT"].ToString(),
                        OPERATION = item["OPERATION"].ToString(),
                        ACCESSACCOUNT = item["ACCESSACCOUNT"].ToString(),
                        INSTALL_ADDRESS = item["INSTALL_ADDRESS"].ToString(),
                        CODIGO_LOCALIDAD = item["CODIGO_LOCALIDAD"].ToString(),
                        ERROR_CODE = item["ERROR_CODE"].ToString(),
                        ERROR_DESC = item["ERROR_DESC"].ToString()
                    };

                    objetoSOM.Add(itemSom);
                }
            }
            else
            {
                objetoSOM = new List<autosgrsm_SOM>();
            }

            return objetoSOM;


        }
    }
}
