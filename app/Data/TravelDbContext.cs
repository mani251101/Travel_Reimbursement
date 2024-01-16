using Microsoft.EntityFrameworkCore;
using Reimbursementform.Models;
#nullable disable

namespace scaflogindbcontext
{
    public class TravelDbContext : DbContext
    {
        public TravelDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Reimbursementforms>(
                eb => {
                    eb.HasKey(x => x.FormId);
                }
            );
        }
        public DbSet<Reimbursementforms> reimbursementforms {get; set;}
    }
}