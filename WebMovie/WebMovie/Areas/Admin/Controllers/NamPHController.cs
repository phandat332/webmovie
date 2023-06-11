using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMovie.Models;
using PagedList;
using PagedList.Mvc;
using WebMovie.ViewModel;
using System.Text;
using WebMovie.App_Start;

namespace WebMovie.Areas.Admin.Controllers
{
    public class NamPHController : Controller
    {
        MovieDataDataContext data = new MovieDataDataContext();
        
        // GET: Admin/NamSX
        [AdminAuthorize]
        [HttpGet]
        public ActionResult QLNam(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(data.NAMPHATHANHs.OrderByDescending(n => n.Nam).ToList().ToPagedList(pageNumber, pageSize));
        }
        //Them nam phat hanh
        [AdminAuthorize]
        [HttpGet]
        public ActionResult ThemNam()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemNam(FormCollection collection,NAMPHATHANH n)
        {
            if (string.IsNullOrEmpty(n.Nam))
            {
                ViewBag.ThongBao = "Bạn cần nhập năm phát hành";
                return View(n.Nam);
            }
            var brand = data.NAMPHATHANHs.FirstOrDefault(b => b.Nam == n.Nam);
            if (brand != null)
            {
                ViewBag.ThongBao = "Năm phát hành đã tồn tại";
                return View(n);
            }


            // Lưu thương hiệu vào CSDL ở đây


            data.NAMPHATHANHs.InsertOnSubmit(n);
            data.SubmitChanges();
            return RedirectToAction("QLNam");
        }

        //xoa nam phat hanh
        [AdminAuthorize]
         [HttpGet]
         public ActionResult XoaNam(int id)
         {
             NAMPHATHANH dm = data.NAMPHATHANHs.SingleOrDefault(n => n.MaNam == id);
             if (dm == null)
             {
                 Response.StatusCode = 404;
                 return null;
             }
             ViewBag.idDM = dm.MaNam;
             return View(dm);

         }
         [HttpPost, ActionName("XoaNam")]
         public ActionResult AcceptXoaDM(int id)
         {
             NAMPHATHANH dm = data.NAMPHATHANHs.SingleOrDefault(n => n.MaNam == id);
             if (dm == null)
             {
                 Response.StatusCode = 404;
                 return null;
             }
             ViewBag.idDM = dm.MaNam;
             data.NAMPHATHANHs.DeleteOnSubmit(dm);
             data.SubmitChanges();
             return RedirectToAction("QLNam");
         }
        //sua nam phat hanh
        [AdminAuthorize]
         [HttpGet]
         public ActionResult SuaNam(int id)
         {
             NAMPHATHANH danhmuc = data.NAMPHATHANHs.SingleOrDefault(n => n.MaNam == id);
             if (danhmuc == null)
             {
                 Response.StatusCode = 404;
                 return null;
             }

             return View(danhmuc);
         }
         [HttpPost, ActionName("SuaNam")]
         [ValidateInput(false)]
         public ActionResult save(int id)
         {

             NAMPHATHANH namph = data.NAMPHATHANHs.SingleOrDefault(n => n.MaNam == id);
             if (namph == null)
             {
                 Response.StatusCode = 404;
                 return null;
             }

             ViewBag.MaTS = namph.MaNam;
             UpdateModel(namph);
             data.SubmitChanges();
             return RedirectToAction("QLNam");
         }
    }
}