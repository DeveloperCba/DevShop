using DevShop.Identity.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DevShop.Identity.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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

        public IActionResult TesteAjax()
        {
            var lista = new List<People>
            {
                new People {Id = 1, Name = "Name 01"},
                new People {Id = 2, Name = "Name 02"},
                new People {Id = 4, Name = "Name 03"},
                new People {Id = 5, Name = "Name 04"},
                new People {Id = 6, Name = "Name 05"}
            };

            var peopleVm = new PeopleViewModel
            {
                Pessoas = lista
            };

            return Json(peopleVm);
        }

        [HttpPost]
        public IActionResult TesteAjaxPost([FromBody] People people)
        {
            var lista = new List<string>
            {
                "TESTE 01", "TESTE 02", "TESTE 03", "TESTE 04", "TESTE 05"
            };

            lista = lista.Where(x => x == people?.Name?.ToUpper()).ToList();
            return Json(people);
        }

        [HttpPost]
        public IActionResult TesteAjaxListaParametrosPost([FromBody] PeopleViewModel peoples)
        {
            return Json(peoples);
        }

        [HttpPost]
        public IActionResult TesteAjaxListaParametrosPost2([FromBody] List<People> peoples)
        {
            return Json(data: peoples);
        }

        [HttpPost]
        public IActionResult TesteAjaxListaParametros2Post([FromBody] People2ViewModel peoples)
        {
            return Json(peoples);
        }
    }
}