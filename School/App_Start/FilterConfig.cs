using System.Web;
using System.Web.Mvc;

namespace School
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //can't deal with the pages if you didn't logged in
            filters.Add(new AuthorizeAttribute());
        }
    }
}
