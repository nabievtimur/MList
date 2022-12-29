using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MList.Storage;
using MList.Storage.Container;

namespace MList.Storage.Table
{
    public class iTable<T> where T : iConteiner, new()
    {
        string name = "table";
        public ContainerCollection<T> storageGet()
        {
            T t = new T();
            return ContainerCollection<T>(SqLite.execGet(
                "SELECT id, address FROM addresses",
                t.storageFillParameterCollectionWithId(),
                "Read addresses."))
        }
        public abstract ContainerCollection<iConteiner> storageGet(string search);
        public abstract void storageAdd(iConteiner container);
        public abstract void storageUpdate(iConteiner container);
        public void storageDelete(long id)
        {
            SqLite.Delete(name, id);
        }
        public abstract void gridInit(DataGridView table);
        public void gridFill(DataGridView table, ContainerCollection<iConteiner> collection)
        {
            table.Rows.Clear();
            foreach (var it in collection)
            {
                table.Rows.Add();
                DataGridViewRow dataGridRow = table.Rows[table.Rows.GetLastRow(DataGridViewElementStates.None)];
                it.gridRowFill(ref dataGridRow);
            }
        }
    }
}
