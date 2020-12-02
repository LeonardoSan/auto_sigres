using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBManager.DBManually.DataAnnotation
{
    [AttributeUsage(System.AttributeTargets.Property)]
    public class ParametroSP : Attribute
    {
        public string parametro { get; set; }
    }
}