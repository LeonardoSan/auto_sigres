using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBManager.DBManually.DataAnnotation
{
    [AttributeUsage(System.AttributeTargets.Class)]
    public class SP: Attribute
    {
        public string nombreSP { get; set; }
    }
}