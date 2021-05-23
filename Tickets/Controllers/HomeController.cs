using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tickets.Data;
using Tickets.Models;

namespace Tickets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TicketContext _ctx;
        private readonly IUnitOfWork _unitOfWork;

        private ITicketRepository _repo;

        public HomeController(ILogger<HomeController> logger, TicketContext ticketContext, ITicketRepository repo, IUnitOfWork uow)
        {
            _logger = logger;
            _ctx = ticketContext;
            _repo = repo;
            _unitOfWork = uow;
        }

        public IActionResult Index()
        {
            IEnumerable<Ticket> tickets = _repo.GetAll();
            return View(tickets);
        }

        public IActionResult Detail(int id)
        {
            Ticket ticket = _repo.Get(id);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
