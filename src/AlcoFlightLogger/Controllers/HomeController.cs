using System.Collections.Generic;
using AlcoFlightLogger.Models;
using AlcoFlightLogger.Models.FlightViewModels;
using AutoMapper;
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

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Flights()
        {
            var user = await this.userManager.GetUserAsync(HttpContext.User);
            var flights = Mapper.Map<IEnumerable<FlightViewModel>>(this.repository.GetUserAllFlights(1));
            return View(flights);
        }
    }
}
