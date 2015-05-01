using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SklepL2.Models;
using SklepL2.ViewModel;

namespace SklepL2.Controllers
{
    public class InvoicesController : Controller
    {
        private SklepL2Entities db = new SklepL2Entities();

        public ActionResult Browser1()
        {
            return View();
        }

        public ActionResult Browser2()
        {
            return View();
        }

        //[ChildActionOnly]
        public PartialViewResult Browser2Partial(string arg)
        {
            var searchResInvoices = db.Invoices.Where(I => I.Number.Contains(arg)).ToList();
            List<FindResultViewModel> invoiceViewModelCollection = new List<FindResultViewModel>();

            foreach(var invoice in searchResInvoices)
            {
                Decimal invoiceTotalAmount = 0;

                foreach (var invoiceItem in db.InvoiceItems.Where(ii => ii.Invoice_ID == invoice.Invoice_ID).ToList())
                {
                    var price = invoiceItem.Product.Price.Value;
                    invoiceTotalAmount += price * invoiceItem.Quantity.Value;
                }

                var customerName = db.Customers.Where(c => c.Customer_ID == invoice.Customer_ID).Select(c => c.Name).FirstOrDefault();

                invoiceViewModelCollection.Add(new FindResultViewModel() { Invoice_ID = invoice.Invoice_ID, InvoiceNumber=invoice.Number, CustomerName=customerName, TotalAmount=invoiceTotalAmount});
            }
            return PartialView(invoiceViewModelCollection);
            //return PartialView(DateTime.Now);
        }


        // GET: Invoices
        public ActionResult Index()
        {
            var invoices = db.Invoices.Include(i => i.Customer);
            return View(invoices.ToList());
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.Customer_ID = new SelectList(db.Customers, "Customer_ID", "Name");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Invoice_ID,Customer_ID,DateIssue,Number")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Customer_ID = new SelectList(db.Customers, "Customer_ID", "Name", invoice.Customer_ID);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.Customer_ID = new SelectList(db.Customers, "Customer_ID", "Name", invoice.Customer_ID);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Invoice_ID,Customer_ID,DateIssue,Number")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Customer_ID = new SelectList(db.Customers, "Customer_ID", "Name", invoice.Customer_ID);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
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
