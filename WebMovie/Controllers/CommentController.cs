using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebMovie.App_Start;
using WebMovie.Models;

namespace WebMovie.Controllers
{

    public class CommentController : Controller
    {
        private MovieDataDataContext db = new MovieDataDataContext();

        public ActionResult Index(int Maphim)
        {
            string hoten = ((KHACHHANG)Session["User"]).Hoten;
            ViewBag.Maphim = Maphim;
            //đếm só bình luận của phim
            int totalBinhLuan = db.COMMENTs.Count(c => c.Maphim == Maphim);
            ViewBag.TotalBinhLuan = totalBinhLuan;
            ViewBag.Hoten = hoten;
            return View();
        }

        [HttpPost]
        public ActionResult Create(int Maphim, int danhgia, string Binhluan)
        {
            int Makh = ((KHACHHANG)Session["User"]).MaKh;
            Them(Maphim, Makh, danhgia, Binhluan);
            return RedirectToAction("Chitiet", "Movie", new { id = Maphim });
        }
        public void Them(int Maphim, int MaKh, int danhgia, string Binhluan)
        {
            COMMENT comment = new COMMENT();
            comment.Maphim = Maphim;
            comment.Makh = MaKh;
            comment.Binhluan = Binhluan;
            comment.Danhgia = danhgia;
            comment.thoigian = DateTime.Now;
            db.COMMENTs.InsertOnSubmit(comment);
            db.SubmitChanges();
        }
        public ActionResult DanhSachBinhLuan(int Maphim, int? page)
        {
            var binhLuan = db.COMMENTs.Where(c => c.Maphim == Maphim).OrderByDescending(c => c.thoigian).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 9;

           
            return View(binhLuan.ToPagedList(pageNumber, pageSize));
        }






    }

}