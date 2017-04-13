using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCFR.Models;

namespace MVCFR.Controllers
{
    public class UserInfoController : Controller
    {
        private MachineLearningEntities db = new MachineLearningEntities();

        // GET: UserInfo
        public ActionResult Index()
        {
            return View(db.UserFaceInfoes.ToList());
        }

        // GET: UserInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserFaceInfo userFaceInfo = db.UserFaceInfoes.Find(id);
            if (userFaceInfo == null)
            {
                return HttpNotFound();
            }
            return View(userFaceInfo);
        }

        // GET: UserInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,faceSample,userId,id,tag")] UserFaceInfo userFaceInfo)
        {
            if (ModelState.IsValid && Request.Files != null && Request.Files.Count > 0)
            {
                //attach the uploaded image to the object before saving to Database
                userFaceInfo.faceSample = GetImage(Request.Files[0]);
                if (userFaceInfo.faceSample != null)
                {
                    db.UserFaceInfoes.Add(userFaceInfo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }                      
            }

            return View(userFaceInfo);
        }        

        // GET: UserInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserFaceInfo userFaceInfo = db.UserFaceInfoes.Find(id);
            if (userFaceInfo == null)
            {
                return HttpNotFound();
            }
            return View(userFaceInfo);
        }

        // POST: UserInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,faceSample,userId,id,tag")] UserFaceInfo userFaceInfo)
        {
            if (ModelState.IsValid && Request.Files != null && Request.Files.Count > 0)
            {
                //attach the uploaded image to the object before saving to Database
                userFaceInfo.faceSample = GetImage(Request.Files[0]);
                if (userFaceInfo.faceSample != null)
                {
                    db.Entry(userFaceInfo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(userFaceInfo);
        }

        // GET: UserInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserFaceInfo userFaceInfo = db.UserFaceInfoes.Find(id);
            if (userFaceInfo == null)
            {
                return HttpNotFound();
            }
            return View(userFaceInfo);
        }

        // POST: UserInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserFaceInfo userFaceInfo = db.UserFaceInfoes.Find(id);
            db.UserFaceInfoes.Remove(userFaceInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MyImage(int id)
        {
            UserFaceInfo userFaceInfo = db.UserFaceInfoes.Find(id);
            return View(userFaceInfo);
        }

        private byte[] GetImage(HttpPostedFileBase imgFile)
        {
            byte[] imgBytes = null;
            var bytesToRead = (int)Request.Files[0].ContentLength;

            if (bytesToRead > 0)
            {
                var numBytesRead = 0;
                imgBytes = new byte[bytesToRead];

                while (bytesToRead > 0)
                {
                    int n = Request.Files[0].InputStream.Read(imgBytes, numBytesRead, bytesToRead);
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    bytesToRead -= n;
                }
            }

            return imgBytes;
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
