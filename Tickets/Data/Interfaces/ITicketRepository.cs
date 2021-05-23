using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tickets.Models;

namespace Tickets.Data
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        IEnumerable<Ticket> GetAllByUser(string UserId);
        IEnumerable<Ticket> GetAllByUser(IdentityUser user);
    }
}
