using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tickets.Models;

namespace Tickets.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TicketContext _context;
        public UnitOfWork(TicketContext context)
        {
            _context = context;
            Tickets = new TicketRepository(_context);
        }

        public ITicketRepository Tickets { get; private set; }

        public int complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
