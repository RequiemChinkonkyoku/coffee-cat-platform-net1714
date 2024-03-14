using DAOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        CoffeeCatDbContext context;
        DbSet<T> set;

        public RepositoryBase()
        {
            context = new CoffeeCatDbContext();
            set = context.Set<T>();
        }

        public List<T> GetAll()
        {
            return set.ToList();
        }

        public void Add(T item)
        {
            set.Add(item);
            context.SaveChanges();
        }

        public void Update(T item)
        {
            set.Update(item);
            context.SaveChanges();
        }

        public void Delete(T item)
        {
            set.Remove(item);
            context.SaveChanges();
        }

        public T FindById(int id)
        {
            return set.Find(id);
        }

        public T GetLast<TKey>(Func<T, TKey> keySelector)
        {
            return set.OrderByDescending(keySelector).FirstOrDefault();
        }

        public List<T> GetPaginated(int page, int pageSize)
        {
            return set.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
