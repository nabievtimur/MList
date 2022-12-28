using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MList.Storage.Container
{
    public class ContainerCollection : ICollection<iConteiner>, IEnumerable<iConteiner>
    {
        List<iConteiner> containers;

        public int Count => this.containers.Count;

        public bool IsReadOnly => false;

        public void Add(iConteiner item)
        {
            this.containers.Add(item);
        }

        public void Clear()
        {
            this.containers.Clear();
        }

        public bool Contains(iConteiner item)
        {
            return this.containers.Contains(item);
        }

        public void CopyTo(iConteiner[] array, int arrayIndex)
        {
            this.containers.CopyTo(array, arrayIndex);
        }

        public IEnumerator<iConteiner> GetEnumerator()
        {
            return this.containers.GetEnumerator();
        }

        public bool Remove(iConteiner item)
        {
            return containers.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.containers.GetEnumerator();
        }
    }
}
