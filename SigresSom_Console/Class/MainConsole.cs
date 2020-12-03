using DBManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SigresSom_Console.Class
{

    public class CAIs
    {
        public string CRMNO { get; set; }
        public string CAI { get; set; }
    }

    public class MainConsole
    {

        public string cadena = "";
        string entorno = ConfigurationManager.AppSettings["Entorno"].ToString();
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
            Log log = new Log();
            log.insertLogGeneral(1, "Inicio Automatización");

            string consultaApiSOM = "";
            string consultaApiSigres = "";
            var connection = ConfigurationManager.ConnectionStrings["DB_CONNECTION_" + cadena].ToString();

            // Obtener Parametros "API_LIST_SOM"
            getParameters(connection, "API_LIST_SOM", ref consultaApiSOM);

            //Acceso a API e Inserción SOM
            loadSOMData(connection, consultaApiSOM);
            log.insertLogGeneral(1, "Inserción de SOM en BD Finalizada");

            //Consulta de SOM pendientes
            List<CAIs> listado = SearchSOMDataInitial(connection);
            if (listado.Count > 0)
            {
                // Obtener Parametros "API_LIST_SIGRES"
                getParameters(connection, "API_LIST_SIGRES", ref consultaApiSigres);

                // Llamado de Api Sigre por cada CAI de SOM
                callSigresSOM(connection, listado, consultaApiSigres);
            }

            Console.WriteLine("Proceso finalizado");
            Console.ReadLine();
        }


        public void getParameters(string connection, string parametro, ref string valorParametro)
        {
            Log log = new Log();

            try
            {
                autosgrsm_Parametros parametros = new autosgrsm_Parametros();
                parametros.cadena = EncryptionManager.Decrypt(connection);
                parametros.querySelect = "WHERE Nombre_Parametro = @Nombre_Parametro";
                dicParameter = new Dictionary<string, string>();
                dicParameter.Add("@Nombre_Parametro", parametro);
                DataTable tbParametros = parametros.select(dicParameter).Tables[0];
                valorParametro = tbParametros.Rows[0]["Valor_Parametro"].ToString();
            }
            catch (Exception ex)
            {
                log.insertLogGeneral(4, "No se pueden consultar los parámetros: " + ex.Message);
            }

        }

        public autosgrsm_SOM getSomDataByCRMNO(string connection, string parametro)
        {
            Log log = new Log();

            autosgrsm_SOM dataSOM = new autosgrsm_SOM();

            try
            {
                dataSOM.cadena = EncryptionManager.Decrypt(connection);
                dataSOM.querySelect = "WHERE CRMNO = @CRMNO";
                dicParameter = new Dictionary<string, string>();
                dicParameter.Add("@CRMNO", parametro);
                DataTable tbSOM = dataSOM.select(dicParameter).Tables[0];

                dataSOM.Id = Convert.ToInt32(tbSOM.Rows[0]["Id"].ToString());
                dataSOM.FLOWID = tbSOM.Rows[0]["FLOWID"].ToString();
                dataSOM.TECNOLOGIA = tbSOM.Rows[0]["TECNOLOGIA"].ToString();
                dataSOM.OPERACION_COMERCIAL = tbSOM.Rows[0]["OPERACION_COMERCIAL"].ToString();
                dataSOM.ESTADO_DE_LA_ORDEN = tbSOM.Rows[0]["ESTADO_DE_LA_ORDEN"].ToString();
                dataSOM.APPOINTMENT_ID = tbSOM.Rows[0]["APPOINTMENT_ID"].ToString();
                dataSOM.FLOWNO = tbSOM.Rows[0]["FLOWNO"].ToString();
                dataSOM.ORDERCREATETIME = Convert.ToDateTime(tbSOM.Rows[0]["ORDERCREATETIME"].ToString());
                dataSOM.WORKCREATETIME = Convert.ToDateTime(tbSOM.Rows[0]["WORKCREATETIME"].ToString());
                dataSOM.DATE_ERROR = Convert.ToDateTime(tbSOM.Rows[0]["DATE_ERROR"].ToString());
                dataSOM.FLOWTYPE = tbSOM.Rows[0]["FLOWTYPE"].ToString();
                dataSOM.FLOWTYPEID = tbSOM.Rows[0]["FLOWTYPEID"].ToString();
                dataSOM.PROCESSINSTNAME = tbSOM.Rows[0]["PROCESSINSTNAME"].ToString();
                dataSOM.ACTIVITYDEFID = tbSOM.Rows[0]["ACTIVITYDEFID"].ToString();
                dataSOM.ACTIVITYINSTNAME = tbSOM.Rows[0]["ACTIVITYINSTNAME"].ToString();
                dataSOM.CRMNO = tbSOM.Rows[0]["CRMNO"].ToString();
                dataSOM.CUSTOMNAME = tbSOM.Rows[0]["CUSTOMNAME"].ToString();
                dataSOM.PRODUCT = tbSOM.Rows[0]["PRODUCT"].ToString();
                dataSOM.OPERATION = tbSOM.Rows[0]["OPERATION"].ToString();
                dataSOM.ACCESSACCOUNT = tbSOM.Rows[0]["ACCESSACCOUNT"].ToString();
                dataSOM.INSTALL_ADDRESS = tbSOM.Rows[0]["INSTALL_ADDRESS"].ToString();
                dataSOM.CODIGO_LOCALIDAD = tbSOM.Rows[0]["CODIGO_LOCALIDAD"].ToString();
                dataSOM.ERROR_CODE = tbSOM.Rows[0]["ERROR_CODE"].ToString();
                dataSOM.ERROR_DESC = tbSOM.Rows[0]["ERROR_DESC"].ToString();
                dataSOM.CREATED = Convert.ToDateTime(tbSOM.Rows[0]["CREATED"].ToString());
                dataSOM.STATUS_FLOW = Convert.ToInt32(tbSOM.Rows[0]["STATUS_FLOW"].ToString());
                dataSOM.STATUS_CAI = Convert.ToInt32(tbSOM.Rows[0]["STATUS_CAI"].ToString());

            }
            catch (Exception ex)
            {
                log.insertLogGeneral(4, "No se pueden consultar los parámetros: " + ex.Message);
            }

            return dataSOM;
        }


        public void loadSOMData(string connection, string consultaApi)
        {
            Log log = new Log();
            List<autosgrsm_SOM> jsonSOM = null;

            if (consultaApi != "")
            {
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
                    log.insertLogGeneral(3, "Falló Api SOM: " + ex.Message);
                }

                if (jsonSOM.Count > 0)
                {
                    try
                    {
                        foreach (autosgrsm_SOM item in jsonSOM)
                        {
                            item.cadena = EncryptionManager.Decrypt(connection);
                            item.CREATED = DateTime.Now;
                            item.STATUS_FLOW = 2;
                            item.insert();

                            log.insertLog(item.CRMNO, item.STATUS_FLOW, "Cargue Tabla SOM");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.insertLogGeneral(4, "Falló Inserción en tabla SOM: " + ex.Message);
                    }
                }
                else
                {
                    log.insertLogGeneral(1, "La consulta SOM no trajo resultados");
                    Console.WriteLine("La consulta SOM no trajo resultados");
                }
            }


        }

        public void loadSIGRESData(string connection, string consultaApi, CAIs CAI)
        {
            Log log = new Log();
            List<autosgrsm_SIGRES> jsonSigres = null;

            if (consultaApi != "")
            {
                var request = (HttpWebRequest)WebRequest.Create(consultaApi + CAI.CAI);
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
                                jsonSigres = js.Deserialize<List<autosgrsm_SIGRES>>(json);
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    log.insertLogGeneral(3, "Falló Api Sigres: " + ex.Message);
                }

                if (jsonSigres.Count > 0)
                {
                    try
                    {
                        foreach (autosgrsm_SIGRES item in jsonSigres)
                        { 
                            autosgrsm_SOM som = getSomDataByCRMNO(connection, CAI.CRMNO);
                            som.cadena = EncryptionManager.Decrypt(connection);
                            som.STATUS_CAI = item.ESTADO;
                            som.STATUS_FLOW = 5;
                            som.queryUpdate = "WHERE CRMNO = @CRMNO";
                            dicParameter = new Dictionary<string, string>();
                            dicParameter.Add("@CRMNO", CAI.CRMNO); 
                            som.update(dicParameter);

                            log.insertLog(CAI.CRMNO, som.STATUS_FLOW, "Actualización del Estado CAI");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.insertLogGeneral(4, "Falló Actualziación en tabla SOM (Estado CAI): " + ex.Message);
                    }
                }
                else
                {
                    log.insertLog(CAI.CRMNO, 3, "No se encontró data para el CAI: " + CAI.CAI);
                    Console.WriteLine("No se encontró data para el CAI: " + CAI.CAI);
                }
            }


        }


        public List<CAIs> SearchSOMDataInitial(string connection)
        {
            Log log = new Log();
            log.insertLogGeneral(1, "Consulta de SOM pendientes: Estado Cargue Inicial");

            List<CAIs> listado = new List<CAIs>();
            string STATUS_FLOW = "2";

            try
            {
                autosgrsm_SOM dataSom = new autosgrsm_SOM();
                dataSom.cadena = EncryptionManager.Decrypt(connection);
                dataSom.querySelect = "WHERE STATUS_FLOW = @STATUS_FLOW";
                dicParameter = new Dictionary<string, string>();
                dicParameter.Add("@STATUS_FLOW", STATUS_FLOW);
                DataTable somDatatable = dataSom.select(dicParameter).Tables[0];

                if (somDatatable.Rows.Count > 0)
                {
                    foreach (DataRow item in somDatatable.Rows)
                    {
                        CAIs cai = new CAIs()
                        {
                            CRMNO = item["CRMNO"].ToString(),
                            CAI = item["ERROR_DESC"].ToString().Replace("Service", "").Replace("already exists", "").Trim()
                        };

                        listado.Add(cai);
                    }
                }
                else
                {
                    log.insertLogGeneral(1, "No se encontraron datos de SOM pendientes. Proceso finalizado.");
                }
            }
            catch (Exception ex)
            {
                log.insertLogGeneral(4, "Error en la consulta de datos de SOM pendientes: " + ex.Message);
            }

            return listado;
        }

        public void callSigresSOM(string connection, List<CAIs> listado, string consultaApiSigres)
        {
            foreach (CAIs item in listado)
            {
                loadSIGRESData(connection, consultaApiSigres, item);
            }

        }

    }
}
