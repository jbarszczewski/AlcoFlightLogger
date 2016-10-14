using AlcoFlightLogger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AlcoFlightLogger.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<Pilot> userManager;
        private IFlightsRepository repository;

        public HomeController(UserManager<Pilot> userManager, IFlightsRepository repository)
        {
            this.userManager = userManager;
            this.repository = repository;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
