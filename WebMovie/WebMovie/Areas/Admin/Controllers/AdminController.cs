using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMovie.Models;
using PagedList;
using PagedList.Mvc;
using WebMovie.App_Start;
using WebMovie.ViewModel;
using System.Security.Cryptography;
using System.Text;

namespace WebMovie.Areas.Admin.Controllers
{
   
    public class AdminController : Controller
    {
         MovieDataDataContext data = new MovieDataDataContext();
        [AdminAuthorize]
        public ActionResult Index()
        {
            List<NAMPHATHANH> ph = data.NAMPHATHANHs.ToList();
            int n = ph.Count();
            List<THELOAI> tl = data.THELOAIs.ToList();
            int t = tl.Count();
            List<QUOCGIA> qg = data.QUOCGIAs.ToList();
            int q = qg.Count();
            List<PHIM> phim = data.PHIMs.ToList();
            int p = phim.Count();
            List<KHACHHANG> User = data.KHACHHANGs.Where(m => m.MaQuyen == 0).ToList();
            List<KHACHHANG> Admin = data.KHACHHANGs.Where(m => m.MaQuyen == 1).ToList();
            ViewBag.nam = n;
            ViewBag.theloai = t;
            ViewBag.quocgia = q;
            ViewBag.khachhang = User.Count();
            ViewBag.Admin = Admin.Count();
            ViewBag.phim = p;
            return View();
        }
        [HttpGet]
        //mã hóa mật khẩu 
      
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
            //mã hóa ở đây lúc đăng kí, nên đăng nhặp cg phải mã hóa ngược, ok chưa

        }
        ///quản lý admin
        [HttpGet]
        #region QUẢN LÝ THÔNG TIN ADMIN
        [AdminAuthorize]
        public ActionResult QLAdmin(int? page)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 4;
                return View(data.KHACHHANGs.Where(n => n.MaQuyen == 1).ToList().ToPagedList(pageNumber, pageSize));
            }
        [AdminAuthorize]
        [HttpGet]
        public ActionResult Themad()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Themad(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HoTen"];
            var tendn = collection["Taikhoan"];
            var matkhau = collection["MatKhau"];
            var matkhau2 = collection["MatKhau2"];
            var email = collection["Email"];
            var sdt = collection["Dienthoaikh"];

            KHACHHANG check = data.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn);

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống!";
            }
            else if (String.IsNullOrEmpty(tendn) || tendn.Count() < 6)
            {
                ViewData["LoiCountTK"] = "Vui lòng nhập tên tài khoản dài hơn 6 ký tự!";
            }
            else if (check != null)
            {
                ViewData["LoiTrungTK"] = "Tài khoản đã được sử dụng!";
            }
            else if (String.IsNullOrEmpty(matkhau) || matkhau.Count() < 6)
            {
                ViewData["LoiCountMK"] = "Vui lòng nhập mật khẩu dài hơn 6 ký tự!";
            }
            else if (String.IsNullOrEmpty(matkhau2))
            {
                ViewData["Loi4"] = "Vui lòng nhập lại mật khẩu!";
            }
            else if (matkhau != matkhau2)
            {
                ViewData["LoiTrungMK"] = "Mật khẩu không khớp!";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi6"] = "Vui lòng nhập email!";
            }
            else if (String.IsNullOrEmpty(sdt))
            {
                ViewData["Loi7"] = "Vui lòng nhập số điện thoại!";
            }
            else
            {
                kh.Hoten = hoten;
                kh.Email = email;
                kh.DienthoaiKh = sdt;
                kh.Taikhoan = tendn;
                kh.Matkhau = MD5Hash(matkhau);
                kh.MaQuyen = 1;

                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("QLAdmin");
            }

            return Themad();
        }
        // SỬA ADMIN
        [AdminAuthorize]
        [HttpGet]
        public ActionResult Suaad(int id)
        {
            KHACHHANG tk = data.KHACHHANGs.SingleOrDefault(n => n.MaKh == id);
            if (tk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tk);
        }

        [HttpPost, ActionName("Suaad")]
        [ValidateInput(false)]
        public ActionResult Saved(FormCollection collection, int id)
        {
            KHACHHANG tk = data.KHACHHANGs.SingleOrDefault(n => n.MaKh == id);
            var Matkhau1 = collection["Matkhau1"];
            var Matkhau2 = collection["Matkhau2"];
            if(String.IsNullOrEmpty(Matkhau1) )
            {
                //không làm gì cả 
            }             
            else if (tk.Matkhau != MD5Hash(Matkhau1))
            {
                tk.Matkhau = MD5Hash(Matkhau1);
                UpdateModel(tk.Matkhau);

            }
            UpdateModel(tk);      
            data.SubmitChanges();
            return RedirectToAction("QLAdmin");
        }


        //XÓA ADMIN
        [AdminAuthorize]
        [HttpGet]
        public ActionResult Xoaad(int id)
        {
            KHACHHANG tk = data.KHACHHANGs.Where(n => n.MaQuyen == 1).SingleOrDefault(n => n.MaKh == id);
            if (tk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaKh = tk.MaKh;
            return View(tk);
        }
        [HttpPost, ActionName("Xoaad")]
        public ActionResult Upxoaad(int id)
        {
            KHACHHANG tk = data.KHACHHANGs.SingleOrDefault(n => n.MaKh == id);
            if (tk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaKh = tk.MaKh;
            data.KHACHHANGs.DeleteOnSubmit(tk);
            data.SubmitChanges();
            return RedirectToAction("QLAdmin");
        }
        //Chi tiết admin
        [AdminAuthorize]     
             public ActionResult Chitietad(int id)
             {
                 KHACHHANG kh = data.KHACHHANGs.Where(n => n.MaQuyen == 1).SingleOrDefault(n => n.MaKh == id);
                 if (kh == null)
                 {
                     Response.StatusCode = 404;
                     return null;
                 }
                 ViewBag.MAKH = kh.MaKh;
                 return View(kh);
             }

        #endregion



    }
}