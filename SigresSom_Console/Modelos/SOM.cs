using System;
using DBManager.DBManually;
using DBManager.DBManually.DataAnnotation;

namespace DBManager
{
    public class autosgrsm_SOM : DBManuallyManager
    {
        [Columna(nombreColumna = "Id", esPrimaryKey = true)]
        public double Id { get; set; }
        [Columna(nombreColumna = "FLOWID", esPrimaryKey = false)]
        public string FLOWID { get; set; }
        [Columna(nombreColumna = "TECNOLOGIA", esPrimaryKey = false)]
        public string TECNOLOGIA { get; set; }
        [Columna(nombreColumna = "OPERACION_COMERCIAL", esPrimaryKey = false)]
        public string OPERACION_COMERCIAL { get; set; }
        [Columna(nombreColumna = "ESTADO_DE_LA_ORDEN", esPrimaryKey = false)]
        public string ESTADO_DE_LA_ORDEN { get; set; }
        [Columna(nombreColumna = "APPOINTMENT_ID", esPrimaryKey = false)]
        public string APPOINTMENT_ID { get; set; }
        [Columna(nombreColumna = "FLOWNO", esPrimaryKey = false)]
        public string FLOWNO { get; set; }
        [Columna(nombreColumna = "ORDERCREATETIME", esPrimaryKey = false)]
        public DateTime ORDERCREATETIME { get; set; }
        [Columna(nombreColumna = "WORKCREATETIME", esPrimaryKey = false)]
        public DateTime WORKCREATETIME { get; set; }
        [Columna(nombreColumna = "DATE_ERROR", esPrimaryKey = false)]
        public DateTime DATE_ERROR { get; set; }
        [Columna(nombreColumna = "FLOWTYPE", esPrimaryKey = false)]
        public string FLOWTYPE { get; set; }
        [Columna(nombreColumna = "FLOWTYPEID", esPrimaryKey = false)]
        public string FLOWTYPEID { get; set; }
        [Columna(nombreColumna = "PROCESSINSTNAME", esPrimaryKey = false)]
        public string PROCESSINSTNAME { get; set; }
        [Columna(nombreColumna = "ACTIVITYDEFID", esPrimaryKey = false)]
        public string ACTIVITYDEFID { get; set; }
        [Columna(nombreColumna = "ACTIVITYINSTNAME", esPrimaryKey = false)]
        public string ACTIVITYINSTNAME { get; set; }
        [Columna(nombreColumna = "CRMNO", esPrimaryKey = false)]
        public string CRMNO { get; set; }
        [Columna(nombreColumna = "CUSTOMNAME", esPrimaryKey = false)]
        public string CUSTOMNAME { get; set; }
        [Columna(nombreColumna = "PRODUCT", esPrimaryKey = false)]
        public string PRODUCT { get; set; }
        [Columna(nombreColumna = "OPERATION", esPrimaryKey = false)]
        public string OPERATION { get; set; }
        [Columna(nombreColumna = "ACCESSACCOUNT", esPrimaryKey = false)]
        public string ACCESSACCOUNT { get; set; }
        [Columna(nombreColumna = "INSTALL_ADDRESS", esPrimaryKey = false)]
        public string INSTALL_ADDRESS { get; set; }
        [Columna(nombreColumna = "CODIGO_LOCALIDAD", esPrimaryKey = false)]
        public string CODIGO_LOCALIDAD { get; set; }
        [Columna(nombreColumna = "ERROR_CODE", esPrimaryKey = false)]
        public string ERROR_CODE { get; set; }
        [Columna(nombreColumna = "ERROR_DESC", esPrimaryKey = false)]
        public string ERROR_DESC { get; set; }
        [Columna(nombreColumna = "CREATED", esPrimaryKey = false)]
        public DateTime CREATED { get; set; }
        [Columna(nombreColumna = "STATUS_FLOW", esPrimaryKey = false)]
        public string STATUS_FLOW { get; set; }

    }
}
