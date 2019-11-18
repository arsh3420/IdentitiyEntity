using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitiyEntity.Models;
using System.IO;
using Microsoft.AspNet.Identity.Owin;

namespace IdentitiyEntity.Controllers
{
    public class ProductDetailsController : Controller
    {
        private Entities db = new Entities();

        // GET: ProductDetails
        [Authorize]
        public ActionResult Index()
        {
            return View(db.ProductDetails.ToList());
        }
        [Authorize]
        // GET: ProductDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = db.ProductDetails.Find(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            return View(productDetail);
        }
        [Authorize]
        // GET: ProductDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,productname,productimg,productprice,userip,file")] ProductDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                //Use Namespace called :  System.IO  
                string FileName = Path.GetFileNameWithoutExtension(productDetail.file.FileName);

                //To Get File Extension  
                string FileExtension = Path.GetExtension(productDetail.file.FileName);

                //Add Current Date To Attached File Name  
                FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                //Its Create complete path to store in server.  
                productDetail.productimg = FileName;

                //To copy and save file into server.  
                productDetail.file.SaveAs(Server.MapPath("~" + ConfigurationManager.AppSettings["UserImagePath"].ToString()) + FileName);
                productDetail.userip = Request.ServerVariables["REMOTE_ADDR"];
                db.ProductDetails.Add(productDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productDetail);
        }

        // GET: ProductDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = db.ProductDetails.Find(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            return View(productDetail);
        }

        // POST: ProductDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,productname,productimg,productprice,file")] ProductDetail productDetail)
        {
            productDetail.userip = Request.ServerVariables["REMOTE_ADDR"];

            //Use Namespace called :  System.IO  
            string FileName = Path.GetFileNameWithoutExtension(productDetail.file.FileName);

            //To Get File Extension  
            string FileExtension = Path.GetExtension(productDetail.file.FileName);

            //Add Current Date To Attached File Name  
            FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

            //Its Create complete path to store in server.  
            productDetail.productimg = FileName;

            //To copy and save file into server.  
            productDetail.file.SaveAs(Server.MapPath("~" + ConfigurationManager.AppSettings["UserImagePath"].ToString()) + FileName);

            if (ModelState.IsValid)
            {
                db.Entry(productDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productDetail);
        }

        // GET: ProductDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = db.ProductDetails.Find(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            return View(productDetail);
        }

        // POST: ProductDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductDetail productDetail = db.ProductDetails.Find(id);
            if (new FileInfo(Server.MapPath("~" + ConfigurationManager.AppSettings["UserImagePath"].ToString()) + productDetail.productimg).Exists)
            {
                new FileInfo(Server.MapPath("~" + ConfigurationManager.AppSettings["UserImagePath"].ToString()) + productDetail.productimg).Delete();
            }
            db.ProductDetails.Remove(productDetail);
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
