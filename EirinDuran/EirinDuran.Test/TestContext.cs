using Microsoft.EntityFrameworkCore;
using EirinDuran.DataAccess;
using Entities;

namespace EirinDuran.Test
{
    public class TestContext : IContext
    {
        public TestContext()
        {
        }

        public DbSet<UserEntity> UserEntities { get; set; }

        public int SaveChanges()
        {
            return 0;
        }
    }
}