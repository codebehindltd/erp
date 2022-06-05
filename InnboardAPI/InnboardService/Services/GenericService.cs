using InnboardConfiguration;
using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using InnboardDomain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class GenericService<TEntity> : BaseService where TEntity : class
    {
        protected T GetInstance<T>() where T : IGenericRepository<TEntity>
        {
            var repository = ServiceLocator.GetInstance<T>();
            
            return repository;

        }

        public virtual Response<TEntity> Update(TEntity entity)
        {
            var repository = GetInstance<IGenericRepository<TEntity>>();
            var result = SafeExecute(() => repository.Update(entity));
            return result;
        }

        public virtual Response<TEntity> Save(TEntity entity)
        {
            var repository = GetInstance<IGenericRepository<TEntity>>();
            var result = SafeExecute(() => repository.Save(entity));
            return result;
        }

        public virtual Response<List<TEntity>> SaveAll(List<TEntity> entity)
        {
            var repository = GetInstance<IGenericRepository<TEntity>>();
            var result = SafeExecute(() => repository.SaveAll(entity));
            return result;
        }

        public virtual Response<List<TEntity>> TruncateAllAndInsert(List<TEntity> entity)
        {
            var repository = GetInstance<IGenericRepository<TEntity>>();
            var result = SafeExecute(() => repository.TruncateAllAndInsert(entity));
            return result;
        }
        public virtual Response<List<TEntity>> TruncateAllAndInsertWithTransaction(List<TEntity> entity)
        {
            var repository = GetInstance<IGenericRepository<TEntity>>();
            var result = SafeExecute(() => repository.TruncateAllAndInsertWithTransaction(entity));
            return result;
        }

        public virtual Response<List<TEntity>> InsertWithIdentity(List<TEntity> entity)
        {
            var repository = GetInstance<IGenericRepository<TEntity>>();
            var result = SafeExecute(() => repository.InsertWithIdentity(entity));
            return result;
        }


        public virtual Response<int> Delete(object id)
        {
            var repository = GetInstance<IGenericRepository<TEntity>>();
            var result = SafeExecute(() => repository.Delete(id));
            return result;
        }

        public virtual Response<int> Delete(TEntity entity)
        {
            var repository = GetInstance<IGenericRepository<TEntity>>();
            var result = SafeExecute(() => repository.Delete(entity));
            return result;
        }

        public virtual Response<int> DeleteAll()
        {
            var repository = GetInstance<IGenericRepository<TEntity>>();
            var result = SafeExecute(() => repository.DeleteAll());
            return result;
        }

        public virtual Response<int> TruncateAll()
        {
            var repository = GetInstance<IGenericRepository<TEntity>>();
            var result = SafeExecute(() => repository.TruncateAll());
            return result;
        }

        public virtual Response<TEntity> Get(object id)
        {
            var repository = GetInstance<IGenericRepository<TEntity>>();
            var result = SafeExecute(() => repository.Get(id));
            return result;
        }       

        public virtual List<TEntity> GetAll()
        {
            var repository = GetInstance<IGenericRepository<TEntity>>();
            var result = SafeExecute(() => repository.GetAll());
            return result.Data;
        }

        public Response<TEntity> ConvertDateTimeUTCtoLocalTime(TEntity entity)
        {
            var result = SafeExecute(() => CommonHelper.ConvertDateTimePropertiesUTCtoLocalTime(entity));
            return result;
        }
        

    }
}
