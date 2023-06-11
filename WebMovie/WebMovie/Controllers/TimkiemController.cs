using PagedList;
using Raven.Abstractions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMovie.Models;

namespace WebMovie.Controllers
{
    public class TimkiemController : Controller
    {
        // GET: Timkiem
        MovieDataDataContext db = new MovieDataDataContext();
        public ActionResult KQTimkiem(string sTukhoa, int? page)
        {
            if (string.IsNullOrEmpty(sTukhoa))  // Kiểm tra nếu không có từ khóa tìm kiếm
            {
                return RedirectToAction("Index","Home");  // Chuyển hướng đến trang chủ hoặc trang tìm kiếm khác
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            // Tìm kiếm theo tên phim
            var listSP = db.PHIMs.Where(n => n.TenPhim.Contains(sTukhoa));

            ViewBag.Tukhoa = sTukhoa;

            IPagedList<PHIM> pagedList = listSP.OrderBy(n => n.TenPhim).ToPagedList(pageNumber, pageSize);

            if (pagedList != null && pagedList.Count > 0)
            {
                return View(pagedList);
            }
            else
            {
                return View(new PagedList<PHIM>(new List<PHIM>(), 1, pageSize));
            }
        }




        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTukhoa)
        {
            //goij ve ham get tim kiem
          
                return RedirectToAction("KQTimkiem", new { @sTukhoa = sTukhoa });
          
            
        }
    }
}