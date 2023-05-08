using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebMovie.Models;
using WebMovie.ViewModel;


namespace WebMovie.Controllers
{
    public class MovieController : Controller
    {
        MovieDataDataContext data = new MovieDataDataContext();
        
        // GET: movie
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Chitiet(int id)
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
                            Phanphim =p.Phanphim,
                            Tapphim = p.Tapphim,
                            TheLoai = (from tl in data.THELOAIs
                                       join ct in data.PHIMTHELOAIs on tl.MaTL equals ct.MaTL
                                       where ct.Maphim == id
                                       select tl.TenTL).ToList(),
                            Nam = (from n in data.NAMPHATHANHs
                                   where n.MaNam == p.MaNam
                                   select n.Nam).ToList(),
                            Quociga = (from n in data.QUOCGIAs
                                       where n.MaQG  == p.MaQG
                                       select n.TenQG).ToList()

                        }).FirstOrDefault();

            if (phim == null)
            {
                return HttpNotFound();
            }

            return View(phim);


        }
        // xem phim
        public ActionResult Xemphim(int id)
        {
            var Xemphim = from s in data.PHIMs
                          where s.Maphim == id
                          select s;
            return View(Xemphim.Single());
        }
        public ActionResult Theloai()
        {
            var theloai = from tl in data.THELOAIs select tl;
            return PartialView(theloai);
        }
        public ActionResult SPTheotheloai(int id, int? page)
        {
            // kiem tra thuong hieu co ton tai khong
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
            return View(listphim.ToPagedList(pageNumber, pageSize));
        }
        // phim theo thể loại hành động
        public ActionResult SanPhamTheoHanhdong( int? page)
        {
            // kiem tra thuong hieu co ton tai khong
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            //truy xuat danh sach theo thuong hieu
            List<PHIM> listphim = data.PHIMs
                .Join(data.PHIMTHELOAIs, p => p.Maphim, pt => pt.Maphim, (p, pt) => new { Phim = p, PhimTheLoai = pt })
                .Join(data.THELOAIs, ppt => ppt.PhimTheLoai.MaTL, tl => tl.MaTL, (ppt, tl) => new { Phim = ppt.Phim, TheLoai = tl })
                .Where(t => t.TheLoai.TenTL == "Hành động")
                .Select(t => t.Phim)
                .ToList(); 

            return View(listphim.ToPagedList(pageNumber, pageSize));
        }
        //sản phẩm theo năm phát hành
        public ActionResult Namph()
        {
            var Namph = from tl in data.NAMPHATHANHs select tl;
            return PartialView(Namph);
        }
        public ActionResult SPtheoNamph(int id, int? page)
        {
            // kiem tra thuong hieu co ton tai khong
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            NAMPHATHANH Namph = data.NAMPHATHANHs.SingleOrDefault(n => n.MaNam == id);
            if (Namph == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //truy xuat danh sach theo thuong hieu
            List<PHIM> listphim = data.PHIMs.Where(n => n.Maphim  == id).ToList();
            if (listphim.Count == 0)
            {
                // không làm gì cả
            }
            else
            {
                int i;
                for (i = 0; i < listphim.Count; i++)
                {
                    i = i++;
                }
                ViewBag.soluong = i;
            }
            return View(listphim.ToPagedList(pageNumber, pageSize));
        }
        // phim lẻ
        public ActionResult Phimle(int ? page) 
        {
            int pageNumber = (page ?? 1);
            int pageSize = 8;
            return View(data.PHIMs.Where(n => n.Phimbo == false).ToList().ToPagedList(pageNumber, pageSize));
        }
    }
}