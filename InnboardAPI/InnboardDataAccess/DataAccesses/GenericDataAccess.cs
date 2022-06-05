
using InnboardDataAccess;
using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Web;

namespace InnboardAPI.DataAccesses
{
    public class GenericDataAccess<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal InnboardDbContext InnboardDBContext;
        internal DbSet<TEntity> InnboardDBSet;

        public GenericDataAccess()
        {
            this.InnboardDBContext = new InnboardDbContext();
            this.InnboardDBSet = InnboardDBContext.Set<TEntity>();
        }

        public int Delete(TEntity entity)
        {
            InnboardDBSet.Remove(entity);
            return SaveChanges();
        }

        public int Delete(object id)
        {
            var entity = InnboardDBSet.Find(id);
            return Delete(entity);
        }

        public virtual TEntity Get(object Id)
        {
            return InnboardDBSet.Find(Id);
        }

        public List<TEntity> GetAll()
        {
            return InnboardDBSet.ToList();
        }

        public virtual TEntity Save(TEntity entity)
        {
            InnboardDBSet.Add(entity);
            SaveChanges();
            return entity;
        }

        public virtual List<TEntity> SaveAll(List<TEntity> entity)
        {
            InnboardDBSet.AddRange(entity);
            SaveChanges();
            return entity;
        }
        public virtual List<TEntity> TruncateAllAndInsert(List<TEntity> entity)
        {
            TruncateAll();
            InsertWithIdentity(entity);
            return entity;
        }
        public virtual List<TEntity> TruncateAllAndInsertWithTransaction(List<TEntity> entity)
        {
            using (var transaction = InnboardDBContext.Database.BeginTransaction())
            {
                TruncateAll();
                InsertWithIdentity(entity);
                transaction.Commit();
                return entity;
            }
        }
        public virtual List<TEntity> InsertWithIdentity(List<TEntity> entity)
        {
            string tableName = typeof(TEntity).Name;
            InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[" + tableName + "] ON");
            InnboardDBSet.AddRange(entity);
            SaveChanges();
            InnboardDBContext.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[" + tableName + "] OFF");            
            return entity;
        }

        public virtual TEntity Insert(TEntity entity)
        {
            InnboardDBSet.Add(entity);
            SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            InnboardDBContext.Entry(entity).State = EntityState.Modified;
            SaveChanges();

            return entity;
        }

        public virtual int SaveChanges()
        {
            try
            {
                return InnboardDBContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
        }

        public int DeleteAll()
        {
            string tableName = typeof(TEntity).Name;
            InnboardDBContext.Database.ExecuteSqlCommand("DELETE FROM " + tableName);
            return SaveChanges();
        }
        public int TruncateAll()
        {
            string tableName = typeof(TEntity).Name;
            InnboardDBContext.Database.ExecuteSqlCommand("TRUNCATE TABLE " + tableName);
            return SaveChanges();
        }
    }
}