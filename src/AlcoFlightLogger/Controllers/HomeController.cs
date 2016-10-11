using System.Collections.Generic;
using AlcoFlightLogger.Models;
using AlcoFlightLogger.Models.FlightViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlcoFlightLogger.Controllers
{
    public class HomeController : Controller
    {
        private IFlightsRepository repository;

        public HomeController(IFlightsRepository repository)
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
            //TODO: add user identity
            var flights = Mapper.Map<IEnumerable<FlightViewModel>>(this.repository.GetUserAllFlights(1));
            return View(flights);
        }
    }
}
