using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMovie.Models;
using PagedList;
using PagedList.Mvc;
using WebMovie.App_Start;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace WebMovie.Areas.Admin.Controllers
{
    public class TheloaiController : Controller
    {
        MovieDataDataContext data = new MovieDataDataContext();
        // GET: Admin/Theloai

        #region quản ly Thể loại
        [AdminAuthorize]
        public ActionResult QLTheloai(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(data.THELOAIs.ToList().OrderBy(n => n.MaTL).ToPagedList(pageNumber, pageSize));
        }
        //Them thể loại
        [AdminAuthorize]
        [HttpGet]
        public ActionResult ThemTL()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemTL(THELOAI theloai)
        {
            if (string.IsNullOrEmpty(theloai.TenTL))
            {
                ViewBag.ThongBao = "Bạn cần nhập tên thể loại";
                return View(theloai.TenTL);
            }
            var brand = data.THELOAIs.FirstOrDefault(b => b.TenTL == theloai.TenTL);
            if (brand != null)
            {
                ViewBag.ThongBao = "Tên thể loại đã tồn tại";
                return View(theloai);
            }
          

            // Lưu thương hiệu vào CSDL ở đây


            data.THELOAIs.InsertOnSubmit(theloai);
            data.SubmitChanges();
            return RedirectToAction("QLTheloai");
        }
        //xoa thể loại
        [AdminAuthorize]
        [HttpGet]
        public ActionResult XoaTL(int id)
        {


            THELOAI dm = data.THELOAIs.SingleOrDefault(n => n.MaTL == id);
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.idDM = dm.MaTL;
            return View(dm);

        }
        [HttpPost, ActionName("XoaTL")]
        public ActionResult AcceptXoath(int id)
        {
            THELOAI dm = data.THELOAIs.SingleOrDefault(n => n.MaTL == id);
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.idDM = dm.MaTL;
            data.THELOAIs.DeleteOnSubmit(dm);
            data.SubmitChanges();
            return RedirectToAction("QLTheloai");
        }
        //sua thể loại
        [AdminAuthorize]
        [HttpGet]
        public ActionResult SuaTL(int id)
        {
            THELOAI theloai = data.THELOAIs.SingleOrDefault(n => n.MaTL == id);
            if (theloai == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(theloai);
        }
        [HttpPost, ActionName("SuaTL")]
        [ValidateInput(false)]
        public ActionResult DropDown(int id)
        {

            THELOAI theloai = data.THELOAIs.SingleOrDefault(n => n.MaTL == id);
            if (theloai == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.MaTS = theloai.MaTL;
            UpdateModel(theloai);
            data.SubmitChanges();
            return RedirectToAction("QLTheloai");
        }

        #endregion
    }
}