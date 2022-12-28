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
        const string name = "table";
        public abstract ContainerCollection get();
        public abstract ContainerCollection get(string search);
        public abstract void add(iConteiner search);
        public abstract void update(iConteiner search);
        public void delete(long id)
        {
            SqLite.Delete(name, id);
        }
        public abstract void gridInit(DataGridView table);
        public void gridFill(DataGridView table, ContainerCollection collection)
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
