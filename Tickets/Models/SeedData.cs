using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tickets.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Tickets.Handler;

namespace Tickets.Models
{
    public class SeedData
    {




        public async Task Initialize(IServiceProvider serviceProvider)
        {

            UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            using (var context = new TicketContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<TicketContext>
                    >()
                )
            )
            {
                // are we already populated?
                if (!context.allTickets.Any())
                {
                    IdentityUser TicketMaker = await userManager.FindByEmailAsync("CreatorOfAllBadPrograms@BadCode.404");
                    context.allTickets.AddRange(
                        new Ticket
                        {
                            initialDate = DateTime.Now,
                            closeDate = DateTime.Now,
                            Title = "Baguette Eating Squirrel Problem Rising",
                            Description = "",
                            ResType = Ticket.resolutionType.unassigned,

                            UserId = TicketMaker.Id, // not entirely sure why this breaks if I feed a user but not if I feed the id directly
                            //User = TicketMaker 


                        },
                        new Ticket
                        {
                            initialDate = DateTime.Now,
                            // closeDate = DateTime.Now,
                            Title = "Flare",
                            Description = "chars",
                            ResType = Ticket.resolutionType.closed,

                            UserId = TicketMaker.Id,
                            //User = TicketMaker
                        }
                    );
                    context.SaveChanges();
                }

            }
        }

        public async Task CreateRolledAccounts(IServiceProvider serviceProvider, IConfiguration config)
        {
            UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            string[] SeedUserData = new string[]
            {
                "CreatorOfBad",
                "Admin2",
                "Admin3",
            };
            foreach (string User in SeedUserData)
            {

                string name     =   config["Data:" + User + ":Name"];
                string email    =   config["Data:" + User + ":Email"];
                string password =   config["Data:" + User + ":Password"];
                string role     =   config["Data:" + User + ":Role"];

                //var existing = await userManager.FindByNameAsync("");
                //await userManager.AddToRoleAsync(existing, role);             // I imagine this will be useful in giving peeps rolls

                // check if user exists
                if (await userManager.FindByNameAsync(name) == null)
                {
                    // check if role exists
                    if (await roleManager.FindByNameAsync(role) == null)
                    {
                        // add role if missing
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }


                IdentityUser user = new IdentityUser
                {
                    UserName = name,
                    Email = email
                };
                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
