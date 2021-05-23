using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tickets.Data;
using Tickets.Models;

namespace Tickets.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly TicketContext _context;
        private IUnitOfWork _unitOfWork;
        private ITicketRepository _repo;
        //private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private IAuthorizationService _authService;

        public TicketsController(
            TicketContext               context     , 
            ITicketRepository           repo        , 
            IUnitOfWork                 unitOfWork  ,
            UserManager<IdentityUser>   usr         ,
            IAuthorizationService       auth 
            )
        {
            _context        =   context     ;
            _repo           =   repo        ;
            _unitOfWork     =   unitOfWork  ;
            _userManager    =   usr         ;
            _authService    =   auth        ;
        }

        // GET: Tickets
        public IActionResult Index()
        {
            return View(_repo.GetAllByUser(_userManager.GetUserId(User)));
        }

        // GET: Tickets/Details/5
        public IActionResult Details(int id) // removed int id = 0 because that seems dumb
        {
            if (id == 0)
            { return NotFound(); }

            var ticket = _repo.Get(id);
            
            if (ticket == null)
            { return NotFound(); }

            return View(ticket);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            var user = await  _userManager.GetUserAsync(User);
            Ticket ticket = new Ticket();
            ticket.User         = user;
            ticket.UserId       = user.Id;
            ticket.initialDate  = DateTime.Now;
            ticket.ResType = Ticket.resolutionType.unassigned;

            TempData["userid"] = user.Id;

            return View(ticket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Create([Bind("id,initialDate,closeDate,Title,Description,ResType,UserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(ticket);
                _unitOfWork.complete();

                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        
        public async Task<IActionResult> Edit(int id = 0) // again removed int id = 0, that sounds terrible
        {
            if (id == 0)
            { return NotFound(); }

            Ticket ticket = _repo.Get(id);
            if (ticket == null)
            { return NotFound(); }

            AuthorizationResult authorized = await _authService.AuthorizeAsync(User, ticket, "OwnersAndAdmins");

            if (authorized.Succeeded)
            { return View(ticket); }
            else
            { return new ForbidResult(); }
            
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,initialDate,closeDate,Title,Description,ResType,UserId")] Ticket ticket)
        {
            if (id != ticket.id)
            { return NotFound(); }

            AuthorizationResult authorized = await _authService.AuthorizeAsync(User, ticket, "OwnersAndAdmins");

            if (authorized.Succeeded)
            {
                if (ModelState.IsValid)
                {
                    Ticket updatedTicket = _repo.Get(ticket.id);

                    updatedTicket.initialDate = ticket.initialDate;
                    updatedTicket.closeDate = ticket.closeDate;
                    updatedTicket.Title = ticket.Title;
                    updatedTicket.Description = ticket.Description;
                    updatedTicket.ResType = ticket.ResType;

                    _unitOfWork.complete();

                    return RedirectToAction(nameof(Index));
                }
                return View(ticket);   
            }
            else
            { return new ForbidResult(); }

        }

        // GET: Tickets/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id = 0)
        {
            if (id == 0)
            { return NotFound(); }

            var ticket = _repo.Get(id);
            if (ticket == null)
            { return NotFound(); }


            AuthorizationResult authorized = await _authService.AuthorizeAsync(User, ticket, "OwnersAndAdmins");

            if (authorized.Succeeded)
            { return View(ticket); }
            else
            { return new ForbidResult(); }

            
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = _repo.Get(id);

            AuthorizationResult authorized = await _authService.AuthorizeAsync(User, ticket, "OwnersAndAdmins");

            if (authorized.Succeeded)
            {
                _repo.Remove(ticket);
                _unitOfWork.complete();
                return RedirectToAction(nameof(Index));
            }
            else
            { return new ForbidResult(); }
            
            
        }

        private bool TicketExists(int id)
        {
            return _repo.Get(id) == null;
        }
    }
}
