using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tickets.Models
{
    public class Ticket 
    {
        public int                  id { get; set; }
        public DateTime             initialDate { get; set; }
        public DateTime?            closeDate { get; set; }

        public string               Title { get; set; } // I'm not sure why this wasn't in here honestly

        public string               Description { get; set; }

        public virtual string       UserId { get; set; }
        public virtual IdentityUser User { get; set; }
        
        public resolutionType       ResType { get; set; }

        public enum resolutionType
        {
            closed,
            solved,
            inProgress,
            unassigned
        }
    }
}
