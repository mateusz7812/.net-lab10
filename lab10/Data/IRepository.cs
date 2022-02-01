using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab10.Data
{
    public interface IRepository<T>
    {

        T this[int id] { get; }
         
        IEnumerable<T> Get(int? start_id = null, int number = int.MaxValue) => Get(start_id, number, (x) => true);
        IEnumerable<T> Get(int? start_id, int number, Func<T,bool> filter);
        T Add(T item);
        T Update(T item);
        bool Delete(int id);
    }
}
