using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BinaryStudio.PhotoGallery.Database.Exceptions;

namespace BinaryStudio.PhotoGallery.Database
{
    abstract class BaseRepository<TItem> : IBaseRepository<TItem> where TItem : class
    {
        protected DatabaseContext Context = null;

        protected BaseRepository(DatabaseContext dataBaseContext)
        {
            Context = dataBaseContext;
        }

        protected DbSet<TItem> DbSet
        {
            get { return this.Context.Set<TItem>(); }
        }

        /// <summary>
        /// Creates a new object(item) with TItem type in database.
        /// </summary>
        /// <exception cref="RepositoryCreateException">Says that repository cann't create new entry.</exception>
        public virtual TItem Add(TItem item)
        {
            try
            {
                TItem entry = DbSet.Add(item);
                Context.SaveChanges();

                return entry;
            }
            catch (Exception e)
            {
                throw new RepositoryCreateException(e);
            }
        }

        /// <summary>
        /// Checks if item with specific predicate exist in database.
        /// </summary>
        public bool Contains(Expression<Func<TItem, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        /// <summary>
        /// Gets all items from database.
        /// </summary>
        public virtual IQueryable<TItem> All()
        {
            return DbSet.AsQueryable();
        }

        /// <summary>
        /// Gets all items with specific predicate from database.
        /// </summary>
        public virtual IQueryable<TItem> Filter(Expression<Func<TItem, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable();
        }

        /// <summary>
        /// Finds first item with primary keys
        /// </summary>
        public virtual TItem Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        /// <summary>
        /// Finds first item with specific predicate
        /// </summary>
        public virtual TItem Find(Expression<Func<TItem, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Counts all items in database
        /// </summary>
        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        /// <summary>
        /// Update a state of item to Modified
        /// </summary>
        /// <exception cref="RepositoryUpdateException">Says that repository cann't update this entry. Maybe it is not present.</exception>
        public virtual void Update(TItem item)
        {
            try
            {
                Context.Entry(item).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new RepositoryUpdateException(e);
            }
        }

        /// <summary>
        /// Delete item from database
        /// </summary>
        /// <exception cref="RepositoryDeleteException">Says that repository cann't delete this entry. Maybe it is alredy deleted</exception>
        public virtual void Delete(TItem item)
        {
            try
            {
                DbSet.Remove(item);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new RepositoryDeleteException(e);
            }
        }

        /// <summary>
        /// Delete item with specific presicate from database
        /// </summary>
        public virtual void Delete(Expression<Func<TItem, bool>> predicate)
        {
            try
            {
                var items = Filter(predicate);

                foreach (var item in items)
                {
                    DbSet.Remove(item);
                }
            }
            catch (Exception e)
            {
                throw new RepositoryDeleteException(e);
            }
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}
