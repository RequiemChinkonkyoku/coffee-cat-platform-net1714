using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        public List<T> GetAll();

        public void Add(T item);

        public void Update(T item);

        public void Delete(T item);

        public T FindById(int id);

        public T GetLast<TKey>(Func<T, TKey> keySelector);

        public List<T> GetPaginated(int page, int pageSize);
    }
}
