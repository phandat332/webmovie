using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebMovie.Models;

namespace WebMovie.App_Start
{
    public class AdminAuthorize: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //1. check session : đã đăng nhập  => cho thực hiện filter
            // Ngược lại quay về trang đăng nhập
            KHACHHANG khSession = (KHACHHANG)HttpContext.Current.Session["Admin"];
            if (khSession != null)
            {
                return;
            }
            else
            {
                var returnUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        Controller= "Account",
                        Action= "Login", 
                       Area = "",
                        returnUrl = returnUrl.ToString()
                    }));
            }
           //2. check quyền: có quền thì => cho thực hiện filter
           // ngược lại thì cho trở lại trang => trang báo giới hạn quyền truy cập
        }
    }
}