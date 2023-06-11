using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMovie.App_Start;
using WebMovie.Models;
using System.Data.SqlClient;
namespace WebMovie.Controllers
{
    public class HomeController : Controller
    {
        MovieDataDataContext data = new MovieDataDataContext();
        private string connectionString = "Data Source=DESKTOP-B14PQIH\\PHANDAT;Initial Catalog=Movie;Integrated Security=True;";

        public ActionResult Index()
        {
            return View();
        }
        [UserAuthorize]
        public ActionResult About()
        {
            return View(data.THEs.ToList().OrderBy(n => n.Mathe));
        }
        [UserAuthorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        // Phương thức tính tổng điểm trung bình của bộ phim với Maphim cụ thể


        public double? TinhDiemTrungBinh(int maphim)
        {
            double? diemTrungBinh = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT SUM(Danhgia) / COUNT(Danhgia) AS DiemTrungBinh " +
                               "FROM BINHLUAN " +
                               "WHERE Maphim = @Maphim";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Maphim", maphim);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    int danhGia = reader.GetInt32(0);
                                    diemTrungBinh = (double)danhGia;
                                }
                                else
                                {
                                    diemTrungBinh = 10.0;
                                }


                            }

                        }
                        else
                        {
                            diemTrungBinh = 10;
                        }

                    }
                }
            }

            return diemTrungBinh;
        }

    }

}
