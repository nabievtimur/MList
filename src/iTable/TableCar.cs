using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using MList.Storage.Table.Container;

namespace MList.Storage.Table
{
    public class TableCar : iTable
    {
        public override void gridInit(DataGridView table)
        {
            table.Columns.Add("brand", "Брэнд");
            table.Columns.Add("number", "Номер");
        }
        public override void storageAdd(iContainer container)
        {
            SqLite.exec(
                "INSERT INTO cars (brand, number) VALUES (@brand, @number)",
                    container.storageFillParameterCollection,
                "Add new adress.");
        }
        public override void storageUpdate(iContainer container)
        {
            SqLite.exec(
                "UPDATE cars SET brand = @brand, number = @number WHERE id = @id",
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
                "SELECT c.id, c.brand, c.\"number\"  FROM cars c ",
                dFillerEmpty,
                "Read cars.")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(string search)
        {
            return new ContainerCollection<ContainerCar>(SqLite.execGet(
                "SELECT id, brand, number FROM cars as cr WHERE cr.brand LIKE @like OR cr.number LIKE @like",
                (SqliteCommand command) => dFillerSearcher(command, search),
                "Search cars.")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(long mlistId)
        {
            return new ContainerCollection<ContainerCar>(SqLite.execGet(
                "SELECT cr.id, cr.brand, cr.number FROM cars AS cr " +
                    "JOIN mlist_cars mc ON cr.id = mc.car_id WHERE mc.mlist_id = @mlist_id",
                (SqliteCommand command) => dFillerSCurrent(command, mlistId),
                "Read MList cars")).downCast();
        }
    }
}
