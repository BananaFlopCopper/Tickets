using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tickets.Data
{
    public interface IUnitOfWork
    {
        ITicketRepository Tickets { get; }
        int complete();
    }
}
