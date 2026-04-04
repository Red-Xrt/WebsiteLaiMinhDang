using LaiMinhDang.SachOnline.Models;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace LaiMinhDang.SachOnline.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();

        public ActionResult Index(int? page)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            int iPageNum = (page ?? 1);
            int iPageSize = 10;

            var lstDonHang = db.DONDATHANGs.OrderByDescending(n => n.NgayDat).ToList();
            return View(lstDonHang.ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            var dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            var chitiet = db.CHITIETDATHANGs.Where(c => c.MaDonHang == id).ToList();
            ViewBag.ChiTiet = chitiet;

            return View(dh);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            var dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(dh);
        }

        [HttpPost]
        public ActionResult Edit(DONDATHANG dh)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            var donhang = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == dh.MaDonHang);
            if (donhang != null)
            {
                donhang.DaThanhToan = dh.DaThanhToan;
                donhang.TinhTrangGiaoHang = dh.TinhTrangGiaoHang;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dh);
        }
    }
}