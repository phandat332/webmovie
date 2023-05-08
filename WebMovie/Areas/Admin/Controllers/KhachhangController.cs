using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebMovie.Models;
using WebMovie.App_Start;
using System.Security.Cryptography;
using System.Text;

namespace WebMovie.Areas.Admin.Controllers
{
    public class KhachhangController : Controller
    {
        MovieDataDataContext data = new MovieDataDataContext();
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
       

        }
        // GET: Admin/Khachhang
        [AdminAuthorize]
        public ActionResult QLKhachhang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.KHACHHANGs.Where(n => n.MaQuyen == 0).ToList().ToPagedList(pageNumber, pageSize));
            
        }
        // SỬA khách hàng

        [AdminAuthorize]
        [HttpGet]
        public ActionResult Suakh(int id)
        {
            KHACHHANG tk = data.KHACHHANGs.SingleOrDefault(n => n.MaKh == id);
            if (tk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tk);
        }

        [HttpPost, ActionName("Suakh")]
        [ValidateInput(false)]
        public ActionResult Save(FormCollection collection, int id)
        {
            KHACHHANG tk = data.KHACHHANGs.SingleOrDefault(n => n.MaKh == id);
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
            data.SubmitChanges();
            return RedirectToAction("QLKhachhang");
        }


        //XÓA khách hàng
        [AdminAuthorize]
        [HttpGet]
        public ActionResult Xoakh(int id)
        {
            KHACHHANG tk = data.KHACHHANGs.Where(n => n.MaQuyen == 0).SingleOrDefault(n => n.MaKh == id);
            if (tk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaKh = tk.MaKh;
            return View(tk);
        }
        [HttpPost, ActionName("Xoakh")]
        public ActionResult Upxoakh(int id)
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
            return RedirectToAction("QLKhachhang");
        }
        //Chi tiết khách hàng
        [AdminAuthorize]
        public ActionResult Chitietkh(int id)
        {
            KHACHHANG kh = data.KHACHHANGs.Where(n => n.MaQuyen == 0).SingleOrDefault(n => n.MaKh == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MAKH = kh.MaKh;
            return View(kh);
        }

    }
}