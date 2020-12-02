using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.DBManually.DataAnnotation
{
    [AttributeUsage(System.AttributeTargets.Property)]
    public class Columna: Attribute
    {
        public string nombreColumna { get; set; }
        public bool esPrimaryKey { get; set; }
    }
}
