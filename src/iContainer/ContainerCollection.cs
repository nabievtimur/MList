using System.Collections;
using System.Collections.Generic;

namespace MList.Storage.Container
{
    public class ContainerCollection<T> : 
        ICollection<T>, IEnumerable<T>
        where T : iContainer, new()
    {
        private List<T> containers;
        public int Count => this.containers.Count;
        public bool IsReadOnly => false;
        public void Add(T item)
        {
            this.containers.Add(item);
        }
        public void Clear()
        {
            this.containers.Clear();
        }
        public bool Contains(T item)
        {
            return this.containers.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.containers.CopyTo(array, arrayIndex);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return this.containers.GetEnumerator();
        }
        public bool Remove(T item)
        {
            return this.containers.Remove(item);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.containers.GetEnumerator();
        }
    }
}
