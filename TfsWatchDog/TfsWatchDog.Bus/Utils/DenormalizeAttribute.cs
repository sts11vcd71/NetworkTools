using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TfsWatchDog.Bus.Utils
{
    #region commented

    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    //public class DenormalizeAttribute : Attribute
    //{
    //    public DenormalizeAttribute(IDenormalizeSelector selector)
    //    {
    //    }

    //}

    //public class DenormalizeSelector<TSubject, TReference> : IDenormalizeSelector
    //{
    //    public DenormalizeSelector(Func<TSubject, object> sel)
    //    {

    //    }
    //}

    //public interface IDenormalizeSelector
    //{
    //}

    #endregion

    public class A
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class B
    {
        public int BId { get; set; }
        public DenormalizedObject<A, int> AMirror { get; set; } 
        public List<int> SelectedAIds { get; set; } 
        public DenormalizedList<A, int> SelectedAMirror { get; set; } 
    }
    public class Root
    {
        public Root()
        {
            ListOfAObjects = new List<A>();
            BObject = new B();
            BObject.AMirror = new DenormalizedObject<A, int>(() => BObject.BId, () => ListOfAObjects, x => x.Id);
            BObject.SelectedAMirror = new DenormalizedList<A, int>(() => BObject.SelectedAIds, x => ListOfAObjects.Find(t => t.Id == x), x => x.Id);
        }

        public List<A> ListOfAObjects { get; set; }
        public B BObject { get; set; }
    }

    public class DenormalizedObject<TObject, TKey>
    {
        private readonly Func<IList<TObject>> _objectsListSelector;
        private readonly Func<TObject, TKey> _keySelector;
        private readonly Func<TKey> _keyRetriever;
        public DenormalizedObject(Func<TKey> keyRetriever, Func<IList<TObject>> objectsListSelector, Func<TObject, TKey> keySelector)
        {
            _objectsListSelector = objectsListSelector;
            _keySelector = keySelector;
            _keyRetriever = keyRetriever;
        }

        public TObject Value
        {
            get
            {
                try
                {
                    TKey key = _keyRetriever();
                    return _objectsListSelector().FirstOrDefault(x => _keySelector(x).Equals(key));
                }
                catch (Exception)
                {
                    return default(TObject);
                }
            }
        }

        static public implicit operator TObject(DenormalizedObject<TObject, TKey> dobject)
        {
            return dobject.Value;
        }
    }

    // todo: use the same approach for selecting from the list
    public class DenormalizedList<TObject, TKey> : IList<TObject>
    {
        private readonly Func<IList<TKey>> _keysRetriver;
        private readonly Func<TKey, TObject> _selectorByKey;
        private readonly Func<TObject, TKey> _keyFromObject;

        public DenormalizedList(Func<IList<TKey>> keysRetriver, Func<TKey, TObject> selectorByKey, Func<TObject, TKey> keyFromObject)
        {
            _keysRetriver = keysRetriver;
            _selectorByKey = selectorByKey;
            _keyFromObject = keyFromObject;
        }

        public IEnumerator<TObject> GetEnumerator()
        {
            foreach (TKey key in _keysRetriver())
            {
                TObject obj;
                try
                {
                    obj = _selectorByKey(key);
                }
                catch (Exception)
                {
                    continue;
                }
                yield return obj;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TObject item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _keysRetriver().Clear();
        }

        public bool Contains(TObject item)
        {
            return _keysRetriver().Contains(_keyFromObject(item));
        }

        public void CopyTo(TObject[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TObject item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }

        public bool IsReadOnly
        {
            get { return _keysRetriver().IsReadOnly; }
        }

        public int IndexOf(TObject item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, TObject item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public TObject this[int index]
        {
            get
            {
                try
                {
                    return _selectorByKey(_keysRetriver()[index]);
                }
                catch (Exception)
                {
                    return default (TObject);
                }                
            }
            set
            {
                throw new NotImplementedException();
                
            }
        }
    }
}
