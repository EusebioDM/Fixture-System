using System;
using System.Collections.Generic;
using System.Text;

namespace SilverFixture.DataAccess
{
    public interface IContextFactory
    {
        Context GetNewContext();
    }
}
