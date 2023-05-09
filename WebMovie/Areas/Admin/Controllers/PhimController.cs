using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebMovie.App_Start;
using WebMovie.Models;
using PagedList;
using WebMovie.ViewModel;
using Raven.Database.Storage;
using System.Drawing;
using System.IO;

namespace WebMovie.Areas.Admin.Controllers
{
    public class PhimController : Controller
    {
        MovieDataDataContext data = new MovieDataDataContext();
        // GET: Admin/Phim
        [AdminAuthorize]
        public ActionResult QLPhim(int? page)
        {

            int pageNumber = (page ?? 1);
            int pageSize = 6;
           /* var model = from p in data.PHIMs
                        join pt in data.PHIMTHELOAIs on p.Maphim equals pt.Maphim
                        join tl in data.THELOAIs on pt.MaTL equals tl.MaTL
                        select new { Phim = p, TenTheLoai = tl.TenTL };

            ViewBag.Theloai = model;*/

            return View(data.PHIMs.ToList().ToPagedList(pageNumber, pageSize));
        }
        // thêm mới phim

        //Chi tiết phim
        [AdminAuthorize]
        public ActionResult Chitietphim(int id)
        {
            var phim = (from p in data.PHIMs
                        where p.Maphim == id
                        select new TheloaiPhim
                        {
                            Maphim = p.Maphim,
                            TenPhim = p.TenPhim,
                            Daodien = p.Daodien,
                            Dienvien = p.Dienvien,
                            Noidung = p.Noidung,
                            Dotuoi = p.Dotuoi,
                            Thoiluong = p.Thoiluong,
                            Ngonngu = p.Ngonngu,
                            Linkphim = p.Linkphim,
                            Trailer = p.Trailer,
                            Anhbia = p.Anhbia,
                            Phimbo = (bool)p.Phimbo,
                            Phanphim = p.Phanphim,
                            Tapphim = p.Tapphim,
                            TheLoai = (from tl in data.THELOAIs
                                       join ct in data.PHIMTHELOAIs on tl.MaTL equals ct.MaTL
                                       where ct.Maphim == id
                                       select tl.TenTL).ToList(),
                            Nam = (from n in data.NAMPHATHANHs
                                   where n.MaNam == p.MaNam
                                   select n.Nam).ToList(),
                            Quociga = (from n in data.QUOCGIAs
                                       where n.MaQG == p.MaQG
                                       select n.TenQG).ToList()

                        }).FirstOrDefault();

            if (phim == null)
            {
                return HttpNotFound();
            }
            if(phim.Phimbo == false)
            {
                ViewData["Phanphim"] = "Trống";
                ViewData["Tapphim"] = "Trống";

            }
            return View(phim);


        }
      
        // Xóa phim5
        [AdminAuthorize]
        [HttpGet]
        public ActionResult Xoaphim(int id)
        {
            PHIM pm = data.PHIMs.SingleOrDefault(n => n.Maphim == id);
            var dsTheLoai = (
            from p in data.PHIMs
            join pt in data.PHIMTHELOAIs on p.Maphim equals pt.Maphim
            join tl in data.THELOAIs on pt.MaTL equals tl.MaTL
            where p.Maphim == id
            select tl.TenTL
           ).ToList();
            if (pm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewData["Theloai"] = dsTheLoai;

            ViewBag.MaKh = pm.Maphim;
            return View(pm);
        }
        [HttpPost, ActionName("Xoaphim")]
        public ActionResult Upxoaphim(int id)
        {
            PHIM pm = data.PHIMs.SingleOrDefault(n => n.Maphim == id);
            if (pm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.PHIMs.DeleteOnSubmit(pm);
            data.SubmitChanges();
            return RedirectToAction("QLPhim");

        }
        // quản lý thể loại phim
        [AdminAuthorize]
        [HttpGet]
        public ActionResult Suaphim(int id)
        {
            PHIM phim = data.PHIMs.SingleOrDefault(n => n.Maphim == id);
            if (phim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phim);
        }

        [HttpPost, ActionName("Suaphim")]
        [ValidateInput(false)]
        public ActionResult Saved(FormCollection collection, int id)
        {
            PHIM tk = data.PHIMs.SingleOrDefault(n => n.Maphim == id);
       
            UpdateModel(tk);
            data.SubmitChanges();
            return RedirectToAction("QLPhim");
        }
        [AdminAuthorize]
        [HttpGet]
        public ActionResult Themphim()
        {
            ViewBag.MaTH = new SelectList(data.NAMPHATHANHs.ToList().OrderBy(n => n.Nam), "MaNam", "Nam");
            ViewBag.MaDM = new SelectList(data.QUOCGIAs.ToList().OrderBy(n => n.TenQG), "MaQG", "TenQG");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themphim(PHIM sanpham, HttpPostedFileBase uploadhinh)
        {

            data.PHIMs.InsertOnSubmit(sanpham);
            data.SubmitChanges();
            if (uploadhinh != null && uploadhinh.ContentLength > 0)
            {
                int id = int.Parse(data.PHIMs.ToList().Last().Maphim.ToString());

                string _FileName = "";
                int index = uploadhinh.FileName.IndexOf('.');
                _FileName = "themsp" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/Content/img"), _FileName);
                uploadhinh.SaveAs(_path);

                PHIM unv = data.PHIMs.FirstOrDefault(x => x.Maphim == id);
                unv.Anhbia = _FileName;
                data.SubmitChanges();
            }
            return RedirectToAction("QLSanpham");

        }
    }
   
}