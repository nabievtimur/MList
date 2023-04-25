using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using MList.Storage.Table.Container;

namespace MList.Storage.Table
{
    public class TableCar : iTable
    {
        public TableCar()
        {
            this.StorageTableName = "Машины";
        }
        public override iContainer getAssociatedContainer()
        {
            return new ContainerCar();
        }
        public override iContainer getAssociatedContainer(DataGridViewRow row)
        {
            return new ContainerCar(row);
        }
        public override void gridInit(DataGridView table)
        {
            base.gridInit(table);
            table.Columns.Add("brand", "Брэнд");
            table.Columns.Add("number", "Номер");
        }
        public override void storageAdd(iContainer container)
        {
            SqLite.exec(
                "INSERT INTO " + this.StorageTableName + " (brand, number) VALUES (@brand, @number)",
                    container.storageFillParameterCollection,
                "Add new adress.");
        }
        public override void storageUpdate(iContainer container)
        {
            SqLite.exec(
                "UPDATE " + this.StorageTableName + " SET brand = @brand, number = @number WHERE id = @id",
                container.storageFillParameterCollectionWithId,
                "Update address.");
        }
        public override void storageDelete(DataGridViewRow row)
        {
            storageDelete(new ContainerCar(row));
        }
        public override ContainerCollection<iContainer> storageGet()
        {
            return new ContainerCollection<ContainerCar>(SqLite.execGet(
                "SELECT c.id, c.brand, c.\"number\"  FROM " + this.StorageTableName + " c ",
                dFillerEmpty,
                "Read " + this.StorageTableName + ".")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(string search)
        {
            return new ContainerCollection<ContainerCar>(SqLite.execGet(
                "SELECT id, brand, number FROM " + this.StorageTableName + " as cr WHERE cr.brand LIKE @like OR cr.number LIKE @like",
                (SqliteCommand command) => dFillerSearcher(command, search),
                "Search " + this.StorageTableName + ".")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(long mlistId)
        {
            return new ContainerCollection<ContainerCar>(SqLite.execGet(
                "SELECT cr.id, cr.brand, cr.number FROM " + this.StorageTableName + " AS cr " +
                    "JOIN mlist_" + this.StorageTableName + " mc ON cr.id = mc.car_id WHERE mc.mlist_id = @mlist_id",
                (SqliteCommand command) => dFillerSCurrent(command, mlistId),
                "Read MList " + this.StorageTableName + "")).downCast();
        }
    }
}
