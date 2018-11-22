using System;
using System.Collections.Generic;

namespace SilverFixture.IDataAccess
{
    public interface IRepository<T>
    {
        T Get(string id);

        IEnumerable<T> GetAll();

        void Add(T model);

        void Update(T model);

        void Delete(string id);
    }
}
