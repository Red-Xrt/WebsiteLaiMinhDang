using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaiMinhDang.SachOnline.Models;

namespace LaiMinhDang.SachOnline.Controllers
{
    public class GioHangController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();

        public List<GioHang> LayGioHang()
        {
            try
            {
                List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
                if (lstGioHang == null)
                {
                    lstGioHang = new List<GioHang>();
                    Session["GioHang"] = lstGioHang;
                }
                return lstGioHang;
            }
            catch
            {
                return new List<GioHang>();
            }
        }

        public ActionResult ThemGioHang(int ms, string url)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Find(n => n.iMaSach == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }

        private int TongSoLuong()
        {
            try
            {
                int iTongSoLuong = 0;
                List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
                if (lstGioHang != null)
                {
                    iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
                }
                return iTongSoLuong;
            }
            catch
            {
                return 0;
            }
        }

        private double TongTien()
        {
            try
            {
                double dTongTien = 0;
                List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
                if (lstGioHang != null)
                {
                    dTongTien = lstGioHang.Sum(n => n.dThanhTien);
                }
                return dTongTien;
            }
            catch
            {
                return 0;
            }
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return View(lstGioHang);
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

        [ChildActionOnly]
        public ActionResult GioHangPartial()
        {
            try
            {
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.TongTien = TongTien();
                return PartialView();
            }
            catch
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
        }

        public ActionResult XoaGioHang(int iMaSach)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSach == iMaSach);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "SachOnline");
                }
            }
            else if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int iMaSach, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "SachOnline");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "LaiMinhDang_User");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            try
            {
                DONDATHANG ddh = new DONDATHANG();
                KHACHHANG kh = Session["TaiKhoan"] as KHACHHANG;
                if (kh == null) return RedirectToAction("DangNhap", "LaiMinhDang_User");

                List<GioHang> lstGioHang = LayGioHang();
                if (lstGioHang == null || lstGioHang.Count == 0) return RedirectToAction("Index", "SachOnline");

                ddh.MaKH = kh.MaKH;
                ddh.NgayDat = DateTime.Now;
                if (DateTime.TryParse(f["NgayGiao"], out DateTime ngayGiao))
                {
                    ddh.NgayGiao = ngayGiao;
                }
                ddh.TinhTrangGiaoHang = 0;
                ddh.DaThanhToan = false;
                db.DONDATHANGs.Add(ddh);
                db.SaveChanges();
                foreach (var item in lstGioHang)
                {
                    CHITIETDATHANG ctdh = new CHITIETDATHANG();
                    ctdh.MaDonHang = ddh.MaDonHang;
                    ctdh.MaSach = item.iMaSach;
                    ctdh.SoLuong = item.iSoLuong;
                    ctdh.DonGia = (decimal)item.dDonGia;
                    db.CHITIETDATHANGs.Add(ctdh);
                }
                db.SaveChanges();
                Session["GioHang"] = null;
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            catch (Exception ex)
            {
                // Handle exception to avoid application crash
                return RedirectToAction("DatHang", "GioHang");
            }
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}