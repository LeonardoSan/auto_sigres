using System;
using DBManager.DBManually;
using DBManager.DBManually.DataAnnotation;

namespace DBManager
{
    public class autosgrsm_Parametros : DBManuallyManager
    {
        [Columna(nombreColumna = "Id", esPrimaryKey = true)]
        public double Id { get; set; }
        [Columna(nombreColumna = "Nombre_Parametro", esPrimaryKey = false)]
        public string Nombre_Parametro { get; set; }
        [Columna(nombreColumna = "Valor_Parametro", esPrimaryKey = false)]
        public string Valor_Parametro { get; set; }
        [Columna(nombreColumna = "Descripcion_Parametro", esPrimaryKey = false)]
        public string Descripcion_Parametro { get; set; }

    }
}
