using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCFaceRecognition.Models;

namespace MVCFaceRecognition.Controllers
{
    public class UserPictureController : Controller
    {
        private MLEntities db = new MLEntities();

        // GET: UserPicture
        public ActionResult Index()
        {
            return View(db.OpenCVFaces.ToList());
        }

        // GET: UserPicture/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenCVFace openCVFace = db.OpenCVFaces.Find(id);
            if (openCVFace == null)
            {
                return HttpNotFound();
            }
            return View(openCVFace);
        }

        // GET: UserPicture/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserPicture/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,faceSample,userId,id")] OpenCVFace openCVFace)
        {
            if (ModelState.IsValid)
            {
                db.OpenCVFaces.Add(openCVFace);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(openCVFace);
        }

        // GET: UserPicture/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenCVFace openCVFace = db.OpenCVFaces.Find(id);
            if (openCVFace == null)
            {
                return HttpNotFound();
            }
            return View(openCVFace);
        }

        // POST: UserPicture/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,faceSample,userId,id")] OpenCVFace openCVFace)
        {
            if (ModelState.IsValid)
            {
                db.Entry(openCVFace).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(openCVFace);
        }

        // GET: UserPicture/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenCVFace openCVFace = db.OpenCVFaces.Find(id);
            if (openCVFace == null)
            {
                return HttpNotFound();
            }
            return View(openCVFace);
        }

        // POST: UserPicture/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            OpenCVFace openCVFace = db.OpenCVFaces.Find(id);
            db.OpenCVFaces.Remove(openCVFace);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
