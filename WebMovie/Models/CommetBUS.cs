using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMovie.Controllers;

namespace WebMovie.Models
{
    public class CommetBUS
    {
        public static void Them(int Maphim, int MaKh, int danhgia, string Binhluan)
        {
            using (var db = new MovieDataDataContext())
            {
                COMMENT comment = new COMMENT();
                comment.Maphim = Maphim;
                comment.Makh = MaKh;
                comment.Binhluan = Binhluan;
                comment.Danhgia = danhgia;
                comment.thoigian = DateTime.Now;
                db.COMMENTs.InsertOnSubmit(comment);
                db.SubmitChanges();
            }
        }
        
    }
}