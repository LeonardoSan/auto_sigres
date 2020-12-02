using DBManager.DBManually.DataAnnotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.DBManually.DBManuallyModels
{
    [SP(nombreSP = "nombre del procedimiento en la base de datos sin el esquema")]
    //LA CLASE DEBE EXTENDER DE DBManuallyManager PARA USAR ES METODO DE EJECUTAR SP
    class SPprueba : DBManuallyManager
    {
        [ParametroSP(parametro = "paramatro del procedimiento con el @")]
        public string valor1 { get; set; }

        //SI TIENE MAS PARAMETROS DE AGREGAN DE LA MISMA MANERA 


    }
}
