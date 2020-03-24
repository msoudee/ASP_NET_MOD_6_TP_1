using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Module6Tp1Dojo.Data;
using Module6Tp1Dojo.Models;
using Module6Tp1Dojo_BO;

namespace Module6Tp1Dojo.Controllers
{
    public class SamouraisController : Controller
    {
        private Context db = new Context();

        // GET: Samourais
        public ActionResult Index()
        {
            return View(db.Samourais.ToList());
        }

        // GET: Samourais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SamouraiViewModel samourai = new SamouraiViewModel();
            samourai.Samourai = db.Samourais.Find(id);

            if (samourai.Samourai == null)
            {
                return HttpNotFound();
            }

            samourai.Potentiel = calculerPotentiel(samourai);

            return View(samourai);
        }

        private double calculerPotentiel(SamouraiViewModel samourai)
        {
            double force = samourai.Samourai.Force;

            double armeDegat = 0;
            if(samourai.Samourai.Arme != null)
            {
                armeDegat = samourai.Samourai.Arme.Degats;
            }

            double nbrArtsMartieux = 0;
            if (samourai.Samourai.ArtMartiaux != null)
            {
                nbrArtsMartieux = samourai.Samourai.ArtMartiaux.Count();
            }

            return (force + armeDegat) * (nbrArtsMartieux + 1);
        }

        // GET: Samourais/Create
        public ActionResult Create()
        {
            SamouraiViewModel vm = new SamouraiViewModel();

            vm.Armes = db.Armes.Where(a => !db.Samourais.Any(s => s.Arme.Id == a.Id)).Select(a => new SelectListItem { Text = a.Nom, Value = a.Id.ToString() }).ToList();
            vm.ArtsMartiaux = db.ArtMartials.Select(am => new SelectListItem { Text = am.Nom, Value = am.Id.ToString() }).ToList();

            return View(vm);
        }

        // POST: Samourais/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiViewModel samouraiVM)
        {
            try
            {
                Samourai samourai = samouraiVM.Samourai;

                if(!db.Samourais.Any(s => s.Arme.Id == samouraiVM.IdArme))
                {
                    samourai.Arme = db.Armes.Find(samouraiVM.IdArme);
                }

                foreach(var idArtMartial in samouraiVM.IdsArtMartiaux)
                {
                    samourai.ArtMartiaux.Add(db.ArtMartials.Find(idArtMartial));
                }

                db.Samourais.Add(samourai);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Samourais/Edit/5
        public ActionResult Edit(int? id)
        {
            SamouraiViewModel vm = new SamouraiViewModel();

            vm.Samourai = db.Samourais.Find(id);
            vm.Armes = db.Armes.Where(a => !db.Samourais.Any(s => s.Arme.Id == a.Id)).Select(a => new SelectListItem { Text = a.Nom, Value = a.Id.ToString() }).ToList();
            vm.ArtsMartiaux = db.ArtMartials.Select(am => new SelectListItem { Text = am.Nom, Value = am.Id.ToString() }).ToList();

            if (vm.Samourai.Arme != null)
            {
                vm.IdArme = vm.Samourai.Arme.Id;
                vm.Armes.Add(new SelectListItem { Text = vm.Samourai.Arme.Nom, Value = vm.Samourai.Arme.Id.ToString() });
            }

            if(vm.Samourai.ArtMartiaux != null && vm.Samourai.ArtMartiaux.Count() > 0)
            {
                vm.IdsArtMartiaux = vm.Samourai.ArtMartiaux.Select(am => am.Id).ToList();
            }

            return View(vm);
        }

        // POST: Samourais/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SamouraiViewModel samouraiVM)
        {
            try
            {
                Samourai samourai = db.Samourais.Find(samouraiVM.Samourai.Id);

                samourai.Nom = samouraiVM.Samourai.Nom;
                samourai.Force = samouraiVM.Samourai.Force;

                if(samouraiVM.IdArme == null)
                {
                    samourai.Arme = null;
                }
                else 
                {
                    if (!db.Samourais.Any(s => s.Arme.Id == samouraiVM.IdArme))
                    {
                        samourai.Arme = db.Armes.Find(samouraiVM.IdArme);
                    }
                }

                if(samouraiVM.IdsArtMartiaux.Count() > 0)
                {
                    samourai.ArtMartiaux.RemoveAll(am => am.Id > 0);
                    foreach (var idArtMartial in samouraiVM.IdsArtMartiaux)
                    {
                        samourai.ArtMartiaux.Add(db.ArtMartials.Find(idArtMartial));
                    }
                }

                db.Entry(samourai).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Samourais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SamouraiViewModel samourai = new SamouraiViewModel();
            samourai.Samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            samourai.Potentiel = calculerPotentiel(samourai);
            return View(samourai);
        }

        // POST: Samourais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samourai samourai = db.Samourais.Find(id);
            db.Samourais.Remove(samourai);
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
