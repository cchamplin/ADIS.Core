using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Configuration
{
    internal class ConfigurationItemList : IList<object>
    {
        private List<dynamic> data;
        public object locker;
        internal List<Guid> removals;

        public ConfigurationItemList()
        {
            locker = new object();
            data = new List<dynamic>();
            removals = new List<Guid>();
        }
       
        public void Add(dynamic item)
        {
            lock (locker)
            {
                if (item is Configuration)
                {
                    ((ICollection<dynamic>)data).Add(item);
                }
                else {
                    dynamic newItem = Configuration.FromInstance(item);
                    ((ICollection<dynamic>)data).Add(newItem);
                }
            }
        }


        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        public IEnumerator GetEnumerator()
        {

            return data.GetEnumerator();
        }
        IEnumerator<dynamic> System.Collections.Generic.IEnumerable<object>.GetEnumerator()
        {
            return data.GetEnumerator();
        }
        public int IndexOf(dynamic item)
        {
            return data.IndexOf(item);
        }
        public void Insert(int index, dynamic item)
        {
            lock (locker)
            {
                dynamic newItem = Configuration.FromInstance(item);
                data.Insert(index, newItem);
            }
        }
        public void RemoveAt(int index)
        {
            lock (locker)
            {
                data[index].removed = true;
                removals.Add(data[index].ID);
                data.RemoveAt(index);
            }
        }

        public bool Remove(dynamic item)
        {
            lock (locker)
            {
                removals.Add(item.ID);
                item.removed = true;
                return data.Remove(item);
            }
        }

        public dynamic this[int index]
        {
            get
            {
                return data[index];
            }
            set
            {
                data[index] = value;
            }
        }
        public void Clear()
        {
            lock (locker)
            {
                data.Clear();
            }
        }
        public int Count
        {
            get
            {
                return data.Count;
            }
        }
        public bool Contains(dynamic item)
        {
            return data.Contains(item);
        }

        public void CopyTo(dynamic[] array, int index)
        {
            throw new NotImplementedException();
        }
    }
}
