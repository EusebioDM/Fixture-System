using System;
using System.Collections.Generic;

namespace EirinDuran.IDataAccess
{
    public interface IRepository<T>
    {
        T Get(object id);

        IEnumerable<T> GetAll();

        void Add(T model);

        void Update(T model);

        void Delete(T model);
    }
}
