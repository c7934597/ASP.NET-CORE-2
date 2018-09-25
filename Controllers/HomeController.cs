using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Models;
using static MyWebsite.Startup;

namespace MyWebsite.Controllers
{
    public class HomeController : Controller
    {
        /*private readonly ISample _sample;

        public HomeController(ISample sample)
        {
            _sample = sample;
        }

        public string Index() {
            return $"[ISample]\r\n"
                + $"Id: {_sample.Id}\r\n"
                + $"HashCode: {_sample.GetHashCode()}\r\n"
                + $"Tpye: {_sample.GetType()}";
        }*/

        private readonly ISample _transient;
        private readonly ISample _scoped;
        private readonly ISample _singleton;

        public HomeController(
            ISampleTransient transient,
            ISampleScoped scoped,
            ISampleSingleton singleton)
        {
            _transient = transient;
            _scoped = scoped;
            _singleton = singleton;
        }

        public IActionResult Index() {
            ViewBag.TransientId = _transient.Id;
            ViewBag.TransientHashCode = _transient.GetHashCode();

            ViewBag.ScopedId = _scoped.Id;
            ViewBag.ScopedHashCode = _scoped.GetHashCode();

            ViewBag.SingletonId = _singleton.Id;
            ViewBag.SingletonHashCode = _singleton.GetHashCode();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
