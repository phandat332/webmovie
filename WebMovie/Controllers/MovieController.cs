using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMovie.App_Start;
using WebMovie.Models;
using WebMovie.ViewModel;
using System.Data.Entity;
using System.Data.Linq;
using System;
using System.Web;

namespace WebMovie.Controllers
{
    public class MovieController : Controller
    {
        MovieDataDataContext data = new MovieDataDataContext();
        private HomeController homeController = new HomeController();
        // GET: movie
        public ActionResult Index()
        {
            return View();
        }
        [UserAuthorize]
        public ActionResult Chitiet(int id)
         {

            int maphim = id; 
            double? trungBinh = homeController.TinhDiemTrungBinh(maphim);
            ViewBag.trungbinh = trungBinh;
            var chitiet = from s in data.PHIMs
                          where s.Maphim == id
                          select s;
            return View(chitiet.Single());
        }

        // xem phim
        [UserAuthorize]
        public ActionResult Xemphim(int id)
        {
       
            int maKhachHang = ((KHACHHANG)System.Web.HttpContext.Current.Session["User"]).MaKh;
            bool daXem = CheckMovie(maKhachHang, id);
            if (!daXem)
            {
                // Ghi nhận lịch sử xem phim nếu chưa xem
                Themlichsu(maKhachHang, id);
            }
            // Lấy thông tin phim từ cơ sở dữ liệu
            var Xemphim = data.PHIMs.Single(p => p.Maphim == id);
            return View(Xemphim);
        }
        //check phim đã xem hay chưa
        public bool CheckMovie(int maKhachHang, int maPhim)
        {
            using (var dbContext = new MovieDataDataContext())
            {
                var lichSu = dbContext.LICHSUs.SingleOrDefault(l => l.Makh == maKhachHang && l.Maphim == maPhim);

                return lichSu != null;
            }
        }
        //lịch sử người dùng
        public void Themlichsu(int maKhachHang, int maPhim)
        {
            LICHSU lichSuXemPhim = new LICHSU
            {
                Makh = maKhachHang,
                Maphim = maPhim,
                Thoigian = DateTime.Now
            };

            data.LICHSUs.InsertOnSubmit(lichSuXemPhim);
            data.SubmitChanges();
        }
        public ActionResult Theloai()
        {
            var theloai = from tl in data.THELOAIs select tl;
            return PartialView(theloai);
        }
        #region sản phẩm theo thể loại
        public ActionResult Theloais()
        {
            var theloai = from tl in data.THELOAIs select tl;
            return PartialView(theloai);
        }
        public ActionResult SPTheotheloai(int id, int? page)
        {
            /*// kiem tra thuong hieu co ton tai khong
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            THELOAI danhmuc = data.THELOAIs.SingleOrDefault(n => n.MaTL == id);
            if (danhmuc == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            //truy xuat danh sach theo thuong hieu
             List<PHIM> listphim = (
                 from p in data.PHIMs
                 join pt in data.PHIMTHELOAIs on p.Maphim equals pt.Maphim
                 where pt.MaTL == id
                 select p
             ).ToList();
              List<PHIM> listphim = data.PHIMs.Where(n => n.Maphim == id).ToList();

            if (listphim.Count == 0)
            {
                // không làm gì cả
            }
            else
            {
               
                int i = listphim.Count;
                ViewBag.soluong = i;
                ViewBag.theloai = danhmuc.TenTL;
            }
            return View(listphim.ToPagedList(pageNumber, pageSize));*/
            // Kiểm tra xem 'data' có được khởi tạo hay không
            if (data == null)
            {
                // Xử lý lỗi ở đây
                return null;
            }

            // Kiểm tra thương hiệu có tồn tại không
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            THELOAI theloai = data.THELOAIs.SingleOrDefault(n => n.MaTL == id);
            if (theloai == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Truy xuất danh sách
            List<PHIM> listphim = data.PHIMs.Where(n => n.MaTL == id).ToList();
            if (listphim.Count == 0)
            {
                // Không có phim nào
                ViewBag.soluong = 0;
            }
            else
            {
                // Có phim
                ViewBag.soluong = listphim.Count;
            }
            ViewBag.Theloai = theloai.TenTL;
            return View(listphim.ToPagedList(pageNumber, pageSize));
        }
        #endregion


        //  phim theo thể loại thể loại
        public ActionResult phimloai(int id,int? page)
        {
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            THELOAI theloai = data.THELOAIs.SingleOrDefault(n => n.MaTL == id);
            if (theloai == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Truy xuất danh sách
            List<PHIM> listphim = data.PHIMs.Where(n => n.MaTL == id).ToList();
            if (listphim.Count == 0)
            {
                // Không có phim nào
                ViewBag.soluong = 0;
            }
            else
            {
                // Có phim
                ViewBag.soluong = listphim.Count;
            }
            ViewBag.Theloai = theloai.TenTL;
            return View(listphim.ToPagedList(pageNumber, pageSize));
        }

        #region sp theo nam phát hành
        //sản phẩm theo năm phát hành
        public ActionResult Namph()
        {
            var Namph = from tl in data.NAMPHATHANHs select tl;
            return PartialView(Namph);
        }

        public ActionResult SPtheoNamph(int id, int? page)
        {
            // Kiểm tra xem 'data' có được khởi tạo hay không
            if (data == null)
            {
                // Xử lý lỗi ở đây
                return null;
            }

            // Kiểm tra thể loại có tồn tại không
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            NAMPHATHANH Namph = data.NAMPHATHANHs.SingleOrDefault(n => n.MaNam == id);
            if (Namph == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Truy xuất danh sách
            List<PHIM> listphim = data.PHIMs.Where(n => n.MaNam == id).ToList();
            if (listphim.Count == 0)
            {
                // Không có phim nào
                ViewBag.soluong = 0;
            }
            else
            {
                // Có phim
                ViewBag.soluong = listphim.Count;
            }
            ViewBag.Nam = Namph.Nam;
            return View(listphim.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        // phim lẻ
        public ActionResult Phimle(int ? page) 
        {
            int pageNumber = (page ?? 1);
            int pageSize = 12;
            return View(data.PHIMs.Where(n => n.Phimbo == false).ToList().ToPagedList(pageNumber, pageSize));
        }
        //sản phẩm theo quôc gia
        #region sp theo quốc gia
        public ActionResult quocgia()
        {
            var quocgia = from tl in data.QUOCGIAs select tl;
            return PartialView(quocgia);
        }

        public ActionResult SPtheoquocgia(int id, int? page)
        {
            // Kiểm tra xem 'data' có được khởi tạo hay không
            if (data == null)
            {
                // Xử lý lỗi ở đây
                return null;
            }

            // Kiểm tra thương hiệu có tồn tại không
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            QUOCGIA quocgia = data.QUOCGIAs.SingleOrDefault(n => n.MaQG == id);
            if (quocgia == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Truy xuất danh sách
            List<PHIM> listphim = data.PHIMs.Where(n => n.MaQG == id).ToList();
            if (listphim.Count == 0)
            {
                // Không có phim nào
                ViewBag.soluong = 0;
            }
            else
            {
                // Có phim
                ViewBag.soluong = listphim.Count;
            }
            ViewBag.quocgia = quocgia.TenQG;
            return View(listphim.ToPagedList(pageNumber, pageSize));
        }
        #endregion
    }
}