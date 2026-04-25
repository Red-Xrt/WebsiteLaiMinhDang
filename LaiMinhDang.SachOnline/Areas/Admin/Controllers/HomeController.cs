using LaiMinhDang.SachOnline.Models;
using System.Linq;
using System.Web.Mvc;

namespace LaiMinhDang.SachOnline.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();

        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var soLuongDonHang = db.DONDATHANGs.Count();

            // Chỉ tính doanh thu cho các đơn hàng ĐÃ GIAO HÀNG và ĐÃ THANH TOÁN
            var danhSachDonHangThanhCong = db.DONDATHANGs
                .Where(d => d.TinhTrangGiaoHang == 1 && d.DaThanhToan == true)
                .Select(d => d.MaDonHang).ToList();

            var tongDoanhThu = db.CHITIETDATHANGs
                .Where(c => danhSachDonHangThanhCong.Contains(c.MaDonHang))
                .Sum(c => (decimal?)(c.SoLuong * c.DonGia)) ?? 0;

            ViewBag.SoLuongDonHang = soLuongDonHang;
            ViewBag.TongDoanhThu = tongDoanhThu;

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var sTenDN = f["UserName"];
            var sMatKhau = f["Password"];

            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau == sMatKhau);

            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}