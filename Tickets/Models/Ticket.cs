using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tickets.Models
{
    public class Ticket 
    {
        public int                  id { get; set; }
        public DateTime             initialDate { get; set; }
        public DateTime?            closeDate { get; set; }
        [Required(ErrorMessage = "Title is a must.")]
        public string               Title { get; set; } // I'm not sure why this wasn't in here honestly
        [Required(ErrorMessage = "Do you even have a reason to make a ticket?")]
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
