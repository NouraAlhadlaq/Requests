using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SparePartsRequests.Models;
using Microsoft.AspNet.Identity;

namespace SparePartsRequests.Controllers
{
    public class RequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Requests
        public async Task<ActionResult> Index()
        {

            var requests = db.Requests.Include(r => r.RequestType);
            var userId = User.Identity.GetUserId();

            if (!string.IsNullOrEmpty(userId))
            {
                requests = db.Requests.Where(x => x.ApplicationUserID == userId);
            }

            return View(await requests.ToListAsync());
        }

        // GET: Requests/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
       [Authorize]
        public ActionResult Create()
        {
            ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = " RequestId,Title,Desc,RequestTypeId,ApplicationUserId")] Request request)
        {
            if (ModelState.IsValid)
            {
                request.ApplicationUserID = User.Identity.GetUserId();
                db.Requests.Add(request);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(request);
        }

        // GET: Requests/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }

            ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RequestId,Title,Desc,RequestTypeId,IsApproved,IsRejected,IsCanceled,,ApplicationUserId")] Request request)
        {
            if (ModelState.IsValid)
            {
                request.ApplicationUserID = User.Identity.GetUserId();

                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = await db.Requests.FindAsync(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Request request = await db.Requests.FindAsync(id);
            db.Requests.Remove(request);
            await db.SaveChangesAsync();
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
