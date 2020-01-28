using Microsoft.AspNet.Identity;
using SparePartsRequests.Models;
using SparePartsRequests.ViewModels;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SparePartsRequests.Controllers
{
    public class RequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        // GET: Requests
        public async Task<ActionResult> Index()
        {

            var requests = db.Requests.Include(r => r.RequestType);
            var userId = User.Identity.GetUserId();
            
            if (!string.IsNullOrEmpty(userId))
            {
                requests = db.Requests.Where(x => x.ApplicationUserID == userId && x.IsCanceled == false);
            }

            return View(await requests.ToListAsync());
        }

        public async Task<ActionResult> AllRequests()
        {

            var requests = db.Requests.Include(r => r.RequestType);
            requests = db.Requests.Where(x => x.IsCanceled== false );
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
        [Authorize (Roles ="manager")]
        public async Task<ActionResult> RequestApproval(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ApproveRequest = await db.Requests.FindAsync(id);

            if (ApproveRequest == null)
            {
                return HttpNotFound();
            }

            ApproveRequest.IsApproved = true;

            db.Entry(ApproveRequest).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("AllRequests");


            var ApproveRequestVM = new RequestApprovalViewModel();
            ApproveRequestVM.RequestId = ApproveRequest.RequestId;
            ApproveRequestVM.Desc = ApproveRequest.Desc;
           

            
   

           // ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", ApproveRequest.RequestTypeId);
            return View(ApproveRequestVM);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestApproval([Bind(Include = "RequestId,Desc")] RequestApprovalViewModel requestVM)
        {
            if (ModelState.IsValid)
            {
                var request = await db.Requests.FindAsync(requestVM.RequestId);
                request.IsApproved = true;
                //request.Desc = requestVM.Desc;
                db.Entry(request).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("AllRequests");
            }
            //ViewBag.RequestTypeId = new SelectList(db.RequestTypes, "RequestTypeId", "Name", request.RequestTypeId);
            return View(requestVM);
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

        [Authorize (Roles = "manager")]
        public async Task<ActionResult> CancelledRequests()
        { 
            var requests = db.Requests.Include(r => r.RequestType);
            requests = db.Requests.Where(x => x.IsCanceled == true);
     
            return View(await requests.ToListAsync());
        }
    }
}
