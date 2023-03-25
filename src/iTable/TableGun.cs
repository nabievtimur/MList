using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using MList.Storage.Table.Container;

namespace MList.Storage.Table
{
    public class TableGun : iTable
    {
        public override void gridInit(DataGridView table)
        {
            table.Columns.Add("brand", "Брэнд");
            table.Columns.Add("series", "Серия");
            table.Columns.Add("number", "Номер");
            table.Columns.Add("ammo", "Патроны");
        }
        public override void storageAdd(iContainer container)
        {
            SqLite.exec(
                "INSERT INTO guns (brand, series, number, ammo) " +
                    "VALUES (@brand, @series, @number, @ammo)",
                container.storageFillParameterCollection,
                "Add new gun.");
        }
        public override void storageUpdate(iContainer container)
        {
            SqLite.exec(
                "UPDATE guns SET brand = @brand, series = @series, number = @number, ammo = @ammo WHERE id = @id",
                container.storageFillParameterCollectionWithId,
                "Update gun.");
        }
        public override void storageDelete(DataGridViewRow row)
        {
            storageDelete(new ContainerGun(row));
        }
        public override ContainerCollection<iContainer> storageGet()
        {
            return new ContainerCollection<ContainerGun>(SqLite.execGet(
                "SELECT id, brand, series, number, ammo FROM guns",
                dFillerEmpty,
                "Read guns.")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(string search)
        {
            return new ContainerCollection<ContainerGun>(SqLite.execGet(
                "SELECT g.id, g.brand, g.series, g.number, g.ammo FROM guns g " +
                    "WHERE g.brand LIKE @like OR g.series LIKE @like OR g.\"number\" LIKE @like or g.ammo LIKE @like " +
                    "ORDER BY g.brand;",
                (SqliteCommand command) => dFillerSearcher(command, search),
                "Read guns.")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(long mlistId)
        {
            return new ContainerCollection<ContainerGun>(SqLite.execGet(
                "SELECT gn.id, gn.number, gn.brand, gn.series, gn.ammo FROM guns AS gn " +
                    "JOIN mlist_gun mg ON gn.id = mg.gun_id WHERE mg.mlist_id = @mlist_id",
                (SqliteCommand command) => dFillerSCurrent(command, mlistId),
                "Get guns by mlist.")).downCast();
        }
        public ContainerCollection<iContainer> storageGetCurrent(long mOrderId)
        {
            return new ContainerCollection<ContainerGun>(SqLite.execGet(
                "SELECT gn.id, gn.number, gn.ammo, gn.series, gn.brand FROM guns AS gn " +
                    "JOIN order_gun og ON gn.id = og.gun_id WHERE og.order_id = @order_id",
                (SqliteCommand command) => dFillerSCurrent(command, mOrderId),
                "Get guns by order.")).downCast();
        }
    }
}
