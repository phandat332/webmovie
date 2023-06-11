using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMovie.Models
{
    public class Giohang
    {

        MovieDataDataContext data = new MovieDataDataContext();

        public int Mathe { get; set; }

        public string LoaiThe { get; set; }

        public string ChatLuong { get; set; }
        public string PhanGiai { get; set; }

        public int giaban { get; set; }

        public int soluong { get; set; }
        public double dThanhTIen
        {
            get { return soluong * giaban; }
        }

        public Giohang(int id)
        {
            Mathe = id;
            THE the = data.THEs.Single(n => n.Mathe == Mathe);
            LoaiThe = the.LoaiThe;
            giaban = (int)double.Parse(the.GiaTien.ToString());

            soluong = 1;
        }
    }
}