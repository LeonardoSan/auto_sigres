using System;
using DBManager.DBManually;
using DBManager.DBManually.DataAnnotation;

namespace DBManager
{
    public class autosgrsm_SOM : DBManuallyManager
    {
        public string FLOWID { get; set; }
        public string TECNOLOGIA { get; set; }
        public string OPERACION_COMERCIAL { get; set; }
        public string ESTADO_DE_LA_ORDEN { get; set; }
        public string APPOINTMENT_ID { get; set; }
        public string FLOWNO { get; set; }
        public DateTime ORDERCREATETIME { get; set; }
        public DateTime WORKCREATETIME { get; set; }
        public DateTime DATE_ERROR { get; set; }
        public string FLOWTYPE { get; set; }
        public string FLOWTYPEID { get; set; }
        public string PROCESSINSTNAME { get; set; }
        public string ACTIVITYDEFID { get; set; }
        public string ACTIVITYINSTNAME { get; set; }
        public string CRMNO { get; set; }
        public string CUSTOMNAME { get; set; }
        public string PRODUCT { get; set; }
        public string OPERATION { get; set; }
        public string ACCESSACCOUNT { get; set; }
        public string INSTALL_ADDRESS { get; set; }
        public string CODIGO_LOCALIDAD { get; set; }
        public string ERROR_CODE { get; set; }
        public string ERROR_DESC { get; set; }

    }
}
