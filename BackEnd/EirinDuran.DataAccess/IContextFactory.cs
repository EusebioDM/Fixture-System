using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.DataAccess
{
    public interface IContextFactory
    {
        Context GetNewContext();
    }
}
