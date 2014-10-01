﻿//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Data.Entity;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ServiceBlock.Foundation.Data;

//namespace ServiceBlock.Foundation.Fakes
//{
//    [ExcludeFromCodeCoverage]
//    public class FakeRepository<T> : IDbSet<T>
//        where T : class
//    {
//        ObservableCollection<T> _data;
//        IQueryable _query;

//        public FakeRepository()
//        {
//            _data = new ObservableCollection<T>();
//            _query = _data.AsQueryable();
//        }

//        public virtual T Find(params object[] keyValues)
//        {
//            throw new NotImplementedException("No default implementation for Find method.  Either use SingleOrDefault method, or derive from FakeRepository<T> and override Find.");
//        }

//        public T Add(T item)
//        {
//            _data.Add(item);
//            return item;
//        }

//        public T Remove(T item)
//        {
//            _data.Remove(item);
//            return item;
//        }

//        public T Attach(T item)
//        {
//            _data.Add(item);
//            return item;
//        }

//        public T Detach(T item)
//        {
//            _data.Remove(item);
//            return item;
//        }

//        public T Create()
//        {
//            return Activator.CreateInstance<T>();
//        }

//        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
//        {
//            return Activator.CreateInstance<TDerivedEntity>();
//        }

//        public ObservableCollection<T> Local
//        {
//            get { return _data; }
//        }

//        Type IQueryable.ElementType
//        {
//            get { return _query.ElementType; }
//        }

//        System.Linq.Expressions.Expression IQueryable.Expression
//        {
//            get { return _query.Expression; }
//        }

//        IQueryProvider IQueryable.Provider
//        {
//            get { return _query.Provider; }
//        }

//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//        {
//            return _data.GetEnumerator();
//        }

//        IEnumerator<T> IEnumerable<T>.GetEnumerator()
//        {
//            return _data.GetEnumerator();
//        }
//    }
//}
