using LaiMinhDang.SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaiMinhDang.SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();

        // =====================================
        // LẤY SÁCH MỚI
        // =====================================
        private List<SACH> SachMoi(int count)
        {
            return db.SACHes
                     .OrderByDescending(s => s.NgayCapNhat)
                     .Take(count)
                     .ToList();
        }

        // =====================================
        // TRANG CHỦ
        // =====================================
        public ActionResult Index()
        {
            var listSachMoi = SachMoi(6);
            return View(listSachMoi);
        }

        // =====================================
        // SLIDER
        // =====================================
        public ActionResult SliderPartial()
        {
            return PartialView();
        }

        // =====================================
        // MENU
        // =====================================
        public ActionResult NavPartial()
        {
            return PartialView();
        }

        // =====================================
        // CHỦ ĐỀ
        // =====================================
        public ActionResult ChuDePartial()
        {
            var listChuDe = db.CHUDEs.ToList();
            return PartialView(listChuDe);
        }

        public ActionResult SachTheoChuDe(int id)
        {
            var listSach = db.SACHes
                             .Where(s => s.MaCD == id)
                             .ToList();

            var chuDe = db.CHUDEs
                          .FirstOrDefault(cd => cd.MaCD == id);

            if (chuDe != null)
                ViewBag.TenChuDe = chuDe.TenChuDe;

            return View(listSach);
        }

        // =====================================
        // NHÀ XUẤT BẢN
        // =====================================
        public ActionResult NhaXuatBanPartial()
        {
            var listNXB = db.NHAXUATBANs.ToList();
            return PartialView(listNXB);
        }

        public ActionResult SachTheoNhaXuatBan(int id)
        {
            var listSach = db.SACHes
                             .Where(s => s.MaNXB == id)
                             .ToList();

            var nxb = db.NHAXUATBANs
                        .FirstOrDefault(x => x.MaNXB == id);

            if (nxb != null)
                ViewBag.TenNXB = nxb.TenNXB;

            return View(listSach);
        }

        // =====================================
        // CHI TIẾT SÁCH
        // =====================================
        public ActionResult ChiTietSach(int id)
        {
            var sach = db.SACHes.SingleOrDefault(s => s.MaSach == id);

            if (sach == null)
            {
                return HttpNotFound();
            }

            return View(sach);
        }

        // =====================================
        // SÁCH BÁN NHIỀU
        // =====================================
        // Theo yêu cầu: "5. Hiển thị 6 sản phẩm mới vào SachBanNhieuPartial. Thực hiện tương tự phần 3 (hiển thị 6 cuốn sách mới)"
        public ActionResult SachBanNhieuPartial()
        {
            var listSachMoi = SachMoi(6);
            return PartialView(listSachMoi);
        }

        // =====================================
        // FOOTER
        // =====================================
        public ActionResult FooterPartial()
        {
            return PartialView();
        }
        public ActionResult Create()
        {
            return View();
        }
    }
}