using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaiMinhDang.SachOnline.Models;

namespace LaiMinhDang.SachOnline.Controllers
{
    public class NhaXuatBanController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();

        // Hiển thị danh sách NXB
        public ActionResult Index()
        {
            var listNXB = db.NHAXUATBANs.ToList();
            return View(listNXB);
        }

        // Hiển thị sách theo NXB
        public ActionResult SachTheoNXB(int id)
        {
            var listSach = db.SACHes.Where(s => s.MaNXB == id).ToList();
            return View(listSach);
        }
        public ActionResult Details(int id)
        {
            var nxb = db.NHAXUATBANs.Find(id);
            return View(nxb);
        }
        public NHAXUATBAN GetNXB(int id)
        {
            return db.NHAXUATBANs
                     .Where(n => n.MaNXB == id)
                     .SingleOrDefault();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(GetNXB(id));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit()
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(Request.Form["MaNXB"]);

                NHAXUATBAN nxb = GetNXB(id);

                nxb.TenNXB = Request.Form["TenNXB"];
                nxb.DiaChi = Request.Form["DiaChi"];
                nxb.DienThoai = Request.Form["DienThoai"];

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Edit");
        }
        [HttpPost]
        public ActionResult ThemNXB(NHAXUATBAN nxb)
        {
            db.NHAXUATBANs.Add(nxb);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        //XoaNXB
        public ActionResult XoaNXB(int id)
        {
            NHAXUATBAN nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            return View(nxb);
        }
        [HttpPost]
        public ActionResult XoaNXB(int id, FormCollection collection)
        {
            NHAXUATBAN nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);

            db.NHAXUATBANs.Remove(nxb);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}      