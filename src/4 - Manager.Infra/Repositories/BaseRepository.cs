using System.Linq;
using Manager.Infra.Context;
using System.Threading.Tasks;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Manager.Infra.Repositories{
    public class BaseRepository<T> : IBaseRepository<T> where T : Base{
        private readonly ManagerContext _context;

        public BaseRepository(ManagerContext context)
        {
            _context = context;
        }

        public virtual async Task<T> CreateAsync(T obj){
            _context.Add(obj);
            await _context.SaveChangesAsync();

            return obj;
        }

        public virtual async Task<T> UpdateAsync(T obj){
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return obj;
        }

        public virtual async Task RemoveAsync(long id){
            var obj = await GetAsync(id);

            if(obj != null){
                _context.Remove(obj);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<T> GetAsync(long id){
            var obj = await _context.Set<T>()
                                    .AsNoTracking()
                                    .Where(x=>x.Id == id)
                                    .ToListAsync();

            return obj.FirstOrDefault();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public virtual async Task<T> GetAsync(
            Expression<Func<T, bool>> expression,
            bool asNoTracking = true)
                => asNoTracking
                ? await BuildQuery(expression)
                        .AsNoTracking()
                        .FirstOrDefaultAsync()

                : await BuildQuery(expression)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

        public virtual async Task<IList<T>> SearchAsync(
            Expression<Func<T, bool>> expression,
            bool asNoTracking = true)
                => asNoTracking
                ? await BuildQuery(expression)
                        .AsNoTracking()
                        .ToListAsync()

                : await BuildQuery(expression)
                        .AsNoTracking()
                        .ToListAsync();

        private IQueryable<T> BuildQuery(Expression<Func<T, bool>> expression)
            => _context.Set<T>().Where(expression);
    }
}