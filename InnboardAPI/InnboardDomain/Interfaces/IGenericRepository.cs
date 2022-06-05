using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Interfaces
{
    public interface IGenericRepository <TEntity> where TEntity:class
    {
        TEntity Update(TEntity entity);
        TEntity Save(TEntity entity);
        List<TEntity> SaveAll(List<TEntity> entity);
        List<TEntity> TruncateAllAndInsert(List<TEntity> entity);        
        List<TEntity> TruncateAllAndInsertWithTransaction(List<TEntity> entity);        
        List<TEntity> InsertWithIdentity(List<TEntity> entity);        
        TEntity Insert(TEntity entity);
        int Delete(object id);
        int DeleteAll();
        int TruncateAll();
        int Delete(TEntity entity);
        TEntity Get(object Id);
        List<TEntity> GetAll();
    }
}
