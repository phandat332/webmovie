﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using WebMovie.App_Start;
using WebMovie.Models;
using WebMovie.ViewModel;
using System.Data.Linq;

namespace WebMovie.Controllers
{
    public class AccountController : Controller
    {
        MovieDataDataContext db = new MovieDataDataContext();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
     
        [HttpGet]
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

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection, loginVn lg)
        {
            var tendn = collection["Taikhoan"];
            var matkhau = collection["Matkhau"];
            System.Diagnostics.Debug.WriteLine(tendn);
            System.Diagnostics.Debug.WriteLine(matkhau);
            if (String.IsNullOrEmpty(tendn) && String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi"] = "Vui lòng nhập tài khoản và mật khẩu!";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Vui lòng nhập tài khoản!";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu!";
            }
            else
            {
                KHACHHANG tk = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == MD5Hash(matkhau));
                KHACHHANG tkCheck = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.MaQuyen == 1);
                KHACHHANG User = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.MaQuyen == 0);
                
                Session["User"] = User;
                Session["Admin"] = tkCheck;
                if (tk == null)
                {
                    ViewBag.checkTK = "Tài khoản chưa tồn tại!";
                }
                else if (tkCheck != null)
                {
                    Session["Taikhoan"] = tendn;
                    Session["Tendangnhap"] = tkCheck.Hoten.ToString();
                    Session["Makh"] = tkCheck.MaKh;
                    Session.Timeout = 500000;
                    return RedirectToAction("Index", "Admin", new { area = "Admin" });
                }
                else if (tk != null)
                {
                    Session["Taikhoan"] = tk.Hoten;
                    Session["TaikhoanCart"] = tk;
                    Session.Timeout = 500000;
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    ViewData["ThongBao"] = "Mật khẩu không chính xác!";
                }
            }
            TempData["Message"] = "bạn đã mua thẻ tháng chưa? hãy mua thẻ tháng để có trả nghiệm tốt hơn nhé";

            return View();
        }

        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HoTen"];
            var tendn = collection["Taikhoan"];
            var matkhau = collection["MatKhau"];
            var matkhau2 = collection["MatKhau2"];
            var email = collection["Email"];
            var sdt = collection["Dienthoaikh"];

            KHACHHANG check = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn);

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
                kh.MaQuyen = 0;

                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("About","Home");
            }

            return DangKy();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        // thay đổi thông tin
        [UserAuthorize]
        [HttpGet]
             public ActionResult Chitiet()
            {
                KHACHHANG Makh = (KHACHHANG)Session["User"];

                    return View(Makh);
           
            }



        [UserAuthorize]
        [HttpGet]
        public ActionResult EditKh(int id)
        {
            KHACHHANG tk = db.KHACHHANGs.SingleOrDefault(n => n.MaKh == id);
            if (tk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tk);
        }

        [HttpPost, ActionName("EditKh")]
        [ValidateInput(false)]
        public ActionResult Saved(FormCollection collection, int id)
        {
            KHACHHANG tk = db.KHACHHANGs.SingleOrDefault(n => n.MaKh == id);
            var Matkhau1 = collection["Matkhau1"];
            var Matkhau2 = collection["Matkhau2"];
            if (String.IsNullOrEmpty(Matkhau1))
            {
                //không làm gì cả 
            }
            else if (tk.Matkhau != MD5Hash(Matkhau1))
            {
                tk.Matkhau = MD5Hash(Matkhau1);
                UpdateModel(tk.Matkhau);

            }
            UpdateModel(tk);
            db.SubmitChanges();
            return RedirectToAction("Login");
        }


        [UserAuthorize]
        public ActionResult LichSuXemPhim()
        {
            int Makh = ((KHACHHANG)Session["User"]).MaKh;
            var lichSuXemPhim = from p in db.PHIMs
                                join l in db.LICHSUs on p.Maphim equals l.Maphim
                                where l.Makh == Makh
                                orderby l.Thoigian descending
                                select p;

            return View(lichSuXemPhim);
        }
    }
}