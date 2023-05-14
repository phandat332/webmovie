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
            return View(data.PHIMs.ToList().ToPagedList(pageNumber, pageSize));
        }
        // thêm mới phim

        //Chi tiết phim
        [AdminAuthorize]
        public ActionResult Chitietphim(int id)
        {
            PHIM phim = data.PHIMs.SingleOrDefault(n => n.Maphim == id);
            ViewBag.MaSP = phim.Maphim;
            if (phim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phim);
        }
      
        // Xóa phim5
        [AdminAuthorize]
        [HttpGet]
        public ActionResult Xoaphim(int id)
        {
            /*PHIM pm = data.PHIMs.SingleOrDefault(n => n.Maphim == id);
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
            return View(pm);*/
            PHIM phim = data.PHIMs.SingleOrDefault(n => n.Maphim == id);
            ViewBag.MaSP = phim.Maphim;
            if (phim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phim);

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
            ViewBag.Nam = new SelectList(data.NAMPHATHANHs.ToList().OrderBy(n => n.MaNam), "MaNam", "Nam", phim.MaNam);
            ViewBag.QuocGia = new SelectList(data.QUOCGIAs.ToList().OrderBy(n => n.MaQG), "MaQG", "TenQG", phim.MaQG);
            ViewBag.Theloai = new SelectList(data.THELOAIs.ToList().OrderBy(n => n.MaTL), "MaTL", "TenTL", phim.MaTL);
            return View(phim);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suaphim(PHIM phim , HttpPostedFileBase uploadhinh,int id)
        {
           ViewBag.Nam = new SelectList(data.NAMPHATHANHs.ToList().OrderBy(n => n.MaNam), "MaTH", "TenTH");
            ViewBag.QuocGia = new SelectList(data.QUOCGIAs.ToList().OrderBy(n => n.MaQG), "MaQG", "TenQG");
            ViewBag.Theloai = new SelectList(data.THELOAIs.ToList().OrderBy(n => n.MaTL), "MaTL", "TenTL");
            PHIM sp = data.PHIMs.FirstOrDefault(p => p.Maphim == phim.Maphim || p.Maphim == id);
            sp.TenPhim = phim.TenPhim;
            sp.Noidung = phim.Noidung;

            if (uploadhinh != null && uploadhinh.ContentLength > 0)
            {
              /*  int id = phim.Maphim;*/

                string _FileName = "";
                int index = uploadhinh.FileName.IndexOf('.');
                _FileName = "Suaphim" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/image/"), _FileName);
                uploadhinh.SaveAs(_path);
                sp.Anhbia = _FileName;
            }

            sp.Dotuoi = phim.Dotuoi;
            sp.Daodien = phim.Daodien;
            sp.Dienvien = phim.Dienvien;
            sp.Thoiluong = phim.Thoiluong;
            sp.Ngonngu = phim.Ngonngu;
            sp.Phanphim = phim.Phanphim;
            sp.Tapphim = phim.Tapphim;
            sp.Trailer= phim.Trailer;
            sp.MaNam = phim.MaNam;
            sp.MaQG= phim.MaQG ;
            UpdateModel(sp);
            data.SubmitChanges();
            return RedirectToAction("QLPhim");

        }
        [AdminAuthorize]
        [HttpGet]
        public ActionResult Themphim()
        {
            ViewBag.MaNam = new SelectList(data.NAMPHATHANHs.ToList().OrderBy(n => n.Nam), "MaNam", "Nam");
            ViewBag.MaQG = new SelectList(data.QUOCGIAs.ToList().OrderBy(n => n.TenQG), "MaQG", "TenQG");
            ViewBag.MaTL = new SelectList(data.THELOAIs.ToList().OrderBy(n => n.MaTL), "MaTL", "TenTL");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themphim(PHIM phim, HttpPostedFileBase uploadhinh)
        {
            data.PHIMs.InsertOnSubmit(phim);
            data.SubmitChanges();
            if (uploadhinh != null && uploadhinh.ContentLength > 0)
            {
                int id = int.Parse(data.PHIMs.ToList().Last().Maphim.ToString());

                string _FileName = "";
                int index = uploadhinh.FileName.IndexOf('.');
                _FileName = "themsp" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/image/"), _FileName);
                uploadhinh.SaveAs(_path);

                PHIM unv = data.PHIMs.FirstOrDefault(x => x.Maphim == id);
                unv.Anhbia = _FileName;
                data.SubmitChanges();
            }
            return RedirectToAction("QLPhim");

        }
    }
   
}