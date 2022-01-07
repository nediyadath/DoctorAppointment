using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoctorAppointment.Models;

namespace DoctorAppointment.Controllers
{
    public class AppointmentsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Appointments
        public ActionResult Index()
        {
            var appointment = db.appointment.Include(a => a.doc).Include(a => a.pat);
            return View(appointment.ToList());
        }

        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.appointment.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            ViewBag.docid = new SelectList(db.doctror, "id", "name");
            ViewBag.ptid = new SelectList(db.patient, "id", "name");
            return View();
        }


        public JsonResult GetAppointment(DateTime date, int docID)
        {
            string dow = date.ToString("ddd").ToUpper();
            string slots = db.doctror.Single(x => x.id == docID).Slots;
            string[] slotArray = slots.Split(',');
            string SelectedSlot = "";
            foreach(string sl in slotArray)
            {
                if (sl.Substring(0, 3) == dow)
                {
                    SelectedSlot = sl;
                    break;
                }
            }
            string[] sslot = SelectedSlot.Split('*');
            string[] times = sslot[1].Split('-');
            int startnum = int.Parse(times[0]);
            int endnum = int.Parse(times[1]);
            string slottimes = "";
            for(int i = startnum; i < endnum; i += 1)
            {
                int fn = i;
                int sn = i + 1;
                slottimes += fn+"-"+sn+",";
            }
            slottimes = slottimes.TrimEnd(',');
            return Json(slots, JsonRequestBehavior.AllowGet);

        }
        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ptid,docid,appDate,appSlot")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.appointment.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.docid = new SelectList(db.doctror, "id", "name", appointment.docid);
            ViewBag.ptid = new SelectList(db.patient, "id", "name", appointment.ptid);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.appointment.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.docid = new SelectList(db.doctror, "id", "name", appointment.docid);
            ViewBag.ptid = new SelectList(db.patient, "id", "name", appointment.ptid);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ptid,docid,appDate,appSlot")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.docid = new SelectList(db.doctror, "id", "name", appointment.docid);
            ViewBag.ptid = new SelectList(db.patient, "id", "name", appointment.ptid);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.appointment.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.appointment.Find(id);
            db.appointment.Remove(appointment);
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
