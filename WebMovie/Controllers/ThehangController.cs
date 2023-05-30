
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMovie.Models;

namespace WebMovie.Controllers
{
    public class ThehangController : Controller
    {
        MovieDataDataContext data = new MovieDataDataContext();
        public List<Giohang> Laygiohang()
        {
         
            List<Giohang> lstgiohang = Session["Giohang"] as List<Giohang>;
            if (lstgiohang == null)
            {
                lstgiohang = new List<Giohang>();
                Session["Giohang"] = lstgiohang;
            }

            return lstgiohang;
        }
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoluongSanPham();
            Session["TongSoSp"] = TongSoluongSanPham() + "";
            return View(lstGiohang);
        }
        // GET: Giohang

        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<Giohang> lstgiohang = Laygiohang();
            Giohang sanpham = lstgiohang.Find(n => n.Mathe == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                lstgiohang.Add(sanpham);
            }
            else
            {
                sanpham.soluong++;
            }

            Session["TongSoSp"] = TongSoluongSanPham() + "";

            // Chuyển hướng trực tiếp đến trang giỏ hàng
            return RedirectToAction("Giohang", "Thehang");

            // Thay thế "TenHanhDongGiỏHàng" bằng tên hành động trong Controller của bạn để hiển thị trang giỏ hàng.
            // Thay thế "TenController" bằng tên Controller của bạn.
        }

        private int TongSoluongSanPham()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            Session["TongSoSp"] = tsl + "";
            return tsl;
        }
        private int TongSoluong()
        {
            int tsl = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.soluong);
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.dThanhTIen);
            }
            return tt;
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoluongSanPham();
            Session["TongSoSp"] = TongSoluongSanPham() + "";
            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.Mathe == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.Mathe == id);
                return RedirectToAction("Giohang");
            }
            return RedirectToAction("About","Home");
        }
        public ActionResult Xoatatcagiohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("About", "Home");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("About", "Home");
            }

            List<Giohang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoluongSanPham();

            return View(lstGioHang);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DONHANG dh = new DONHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaikhoanCart"];

            List<Giohang> gh = Laygiohang();

            dh.Makh = kh.MaKh;
            dh.NgayDH = DateTime.Now;
            dh.Trangthai = false;

            data.DONHANGs.InsertOnSubmit(dh);
            data.SubmitChanges();

            foreach (var item in gh)
            {
                CHITIETDONHANG ctdh = new CHITIETDONHANG();
                ctdh.MaDH = dh.MaDH;
                ctdh.Mathe = item.Mathe;
                ctdh.Soluong = item.soluong;
                ctdh.Dongia = (decimal)item.giaban;

                data.CHITIETDONHANGs.InsertOnSubmit(ctdh);
            }

            data.SubmitChanges();
            Session["GioHang"] = null;

            return RedirectToAction("XacNhanDonHang", "Thehang");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }

        //public ActionResult Payment()
        //{
        //    //request params need to request to MoMo system
        //    List<Giohang> giohangs = Session["GioHang"] as List<Giohang>;
        //    string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
        //    string partnerCode = ConfigurationManager.AppSettings["partnerCode"].ToString();
        //    string accessKey = ConfigurationManager.AppSettings["accessKey"].ToString();
        //    string serectKey = ConfigurationManager.AppSettings["serectKey"].ToString();
        //    string orderInfo = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");
        //    string returnUrl = ConfigurationManager.AppSettings["returnUrl"].ToString();
        //    string notifyurl = ConfigurationManager.AppSettings["notifyurl"].ToString();

        //    string amount = giohangs.Sum(n => n.dThanhTIen).ToString();
        //    string orderid = Guid.NewGuid().ToString();
        //    string requestId = Guid.NewGuid().ToString();
        //    string extraData = "";

        //    //Before sign HMAC SHA256 signature
        //    string rawHash = "partnerCode=" +
        //        partnerCode + "&accessKey=" +
        //        accessKey + "&requestId=" +
        //        requestId + "&amount=" +
        //        amount + "&orderId=" +
        //        orderid + "&orderInfo=" +
        //        orderInfo + "&returnUrl=" +
        //        returnUrl + "&notifyUrl=" +
        //        notifyurl + "&extraData=" +
        //        extraData;

        //    MoMoSecurity crypto = new MoMoSecurity();
        //    //sign signature SHA256
        //    string signature = crypto.signSHA256(rawHash, serectKey);

        //    //build body json request
        //    JObject message = new JObject
        //    {
        //        { "partnerCode", partnerCode },
        //        { "accessKey", accessKey },
        //        { "requestId", requestId },
        //        { "amount", amount },
        //        { "orderId", orderid },
        //        { "orderInfo", orderInfo },
        //        { "returnUrl", returnUrl },
        //        { "notifyUrl", notifyurl },
        //        { "extraData", extraData },
        //        { "requestType", "captureMoMoWallet" },
        //        { "signature", signature }

        //    };

        //    string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

        //    JObject jmessage = JObject.Parse(responseFromMomo);

        //    return Redirect(jmessage.GetValue("payUrl").ToString());
        //}

        ////Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
        ////errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
        ////Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i
        //public ActionResult ConfirmPaymentClient(Result result)
        //{
        //    //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
        //    string rMessage = result.message;
        //    string rOrderId = result.orderId;
        //    string rErrorCode = result.errorCode; // = 0: thanh toán thành công
        //    return View();
        //}

        //[HttpPost]
        //public void SavePayment()
        //{
        //    //cập nhật dữ liệu vào db
        //    String a = "";
        //}
    }
}