using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using MList.Storage.Table.Container;

namespace MList.Storage.Table
{
    public abstract class iTable
    {
        private string StorageTableName = "table";
        public abstract ContainerCollection<iContainer> storageGet();
        public abstract ContainerCollection<iContainer> storageGet(string search);
        public abstract ContainerCollection<iContainer> storageGet(long mlistId);
        public abstract void storageAdd(iContainer container);
        public abstract void storageUpdate(iContainer container);
        public void storageDelete(iContainer container)
        {
            SqLite.Delete(this.StorageTableName, container.getId());
        }
        public abstract void storageDelete(DataGridViewRow row);
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
        public static void dFillerEmpty(SqliteCommand command) { }
        public static void dFillerSearcher(SqliteCommand command, string search) { command.Parameters.Add(new SqliteParameter("@like", "%" + search + "%")); }
        public static void dFillerSCurrent(SqliteCommand command, long mlistId) { command.Parameters.Add(new SqliteParameter("@mlist_id", mlistId)); }
    }
}
