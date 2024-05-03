using Microsoft.EntityFrameworkCore;
using QuickChat.DataAccessLayer.Context;

namespace QuickChat.DataAccessLayer.Repositories
{
    public class EntityRepository
    {
        private MyIdentityDbContext _context;
        public EntityRepository(MyIdentityDbContext context)
        {
            _context = context;
        }
        #region Add Entity
        public async Task<bool> AddEntity<T>(T newEntity) where T : class
        {
            try
            {
                await _context.Set<T>().AddAsync(newEntity);
                var result = await _context.SaveChangesAsync();
                return result > 0;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete Entity
        public async Task<bool> DeleteEntity<T>(int id) where T : class
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    _context.Set<T>().Remove(entity);
                    var result = await _context.SaveChangesAsync();
                    return result > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Entity
        public async Task<T> GetEntity<T>(int id) where T : class
        {
            try
            {
                var entity = await _context.Set<T>()
                    .FindAsync(id);
                if (entity != null)
                {
                    return entity;
                }
                else
                {
                    throw new Exception("Entity Not Found");
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Entities
        public async Task<IEnumerable<T>> GetEntities<T>() where T : class
        {
            try
            {
                var entities = await _context.Set<T>().ToListAsync();
                if (entities != null)
                {
                    return entities;
                }
                else
                {
                    throw new Exception("Enities not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Edit Entity
        public async Task<bool> EditEntity<T>(int id, T newEntity) where T : class
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    _context.Entry<T>(entity).CurrentValues.SetValues(newEntity);
                    var result = await _context.SaveChangesAsync();
                    return result > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


    }
}
