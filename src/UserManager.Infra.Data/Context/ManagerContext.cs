using UserManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using UserManager.Infra.Data.Mappings;

namespace UserManager.Infra.Data.Context
{
    public class ManagerContext : DbContext
    {
        public ManagerContext()
        {}

        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options)
        {}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsnuilder)
        //{
        //    optionsnuilder.UseSqlServer(@"Data Source=DESKTOP-652APCE\SQLEXPRESS;Initial Catalog=USER_MANAGER;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        //}

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMap());
        }
    }
}
