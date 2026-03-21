using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaiMinhDang.SachOnline.Models;

namespace LaiMinhDang.SachOnline.Controllers
{
    public class LaiMinhDang_UserController : Controller
    {
        SachOnlineEntities data = new SachOnlineEntities();

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        // POST: User/DangNhap
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            var tendn = f["TenDN"];
            var matkhau = f["MatKhau"];

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Err1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = data.KHACHHANGs
                .SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);

                if (kh != null)
                {
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "SachOnline");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }

            return View();
        }

        // ==============================
        // ĐĂNG XUẤT
        // ==============================

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "SachOnline");
        }

        // ==============================
        // TRANG ĐĂNG KÝ
        // ==============================

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection f)
        {
            KHACHHANG kh = new KHACHHANG();

            kh.HoTen = f["HoTen"];
            kh.TaiKhoan = f["TaiKhoan"];
            kh.MatKhau = f["MatKhau"];
            kh.Email = f["Email"];
            kh.DiaChi = f["DiaChi"];        // sửa ở đây
            kh.DienThoai = f["DienThoai"];  // sửa ở đây
            kh.NgaySinh = DateTime.Parse(f["NgaySinh"]);

            data.KHACHHANGs.Add(kh);
            data.SaveChanges();

            return RedirectToAction("DangNhap");
        }

    }
}