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
        [ChildActionOnly]
        public ActionResult SliderPartial()
        {
            try
            {
                return PartialView();
            }
            catch
            {
                return PartialView();
            }
        }

        // =====================================
        // MENU
        // =====================================
        [ChildActionOnly]
        public ActionResult NavPartial()
        {
            try
            {
                return PartialView();
            }
            catch
            {
                return PartialView();
            }
        }

        // =====================================
        // ĐĂNG NHẬP / ĐĂNG XUẤT PARTIAL
        // =====================================
        [ChildActionOnly]
        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogoutPartial");
        }

        // =====================================
        // CHỦ ĐỀ
        // =====================================
        [ChildActionOnly]
        public ActionResult ChuDePartial()
        {
            try
            {
                var listChuDe = db.CHUDEs.ToList();
                return PartialView(listChuDe);
            }
            catch
            {
                return PartialView(new List<CHUDE>());
            }
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
        [ChildActionOnly]
        public ActionResult NhaXuatBanPartial()
        {
            try
            {
                var listNXB = db.NHAXUATBANs.ToList();
                return PartialView(listNXB);
            }
            catch
            {
                return PartialView(new List<NHAXUATBAN>());
            }
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
        private List<SACH> SachBanNhieu(int count)
        {
            return db.SACHes
                     .OrderByDescending(s => s.SoLuongBan)
                     .Take(count)
                     .ToList();
        }

        [ChildActionOnly]
        public ActionResult SachBanNhieuPartial()
        {
            try
            {
                var listSachBanNhieu = SachBanNhieu(6);
                return PartialView(listSachBanNhieu);
            }
            catch
            {
                return PartialView(new List<SACH>());
            }
        }

        // =====================================
        // FOOTER
        // =====================================
        [ChildActionOnly]
        public ActionResult FooterPartial()
        {
            try
            {
                return PartialView();
            }
            catch
            {
                return PartialView();
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        // =====================================
        // LIÊN HỆ
        // =====================================
        public ActionResult LienHe()
        {
            return View();
        }

        // =====================================
        // TÌM KIẾM
        // =====================================
        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text ?? "";

            text = text.ToLower();
            string[] vnChars = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };

            for (int i = 1; i < vnChars.Length; i++)
            {
                for (int j = 0; j < vnChars[i].Length; j++)
                {
                    text = text.Replace(vnChars[i][j].ToString(), vnChars[0][i - 1].ToString());
                }
            }
            return text;
        }

        public ActionResult KetQuaTimKiem(string data, int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            ViewBag.TuKhoa = data;

            var allBooks = db.SACHes.ToList();
            string searchString = RemoveDiacritics(data);

            var listSach = allBooks
                             .Where(s => RemoveDiacritics(s.TenSach).Contains(searchString))
                             .OrderBy(s => s.MaSach)
                             .ToList();

            return View(listSach.ToPagedList(pageNumber, pageSize));
        }
    }
}