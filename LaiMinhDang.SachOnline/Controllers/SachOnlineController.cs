using LaiMinhDang.SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

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
        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var listSachMoi = SachMoi(20);
            return View(listSachMoi.ToPagedList(pageNumber, pageSize));
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
        // ĐĂNG NHẬP / ĐĂNG XUẤT PARTIAL
        // =====================================
        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogoutPartial");
        }

        // =====================================
        // CHỦ ĐỀ
        // =====================================
        public ActionResult ChuDePartial()
        {
            var listChuDe = db.CHUDEs.ToList();
            return PartialView(listChuDe);
        }

        public ActionResult SachTheoChuDe(int id, int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var listSach = db.SACHes
                             .Where(s => s.MaCD == id)
                             .OrderBy(s => s.MaSach)
                             .ToList();

            var chuDe = db.CHUDEs
                          .FirstOrDefault(cd => cd.MaCD == id);

            if (chuDe != null)
            {
                ViewBag.TenChuDe = chuDe.TenChuDe;
                ViewBag.MaCD = id;
            }

            return View(listSach.ToPagedList(pageNumber, pageSize));
        }

        // =====================================
        // NHÀ XUẤT BẢN
        // =====================================
        public ActionResult NhaXuatBanPartial()
        {
            var listNXB = db.NHAXUATBANs.ToList();
            return PartialView(listNXB);
        }

        public ActionResult SachTheoNhaXuatBan(int id, int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var listSach = db.SACHes
                             .Where(s => s.MaNXB == id)
                             .OrderBy(s => s.MaSach)
                             .ToList();

            var nxb = db.NHAXUATBANs
                        .FirstOrDefault(x => x.MaNXB == id);

            if (nxb != null)
            {
                ViewBag.TenNXB = nxb.TenNXB;
                ViewBag.MaNXB = id;
            }

            return View(listSach.ToPagedList(pageNumber, pageSize));
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