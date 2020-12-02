using System.Web;
using System.Web.Mvc;

namespace api_consulta_sigres_som
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
