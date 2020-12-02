using DBManager.DBManually.DataAnnotation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;

namespace DBManager.DBManually
{
    public class DBManuallyManager
    {


        public string server = ".";
        public string catalogo = "";
        public string username = "";
        public string password = "";
        public string querySelect = "";
        public string queryUpdate = "";
        public string queryDelete = "";
        public string queryInsertUpdate = "";
        public string querySimpleCommand = "";
        public string querySimpleCommandDataTable = "";
        public int MaxValue = 0;
        //private string cadena = "Data Source="+server+";Initial Catalog="+catalogo+";User ID="+username+";Password="+password+";";
        //private string cadenaLocal = "Data Source=.;Initial Catalog="+catalogo+";Integrated Security=True;";
        public string cadena = "";

        private Type clase;

        public DBManuallyManager()
        {
            clase = this.GetType();
        }

        private string getConnectioString()
        {
            string cadena = "Data Source=" + server + ";Initial Catalog=" + catalogo + ";User ID=" + username + ";Password=" + password + "; Connection Timeout=" + MaxValue;
            string cadenaLocal = "Data Source=.;Initial Catalog=" + catalogo + ";Integrated Security=True; Connection Timeout = " + MaxValue;
            string connectionString = "";

            connectionString = cadena;
            if (server == ".")
            {
                connectionString = cadenaLocal;
            }
            if (this.cadena != "")
            {
                connectionString = this.cadena;
            }

            return connectionString;

        }

        private string getConnectionStringCurrent()
        {
            string cadena = "";
            string entorno = System.Configuration.ConfigurationManager.AppSettings["Entorno"].ToString();

            if (entorno == "0")
                cadena = "DB_CONNECTION_DEV";
            else if (entorno == "1")
                cadena = "DB_CONNECTION_TEST";
            else if (entorno == "2")
                cadena = "DB_CONNECTION_PROD";

            cadena = EncryptionManager.Decrypt(System.Configuration.ConfigurationManager.ConnectionStrings[cadena].ConnectionString);

            return cadena;
        }

        public SqlConnection startConnection()
        {
            return new SqlConnection(getConnectioString());
        }

        public SqlConnection startConnectionCurrent()
        {
            return new SqlConnection(getConnectionStringCurrent());
        }

        public void closeConnection(SqlCommand command)
        {
            command.Connection.Close();
            command.Connection.Dispose();
        }

        private SqlDataAdapter crearSp()
        {
            PropertyInfo[] propiedades = null;
            SP annotatioSP = clase.GetCustomAttribute<SP>();

            if (annotatioSP == null)
            {
                return null;
            }

            SqlCommand cmd = new SqlCommand(annotatioSP.nombreSP, startConnection());
            cmd.Connection.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            propiedades = clase.GetProperties();

            foreach (PropertyInfo prop in propiedades)
            {
                var i = prop.GetCustomAttribute<ParametroSP>();
                if (i != null)
                {
                    object valor = prop.GetValue(this, null);
                    cmd.Parameters.AddWithValue(i.parametro, valor);
                }

            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            //closeConnection(cmd);

            return da;
        }

        public T ejecutarSP<T>()
        {

            SqlDataAdapter da = crearSp();
            DataSet dataset = new DataSet();
            DataTable dt = new DataTable();
            DataTable[] dts = null;
            DataTableCollection tablas = null;
            Object obj = null;
            if (da == null)
            {
                return (T)obj;
            }

            switch (typeof(T).Name)
            {
                case "DataTable":

                    da.Fill(dataset);
                    dt = dataset.Tables[0];
                    obj = dt;

                    break;
                case "DataTable[]":

                    da.Fill(dataset);
                    tablas = dataset.Tables;
                    dts = new DataTable[tablas.Count];

                    for (int i = 0; i < tablas.Count; i++)
                    {
                        dts[i] = tablas[i];
                    }
                    obj = dts;

                    break;
                case "DataTableCollection":

                    da.Fill(dataset);
                    tablas = dataset.Tables;
                    obj = tablas;

                    break;
                case "DataSet":

                    da.Fill(dataset);
                    obj = dataset;

                    break;
                default:
                    break;
            }

            ((IDbDataAdapter)da).SelectCommand.Connection.Close();
            ((IDbDataAdapter)da).SelectCommand.Connection.Dispose();

            return (T)obj;
        }

        public DataSet select()
        {
            return select(null);
        }

        public DataSet select(Dictionary<string, string> param)
        {
            DataSet tbl = new DataSet();
            PropertyInfo[] propiedades = null;
            propiedades = clase.GetProperties();
            string query = querySelect;

            //string sqlSelect = "SELECT *";
            //foreach (PropertyInfo prop in propiedades)
            //{
            //    sqlSelect += prop.Name + ",";
            //}

            //sqlSelect = sqlSelect.Substring(0, sqlSelect.Length - 1);

            //sqlSelect += " FROM " + clase.Name;

            if (query == "NO WHERE")
            {
                //sqlSelect += " " + where;
                query = " ";
            }
            string sqlSelect = String.Format("SELECT * FROM {0} {1}", clase.Name, query);

            SqlCommand command = new SqlCommand(sqlSelect, startConnection());
            if (param != null)
            {
                foreach (KeyValuePair<string, string> item in param)
                {
                    command.Parameters.AddWithValue(item.Key, item.Value);
                }
            }
            command.Connection.Open();
            var dat = new SqlDataAdapter(command);
            dat.Fill(tbl);

            closeConnection(command);

            return tbl;
        }

        public DataSet selectTOP(Dictionary<string, string> param, string top = "0")
        {
            DataSet tbl = new DataSet();
            PropertyInfo[] propiedades = null;
            propiedades = clase.GetProperties();
            string query = querySelect;
              
            if (query == "NO WHERE")
            {
                //sqlSelect += " " + where;
                query = " ";
            }

            if (top == "0" || top == "*" || top.Trim() == "" || top == null)
            { 
                top = " ";
            } else
            {
                top = "TOP " + top;
            }

            string sqlSelect = String.Format("SELECT {2} * FROM {0} {1}", clase.Name, query, top); 

            SqlCommand command = new SqlCommand(sqlSelect, startConnection());
            if (param != null)
            {
                foreach (KeyValuePair<string, string> item in param)
                {
                    command.Parameters.AddWithValue(item.Key, item.Value);
                }
            }
            command.Connection.Open();
            var dat = new SqlDataAdapter(command);
            dat.Fill(tbl);

            closeConnection(command);

            return tbl;
        }

        public DataSet selectQuery(string sqlSelect, Dictionary<string, string> param)
        {
            DataSet tbl = new DataSet(); 
            SqlCommand command = new SqlCommand(sqlSelect, startConnection());
            if (param != null)
            {
                foreach (KeyValuePair<string, string> item in param)
                {
                    command.Parameters.AddWithValue(item.Key, item.Value);
                }
            }
            command.Connection.Open();
            var dat = new SqlDataAdapter(command);
            dat.Fill(tbl);

            closeConnection(command);

            return tbl;
        }

        public DataSet selectOIM(Dictionary<string, string> param)
        {
            DataSet tbl = new DataSet();
            PropertyInfo[] propiedades = null;
            propiedades = clase.GetProperties();
            string query = querySelect;

            //"";

            if (query == "NO WHERE")
            {
                //sqlSelect += " " + where;
                query = " ";
            }
            string sqlSelect = String.Format("SELECT [Cedula],[Login_OIM],[Nombre],[Estado_OIM],[cargo],[Tipo_Emp],[Compañia],[Fullstack_Rol] FROM [sql_evolucion].[dbo].[VTAPAP_OIM11_UsuarioS] {0} {1}", clase.Name, query);

            SqlCommand command = new SqlCommand(sqlSelect, startConnection());
            if (param != null)
            {
                foreach (KeyValuePair<string, string> item in param)
                {
                    command.Parameters.AddWithValue(item.Key, item.Value);
                }
            }
            command.Connection.Open();
            var dat = new SqlDataAdapter(command);
            dat.Fill(tbl);

            closeConnection(command);

            return tbl;
        }

        public object selectPrimero(Dictionary<string, string> param)
        {
            DataSet tbl = new DataSet();
            PropertyInfo[] propiedades = null;
            propiedades = clase.GetProperties();
            string query = querySelect;
            //foreach (PropertyInfo prop in propiedades)
            //{
            //    sqlSelect += prop.Name + ",";
            //}

            //sqlSelect = sqlSelect.Substring(0, sqlSelect.Length - 1);

            //sqlSelect += " FROM " + clase.Name;

            if (query == "NO WHERE")
            {
                //sqlSelect += " " + where;
                query = " ";
            }
            string sqlSelect = String.Format("SELECT * FROM [{0}] {1}", clase.Name, query);
            SqlCommand command = new SqlCommand(sqlSelect, startConnection());
            if (param != null)
            {
                foreach (KeyValuePair<string, string> item in param)
                {
                    command.Parameters.AddWithValue(item.Key, item.Value);
                }
            }
            //command.Parameters.AddWithValue("@table", clase.Name);
            //command.Parameters.AddWithValue("@where", where);
            command.Connection.Open();
            var dat = new SqlDataAdapter(command);
            dat.Fill(tbl);
            if (tbl.Tables[0].Rows.Count > 0)
            {
                var row = tbl.Tables[0].Rows[0].ItemArray;

                var obj = new ExpandoObject() as IDictionary<string, object>;

                var cont = 0;
                foreach (var item in row)
                {
                    var columna = tbl.Tables[0].Columns[cont].ToString();
                    obj.Add(columna, item);
                    cont++;
                }

                var js = new JavaScriptSerializer();

                closeConnection(command);

                return js.ConvertToType(obj, clase);
            }
            else
            {
                return null;
            }
        }

        public int delete(Dictionary<string, string> param)
        {
            PropertyInfo[] propiedades = null;
            propiedades = clase.GetProperties();
            //string sqlDelete = "DELETE FROM " + clase.Name;
            string query = queryDelete;
            if (query == "todo")
            {
                query = " ";
            }
            string sqlDelete = String.Format("DELETE FROM {0} {1}", clase.Name, query);


            SqlCommand command = new SqlCommand(sqlDelete, startConnection());
            command.Connection.Open();
            if (param != null)
            {
                foreach (KeyValuePair<string, string> item in param)
                {
                    command.Parameters.AddWithValue(item.Key, item.Value);
                }
            }
            int afectedRows = command.ExecuteNonQuery();

            closeConnection(command);

            return afectedRows;
        }

        public int insert()
        {
            PropertyInfo[] propiedades = null;
            propiedades = clase.GetProperties();
            int cantInsert = 0;
            string sqlSelect = "INSERT INTO  " + clase.Name + " ( ";
            string sqlValores = "VALUES (";
            foreach (PropertyInfo prop in propiedades)
            {
                Type t = prop.PropertyType;
                var i = prop.GetCustomAttribute<Columna>();
                object valor = prop.GetValue(this, null);
                //cmd.Parameters.AddWithValue(i.parametro, valor);
                if (valor != null && !i.esPrimaryKey)
                {
                    sqlSelect += i.nombreColumna + ",";
                    if (t.GenericTypeArguments.Length > 0)
                    {
                        if (t.GenericTypeArguments[0].Name.ToString() == "DateTime")
                        {
                            valor = DateTime.Parse(valor.ToString()).ToString("MM/dd/yyyy HH:mm:ss");
                        }
                    }
                    else
                    {
                        if (t.ToString().Contains("DateTime"))
                        {
                            valor = DateTime.Parse(valor.ToString()).ToString("MM/dd/yyyy HH:mm:ss");
                        }
                    }
                    if (valor.ToString() == "1/01/0001 12:00:00 a. m." || valor.ToString() == "1/01/0001 12:00:00")
                    {
                        valor = null;
                    }
                    if (valor == null)
                    {
                        sqlValores += "NULL,";
                    }
                    //else if (valor.ToString().Contains(" a. m.")) {
                    //    valor = valor.ToString().Substring(0, valor.ToString().IndexOf(" a. m."));
                    //    //valor = valor.ToString().IndexOf(" a. m.")
                    //    sqlValores += "'" + valor + "',";
                    //}
                    //else if (valor.ToString().Contains(" p. m."))
                    //{
                    //    valor = valor.ToString().Substring(0, valor.ToString().IndexOf(" p. m."));
                    //    //valor = valor.ToString().IndexOf(" a. m.")
                    //    sqlValores += "'" + valor + "',";
                    //}
                    else
                    {
                        sqlValores += "'" + valor + "',";

                    }
                    cantInsert++;
                }
                //if (i.esPrimaryKey && (double)valor > 0) {
                //    sqlSelect += prop.Name + ",";
                //    sqlValores += "'" + valor + "',";
                //}
            }
            int modified = -2;
            if (cantInsert > 0)
            {
                sqlSelect = sqlSelect.Substring(0, sqlSelect.Length - 1) + ") ";
                sqlValores = sqlValores.Substring(0, sqlValores.Length - 1) + ") ";
                //sqlSelect += " FROM " + clase.Name;
                sqlSelect += sqlValores;


                SqlCommand command = new SqlCommand(sqlSelect + "; SELECT SCOPE_IDENTITY();", startConnectionCurrent());
                command.Connection.Open();
                modified = Convert.ToInt32(command.ExecuteScalar());
                //var dat = new SqlDataAdapter(command);
                //dat.Fill(tbl);
                closeConnection(command);
            }

            return modified;
        }


        public int insertAndUpdate(Dictionary<string, string> param)
        {
            PropertyInfo[] propiedades = null;
            propiedades = clase.GetProperties();
            int resultado = -2;
            string QuerySelect = " WHERE ";
            querySelect = queryInsertUpdate;
            queryUpdate = queryInsertUpdate;
            foreach (PropertyInfo prop in propiedades)
            {
                var i = prop.GetCustomAttribute<Columna>();
                if (i.esPrimaryKey)
                {
                    object valor = prop.GetValue(this, null);
                    QuerySelect += i.nombreColumna + "='" + valor + "';";
                }
            }

            object res = this.selectPrimero(param);
            if (res == null)
            {
                resultado = this.insert();
            }
            else
            {
                PropertyInfo[] propiedadesUpdate = null;
                propiedadesUpdate = res.GetType().GetProperties();


                foreach (PropertyInfo propUp in propiedadesUpdate)
                {
                    var i = propUp.GetCustomAttribute<Columna>();
                    object valor = propUp.GetValue(res, null);
                    object valor2 = propUp.GetValue(this, null);
                    if (!i.esPrimaryKey && valor != null)
                    {
                        if (valor2 == null)
                        {
                            clase.GetProperty(i.nombreColumna).SetValue(this, valor);
                        }
                        //QuerySelect += propUp.Name + "='" + valor + "';";
                    }
                }


                resultado = this.update(param);
            }
            return resultado;
        }

        public int update(Dictionary<string, string> param)
        {
            PropertyInfo[] propiedades = null;
            propiedades = clase.GetProperties();
            string query = queryUpdate;
            int cantInsert = 0;
            string sqlSelect = "UPDATE [" + clase.Name + "] ";
            string sqlValores = "SET ";
            foreach (PropertyInfo prop in propiedades)
            {
                Type t = prop.PropertyType;
                var i = prop.GetCustomAttribute<Columna>();
                object valor = prop.GetValue(this, null);
                //cmd.Parameters.AddWithValue(i.parametro, valor);
                if (valor != null && !i.esPrimaryKey)
                {
                    //sqlSelect += prop.Name + ",";
                    if (t.Name == "DateTime")
                    {
                        valor = DateTime.Parse(valor.ToString()).ToString("MM/dd/yyyy HH:mm:ss");
                    }
                    if (t.GenericTypeArguments.Length > 0)
                    {
                        if (t.GenericTypeArguments[0].Name.ToString() == "DateTime")
                        {
                            valor = DateTime.Parse(valor.ToString()).ToString("MM/dd/yyyy HH:mm:ss");
                        }
                    }
                    if (valor.ToString() == "1/01/0001 12:00:00 a. m." || valor.ToString() == "1/01/0001 12:00:00")
                    {
                        valor = null;
                    }
                    if (valor == null)
                    {
                        //sqlValores += "NULL,";
                        sqlValores += i.nombreColumna + "=NULL,";
                    }
                    //else if (valor.ToString().Contains(" a. m."))
                    //{
                    //    valor = valor.ToString().Substring(0, valor.ToString().IndexOf(" a. m."));
                    //    //valor = valor.ToString().IndexOf(" a. m.")
                    //    sqlValores += prop.Name + "='" + valor + "',";
                    //}
                    //else if (valor.ToString().Contains(" p. m."))
                    //{
                    //    valor = valor.ToString().Substring(0, valor.ToString().IndexOf(" p. m."));
                    //    //valor = valor.ToString().IndexOf(" a. m.")
                    //    sqlValores += prop.Name + "='" + valor + "',";
                    //}
                    else
                    {
                        sqlValores += i.nombreColumna + "='" + valor + "',";

                    }
                    cantInsert++;
                }
                //if (valor != null && !i.esPrimaryKey)
                //{
                //    sqlValores += prop.Name + "='" + valor + "',";
                //}
            }
            int afectedRows = -2;
            if (cantInsert > 0)
            {
                sqlValores = sqlValores.Substring(0, sqlValores.Length - 1) + " ";
                //sqlSelect += " FROM " + clase.Name;
                sqlSelect += sqlValores;

                if (query != "todo")
                {
                    if (!query.ToLower().Contains("where"))
                    {
                        return -2;
                    }
                    sqlSelect += " " + query;
                }


                SqlCommand command = new SqlCommand(sqlSelect, startConnection());
                command.Connection.Open();
                if (param != null)
                {
                    foreach (KeyValuePair<string, string> item in param)
                    {
                        command.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }
                afectedRows = command.ExecuteNonQuery();


                closeConnection(command);
            }

            return afectedRows;
        }

        public int simpleCommand()
        {
            string sqlCommand = querySimpleCommand;
            sqlCommand = String.Format(sqlCommand);

            SqlCommand command = new SqlCommand(sqlCommand, startConnection());
            command.Connection.Open();

            int afectedRows = command.ExecuteNonQuery();

            closeConnection(command);

            return afectedRows;
        }

        public DataTable simpleCommandDataTable()
        {
            string sqlCommand = querySimpleCommandDataTable;
            sqlCommand = String.Format(sqlCommand);

            DataTable tbl = new DataTable();

            SqlCommand command = new SqlCommand(sqlCommand, startConnection());

            var dat = new SqlDataAdapter(command);
            dat.Fill(tbl);

            closeConnection(command);

            return tbl;

        }
    }
}