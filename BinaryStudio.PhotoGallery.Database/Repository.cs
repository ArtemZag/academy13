using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Database
{
    internal class Repository<TObject> : IRepository<TObject> where TObject : class
    {
        public Repository(IDataBaseContext dataBaseContext)
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TObject> All()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TObject> Filter<TKey>(Expression<Func<TObject, bool>> filter, out int total, int index = 0, int size = 50)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TObject Find(params object[] keys)
        {
            throw new NotImplementedException();
        }

        public TObject Find(Expression<Func<TObject, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TObject Create(TObject item)
        {
            throw new NotImplementedException();
        }

        public void Delete(TObject item)
        {
            throw new NotImplementedException();
        }

        public int Delete(Expression<Func<TObject, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Update(TObject item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }
    }
}
