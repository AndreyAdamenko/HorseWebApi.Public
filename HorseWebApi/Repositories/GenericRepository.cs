using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HorseWebApi.Repositories
{
    //https://github.com/shawnmclean/OnionArchitecture/blob/master/src/OnionArchitecture.Infrastructure.Repository/EntityFrameworkGenericRepository.cs
    //https://gist.github.com/pmbanugo/8456744
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDBContext context;
        internal DbSet<T> dbSet;

        public GenericRepository(ApplicationDBContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public IQueryable<T> Query()
        {
            return this.dbSet.AsNoTracking().AsQueryable();
        }

        public ICollection<T> GetAll()
        {
            return this.context.Set<T>().AsNoTracking().ToList();
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            try
            {
                return await this.context.Set<T>().AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ICollection<T>> GetAllAsync(params Expression<Func<T, object>>[] properties)
        {
            IQueryable<T> query = this.context.Set<T>().AsNoTracking();
            foreach (var props in properties)
            {
                query = query.Include(props);
            }
            return await query.ToListAsync();
        }

        public T GetById(Guid id)
        {
            return this.context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await this.context.Set<T>().FindAsync(id);
        }

        public T GetByUniqueId(Guid id)
        {
            return this.context.Set<T>().Find(id);
        }

        public async Task<T> GetByUniqueIdAsync(Guid id)
        {
            return await this.context.Set<T>().FindAsync(id);
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return this.dbSet.AsNoTracking().SingleOrDefault(match);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await this.context.Set<T>().AsNoTracking().FirstOrDefaultAsync(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return this.context.Set<T>().Where(match).AsNoTracking().ToList();
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match = null, params Expression<Func<T, object>>[] incProps)
        {
            IQueryable<T> query = this.context.Set<T>().AsNoTracking().Where(match);

            foreach (var inclProp in incProps)
            {
                query = query.Include(inclProp);
            }
            return query.ToList();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await this.context.Set<T>().Where(match).AsNoTracking().ToListAsync();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] properties)
        {
            IQueryable<T> query = this.context.Set<T>().Where(match);

            foreach (var inclProp in properties)
            {
                query = query.Include(inclProp);
            }
            return await query.ToListAsync();
        }

        public T Add(T entity)
        {
            this.context.Entry(entity).State = EntityState.Added;
            this.context.Set<T>().Add(entity);
            this.context.SaveChanges();
            return entity;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            this.context.Entry(entity).State = EntityState.Added;
            await this.context.Set<T>().AddAsync(entity);
            this.context.SaveChanges();
            return entity;
        }

        public T Update(T updated)
        {
            if (updated == null)
            {
                return null;
            }

            this.context.Set<T>().Attach(updated);
            this.context.Entry(updated).State = EntityState.Modified;
            this.context.SaveChanges();

            return updated;
        }

        public void Delete(T t)
        {
            this.context.Set<T>().Remove(t);
            this.context.SaveChanges();
        }

        public int Count()
        {
            return this.context.Set<T>().AsNoTracking().Count();
        }

        public async Task<int> CountAsync()
        {
            return await this.context.Set<T>().CountAsync();
        }

        public IEnumerable<T> Filter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? page = null,
            int? pageSize = null)
        {
            IQueryable<T> query = context.Set<T>().AsNoTracking();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (includeProperties != null)
            {
                foreach (
                    var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query.ToList();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return this.context.Set<T>().Where(predicate).AsNoTracking();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] properties)
        {
            IQueryable<T> query = this.context.Set<T>().Where(predicate).AsNoTracking();

            foreach (var inclProp in properties)
            {
                query = query.Include(inclProp);
            }
            return query;
        }

        public bool Exist(Expression<Func<T, bool>> predicate = null)
        {
            var exist = this.context.Set<T>().Where(predicate);
            return exist.Any() ? true : false;
        }

        public virtual void Dispose()
        {
            if (this.context != null)
            {
                this.context.Dispose();
            }
        }
    }
}
