using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMovie.ViewModel
{
    public class TheloaiPhim
    {
        public int Maphim { get; set; }
        public string TenPhim { get; set; }
        public string Daodien { get; set; }
        public string Dienvien { get; set; }
        public string Noidung { get; set; }
        public string Dotuoi { get; set; }
        public string Thoiluong { get; set; }
        public string Ngonngu { get; set; }
        public string Linkphim { get; set; }
        public string Trailer { get; set; }
        
        public string Anhbia { get; set; }
        public bool Phimbo { get; set; }
        public Nullable<int> Phanphim { get; set; }
        public Nullable<int> Tapphim { get; set; }
        public List<string> TheLoai { get; set; }
        public List<string> Nam { get; set; }
        public List<string> Quociga { get; set; }
    }
}