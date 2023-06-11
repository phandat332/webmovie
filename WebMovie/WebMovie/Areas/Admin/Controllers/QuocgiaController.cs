using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMovie.App_Start;
using WebMovie.Models;
using PagedList;
using PagedList.Mvc;

namespace WebMovie.Areas.Admin.Controllers
{
    public class QuocgiaController : Controller
    {
        MovieDataDataContext data = new MovieDataDataContext();

        // GET: Admin/NamSX
        //quản lý quốc gia sản xuất phim
       [AdminAuthorize]
        [HttpGet]
        public ActionResult QLQG(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(data.QUOCGIAs.ToList().OrderBy(n => n.MaQG).ToPagedList(pageNumber, pageSize));
        }
        // Thêm quốc gia
       [AdminAuthorize]
        [HttpGet]
        public ActionResult ThemQG()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemQG(QUOCGIA quocgia)
        {
            if (string.IsNullOrEmpty(quocgia.TenQG))
            {
                ViewBag.ThongBao = "Bạn cần nhập tên quốc gia";
                return View(quocgia.TenQG);
            }
            var brand = data.QUOCGIAs.FirstOrDefault(b => b.TenQG == quocgia.TenQG);
            if (brand != null)
            {
                ViewBag.ThongBao = "Quốc gia đã tồn tại đã tồn tại";
                return View(quocgia);
            }
            // Lưu thương hiệu vào CSDL ở đây


            data.QUOCGIAs.InsertOnSubmit(quocgia);
            data.SubmitChanges();
            return RedirectToAction("QLQG");
        }

        //xoa quốc gia
        [AdminAuthorize]
        [HttpGet]
        public ActionResult XoaQG(int id)
        {
            QUOCGIA dm = data.QUOCGIAs.SingleOrDefault(n => n.MaQG == id);
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.idDM = dm.MaQG;
            return View(dm);

        }
        [HttpPost, ActionName("XoaQG")]
        public ActionResult AcceptXoaDM(int id)
        {
            QUOCGIA dm = data.QUOCGIAs.SingleOrDefault(n => n.MaQG == id);
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.idDM = dm.MaQG;
            data.QUOCGIAs.DeleteOnSubmit(dm);
            data.SubmitChanges();
            return RedirectToAction("QLQG");
        }
        //sua  quốc gia
        [AdminAuthorize]
        [HttpGet]
        public ActionResult SuaQG(int id)
        {
            QUOCGIA quocgia = data.QUOCGIAs.SingleOrDefault(n => n.MaQG == id);
            if (quocgia == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(quocgia);
        }
        [HttpPost, ActionName("SuaQG")]
        [ValidateInput(false)]
        public ActionResult save(int id)
        {

            QUOCGIA quocgiaph = data.QUOCGIAs.SingleOrDefault(n => n.MaQG == id);
            if (quocgiaph == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.MaTS = quocgiaph.MaQG;
            UpdateModel(quocgiaph);
            data.SubmitChanges();
            return RedirectToAction("QLQG");
        }
    }
}