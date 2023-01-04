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
    public abstract class iTable
    {
        private const string StorageTableName = "table";
        public abstract ContainerCollection<iContainer> storageGet();
        public abstract ContainerCollection<iContainer> storageGet(string search);
        public abstract void storageAdd(iContainer container);
        public abstract void storageUpdate(iContainer container);
        public void storageDelete(iContainer container)
        {
            SqLite.Delete(this.StorageTableName, container.getId());
        }
        public abstract void gridInit(DataGridView table);
        public void gridFill(DataGridView table, ContainerCollection<iContainer> collection)
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
