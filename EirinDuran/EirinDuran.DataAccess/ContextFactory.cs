using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.DataAccess
{
    public class ContextFactory : IContextFactory
    {
        private readonly DbContextOptions<Context> options;

        public ContextFactory(DbContextOptions<Context> options)
        {
            this.options = options;
        }

        public Context GetNewContext()
        {
            return new Context(options);
        }
    }
}
