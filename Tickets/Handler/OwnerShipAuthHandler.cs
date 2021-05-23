using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tickets.Models;

namespace Tickets.Handler
{
    public class OwnerShipAuthHandler
    {
        public class OwnershipAuthRequirement : IAuthorizationRequirement
        {
            // authorization requirements
            public bool AllowOwners { get; set; }
            public bool AllowAdmins { get; set; }
        }

        public class OwnershipAuthHandler : AuthorizationHandler<OwnershipAuthRequirement>
        {
            private UserManager<IdentityUser> userManager;

            public OwnershipAuthHandler(UserManager<IdentityUser> usrMgr)
            {
                userManager = usrMgr;
            }

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnershipAuthRequirement requirement)
            {


                // authorization logic
                Ticket ticket= context.Resource as Ticket;

                string userId = userManager.GetUserId(context.User);

                StringComparison compare = StringComparison.OrdinalIgnoreCase;

                if (ticket != null &&
                    userId != null &&
                    (requirement.AllowOwners && ticket.UserId.Equals(userId, compare)) ||
                    (requirement.AllowAdmins && context.User.IsInRole("Admins"))
                )
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }

                return Task.CompletedTask;
            }
        }
    }
}
