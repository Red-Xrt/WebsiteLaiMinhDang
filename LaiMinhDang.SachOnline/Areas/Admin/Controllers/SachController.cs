using LaiMinhDang.SachOnline.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace LaiMinhDang.SachOnline.Areas.Admin.Controllers
{
    public class SachController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();

        public ActionResult Index(int? page)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            int iPageNum = (page ?? 1);
            int iPageSize = 7;

            var lstSach = db.SACHes.OrderByDescending(n => n.NgayCapNhat).ToList();
            return View(lstSach.ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SACH sach, HttpPostedFileBase fFileUpload)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            if (fFileUpload == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                ViewBag.TenSach = sach.TenSach;
                ViewBag.MoTa = sach.MoTa;
                ViewBag.SoLuongBan = sach.SoLuongBan;
                ViewBag.GiaBan = sach.GiaBan;
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.AnhBia = sFileName;
                    sach.NgayCapNhat = DateTime.Now;
                    db.SACHes.Add(sach);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            var sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            var sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            var sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaSach == id);
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Sách này đang có trong đơn đặt hàng nên không thể xóa";
                return View(sach);
            }

            var vietsach = db.VIETSACHes.Where(vs => vs.MaSach == id).ToList();
            if (vietsach != null)
            {
                db.VIETSACHes.RemoveRange(vietsach);
                db.SaveChanges();
            }

            db.SACHes.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            var sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            return View(sach);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(SACH sach, HttpPostedFileBase fFileUpload)
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Home");

            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.AnhBia = sFileName;
                }
                else
                {
                    var s = db.SACHes.AsNoTracking().SingleOrDefault(n => n.MaSach == sach.MaSach);
                    if (s != null)
                    {
                        sach.AnhBia = s.AnhBia;
                    }
                }

                sach.NgayCapNhat = DateTime.Now;
                db.Entry(sach).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(sach);
        }
    }
}