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

        //comment cấp 1
        public ActionResult Index(int Maphim)
        {
            string hoten = ((KHACHHANG)Session["User"]).Hoten;
            ViewBag.Maphim = Maphim;
            //đếm só bình luận của phim
            int totalBinhLuan = db.BINHLUANs.Count(c => c.Maphim == Maphim);
            ViewBag.TotalBinhLuan = totalBinhLuan;
            ViewBag.Hoten = hoten;
           
            return View();
        }
        #region Comment cấp 1
        [HttpPost]
        public ActionResult Create(int Maphim, int danhgia, string Binhluan )
        {
      
            int Makh = ((KHACHHANG)Session["User"]).MaKh;
            Them(Maphim, Makh, danhgia, Binhluan);
            return RedirectToAction("Chitiet", "Movie", new { id = Maphim });
        }
        public void Them(int Maphim, int MaKh, int danhgia, string Binhluan)
        {
            BINHLUAN comment = new BINHLUAN();
            comment.Maphim = Maphim;
            comment.Makh = MaKh;
            comment.Noidung = Binhluan;
            comment.Danhgia = danhgia;
            comment.thoigian = DateTime.Now;
            db.BINHLUANs.InsertOnSubmit(comment);
            db.SubmitChanges();
        }
        public ActionResult DanhSachBinhLuan(int Maphim, int? page)
        {
            var binhLuan = db.BINHLUANs.Where(c => c.Maphim == Maphim && c.Macha == null).OrderByDescending(c => c.thoigian).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            return View(binhLuan.ToPagedList(pageNumber, pageSize));
        }
        #endregion
        //comment cấp 2
        #region COMMENT CẤP 2

        public ActionResult Reply(int Maphim, int Macha)
        {
            string hoten = ((KHACHHANG)Session["User"]).Hoten;
            int macha = ((KHACHHANG)Session["User"]).MaKh;
            ViewBag.Maphim = Maphim;
            //đếm só bình luận của phim
            int totalBinhLuan = db.BINHLUANs.Count(c => c.Maphim == Maphim);
            ViewBag.TotalBinhLuan = totalBinhLuan;
            ViewBag.Hoten = hoten;
            ViewBag.Macha = Macha;
            return View();
        }
        [HttpPost]
        public ActionResult CreateReply(int Maphim, int danhgia, string Binhluan,int macha)
        {

            int Makh = ((KHACHHANG)Session["User"]).MaKh;
            Phanhoi(Maphim, Makh, danhgia, Binhluan,macha);
            return RedirectToAction("Chitiet", "Movie", new { id = Maphim });
        }
        public void Phanhoi(int Maphim, int MaKh, int danhgia, string Binhluan,int macha)
        {
            BINHLUAN comment = new BINHLUAN();
            comment.Maphim = Maphim;
            comment.Makh = MaKh;
            comment.Noidung = Binhluan;
            comment.Danhgia = danhgia;
            comment.thoigian = DateTime.Now;
            comment.Macha= macha;
            db.BINHLUANs.InsertOnSubmit(comment);
            db.SubmitChanges();
        }
        public ActionResult DanhsachReplyBL(int maphim, int macha)
        {
            var binhluanReply = db.BINHLUANs.Where(c => c.Macha == macha && c.Maphim == maphim).ToList();
            return View(binhluanReply);
        }

        #endregion



    }

}