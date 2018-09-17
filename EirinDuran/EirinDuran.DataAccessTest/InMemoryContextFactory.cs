using EirinDuran.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EirinDuran.DataAccessTest
{
    internal class InMemoryContextFactory : IDesignTimeDbContextFactory<Context>
    {
        private DbContextOptions<Context> options;

        public InMemoryContextFactory(DbContextOptions<Context> options)
        {
            this.options = options;
        }

        public Context CreateDbContext(string[] args)
        {
            return new Context(options);
        }
    }
}