using System.Collections.Generic;
using AlcoFlightLogger.Models;
using AlcoFlightLogger.Models.FlightEntryViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlcoFlightLogger.Controllers
{
    public class HomeController : Controller
    {
        private IFlightEntriesRepository repository;

        public HomeController(IFlightEntriesRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Flights()
        {
            var flights = Mapper.Map<IEnumerable<FlightEntryViewModel>>(this.repository.GetUserAllFlightEntries(User.Identity.Name));
            return View(flights);
        }
    }
}
