using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tickets.Models;

namespace Tickets.Data
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(TicketContext context) : base(context)
        {
        }

        public override Ticket Get(int id)
        {
            return Context.allTickets
                .Where(p => p.id == id) // I see why id is a bad name, I should have made Ticket ID something else
                .Include(p => p.User)
                .SingleOrDefault();
        }

        public override IEnumerable<Ticket> GetAll()
        {
            return Context.allTickets
                .Include(p => p.User)
                .ToList();
        }

        public IEnumerable<Ticket> GetAllByUser(string UserId)
        {
            return Context.allTickets
                .Where(p => p.UserId == UserId)
                .Include(p => p.User)
                .ToList();
        }

        public IEnumerable<Ticket> GetAllByUser(IdentityUser user)
        {
            return Context.allTickets.Where(p => p.UserId == user.Id)
                .Include(p => p.User)
                .ToList();
        }
    }
}
