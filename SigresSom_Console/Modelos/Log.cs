using System;
using DBManager.DBManually;
using DBManager.DBManually.DataAnnotation;

namespace DBManager
{
    public class autosgrsm_Log : DBManuallyManager
    { 
        [Columna(nombreColumna = "id", esPrimaryKey = true)]
        public double id { get; set; }
        [Columna(nombreColumna = "id_som", esPrimaryKey = false)]
        public string id_som { get; set; }
        [Columna(nombreColumna = "fecha", esPrimaryKey = false)]
        public DateTime fecha { get; set; }
        [Columna(nombreColumna = "estado", esPrimaryKey = false)]
        public Int32 estado { get; set; }
        [Columna(nombreColumna = "descripcion", esPrimaryKey = false)]
        public string descripcion { get; set; }
    }
}
