using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Walks.API.Models.Domain;

namespace Walks.API.Data
{
    public class WalksAuthDbContext: IdentityDbContext
    {
        public WalksAuthDbContext(DbContextOptions<WalksAuthDbContext> options) : base (options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "DF47265D-9B6F-43E3-8DD9-D7C1A26CA478";
            var writerRoleId = "5921FBD9-E7A8-4EEF-8C96-5DF1757AF2F7";

            // --- Create a data
            var roles = new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id= writerRoleId,
                    ConcurrencyStamp= writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            // --- Send data
            builder.Entity<IdentityRole>().HasData(roles);

            /* --- FOLLOW THIS GUIDE FROM ADD DATA TO DB
             * -- 1: Open the command line
             * -- 2: Run the command 'Add-Migration "{NAME_OF_MIGRATION}" -Context "{CONTEXT_NAME}"'
             *      | Ex.: Add-Migration "003-CreatingAuthDB" -Context "WalksAuthDbContext"
             * -- 3: Update the Database using: 'Update-Database -Context "WalksAuthDbContext"'
             */
        }
    }
}
