using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace BinaryStudio.PhotoGallery.Database
{
    abstract class BaseRepository<TItem> : IBaseRepository<TItem> where TItem : class
    {
        protected DatabaseContext context = null;

        protected BaseRepository(IDatabaseContext dataBaseContext)
        {
            context = dataBaseContext as DatabaseContext;
        }

        protected DbSet<TItem> DbSet
        {
            get { return context.Set<TItem>(); }
        }

        /// <summary>
        /// Creates a new object(item) with TItem type in database.
        /// </summary>
        public virtual void Create(TItem item)
        {
            DbSet.Add(item);
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
        /// Delete item from database
        /// </summary>
        public virtual int Delete(TItem item)
        {
            DbSet.Remove(item);

            return 0;
        }

        /// <summary>
        /// Update a state of item to Modified
        /// </summary>
        public virtual int Update(TItem item)
        {
            var entry = context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;

            return 0;
        }

        /// <summary>
        /// Delete item with specific presicate from database
        /// </summary>
        public virtual int Delete(Expression<Func<TItem, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);

            return 0;
        }

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }
    }
}
