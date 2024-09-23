using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SiyafundaApplication.Controllers
{
    public class SignIn : Controller
    {
        // GET: SignIn
        public ActionResult Index()
        {
            return View();
        }

        // GET: SignIn/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SignIn/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SignIn/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SignIn/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SignIn/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SignIn/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SignIn/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
