using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using MList.Storage.Table.Container;

namespace MList.Storage.Table
{
    public class ContainerCollection<T> : 
        ICollection<T>, IEnumerable<T>
        where T : iContainer, new()
    {
        private List<T> containers;
        public ContainerCollection()
        {
            this.containers = new List<T>();
        }
        public ContainerCollection(SqliteDataReader reader) : this()
        {
            try
            {
                while (reader.Read()) // построчно считываем данные
                {
                    T container = new T();
                    container.storageFill(reader);
                    this.containers.Add(container);
                }
                reader.Close();
            }
            catch (Exception)
            {
                reader.Close();
                throw new QueryExeption();
            }
        }
        public ContainerCollection(List<T> containers)
        {
            this.containers = containers;
        }
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
        public ContainerCollection<iContainer> downCast()
        {
            return new ContainerCollection<iContainer>(((ContainerCollection<T>)this).Cast<iContainer>().ToList());
        }

    }
}
