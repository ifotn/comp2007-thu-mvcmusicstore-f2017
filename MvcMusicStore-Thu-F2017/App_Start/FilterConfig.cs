using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore_Thu_F2017
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
