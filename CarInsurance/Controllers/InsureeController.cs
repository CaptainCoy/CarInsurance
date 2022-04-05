using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            {
                // This will set the base quote to $50 
                insuree.Quote = 50.0m;

                // This will add $100 dollars to the quote if the insuree is 18 and under
                if ((DateTime.Now.Year - insuree.DateOfBirth.Year < 18))  // This subtracts the insuree's date of birth from the current time and checks if the difference is less than 18 
                {
                    insuree.Quote += 100;
                }
                // This will check the insuree's age is greater than 18 but under 25, if the insuree is 19-25 then $50 will be added to the quote
                if ((DateTime.Now.Year - insuree.DateOfBirth.Year > 18 && DateTime.Now.Year - insuree.DateOfBirth.Year < 25))
                {
                    insuree.Quote += 50;
                }
                // This checks if the insuree is 25 and older, if so only $25 is added to the quote
                if ((DateTime.Now.Year - insuree.DateOfBirth.Year > 25))
                {
                    insuree.Quote += 25;
                }
                // If the Insuree's car is made before the year 2000 this will add $25 to the quote
                if (insuree.CarYear < 2000)
                {
                    insuree.Quote += 25;
                }
                // Here does the same thing only it's checking to see if the car was made after 2015
                if (insuree.CarYear > 2015)
                {
                    insuree.Quote += 25;
                }
                // Adding specificity this checks if the insuree's car make is Porsche and if so adds $25 to the base
                if (insuree.CarMake == "Porsche")
                {
                    insuree.Quote += 25;
                }
                // Even more specific this checks if the car is not only a Porsche but the 911 Carrera, if it is a 911 Carrera then an additional $25 is added to the quote
                if (insuree.CarMake == "Porsche" && insuree.CarModel == "911 Carrera")
                {
                    insuree.Quote += 25;
                }
                // This will set sT to 0, check if the number the insuree entered is more than 0, and if so add an additional $10 for every speeding ticket 
                for (int sT = 0; sT < insuree.SpeedingTickets; sT++)
                {
                    insuree.Quote += 10;
                }
                // If this is clicked then the quote will be multiplied by 25%
                if (insuree.DUI == true)
                {
                    insuree.Quote *= 1.25m;
                }
                // If this is clicked then the quote will be multiplied by 50% 
                if (insuree.CoverageType == true)
                {
                    insuree.Quote *= 1.50m;
                }
            }
            if (ModelState.IsValid)
            {
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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
