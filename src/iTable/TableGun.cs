using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using MList.Storage.Table.Container;

namespace MList.Storage.Table
{
    public class TableGun : iTable
    {
        public TableGun()
        {
            this.VisibleTableName = "Оружие";
            this.StorageTableName = "guns";
        }

        public override iContainer getAssociatedContainer()
        {
            return new ContainerGun();
        }

        public override iContainer getAssociatedContainer(DataGridViewRow row)
        {
            return new ContainerGun(row);
        }

        public override void gridInit(DataGridView table)
        {
            base.gridInit(table);
            table.Columns.Add("brand", "Брэнд");
            table.Columns.Add("series", "Серия");
            table.Columns.Add("number", "Номер");
            table.Columns.Add("ammo", "Патроны");
        }

        public override void storageAdd(iContainer container)
        {
            SqLite.exec(
                "INSERT INTO " + this.StorageTableName + " (brand, series, number, ammo) " +
                "VALUES (@brand, @series, @number, @ammo)",
                container.storageFillParameterCollection,
                "Add new gun.");
        }

        public override void storageUpdate(iContainer container)
        {
            SqLite.exec(
                "UPDATE " + this.StorageTableName +
                " SET brand = @brand, series = @series, number = @number, ammo = @ammo WHERE id = @id",
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
                "SELECT id, brand, series, number, ammo FROM " + this.StorageTableName,
                dFillerEmpty,
                "Read " + this.StorageTableName + ".")).downCast();
        }

        public override ContainerCollection<iContainer> storageGet(string search)
        {
            return new ContainerCollection<ContainerGun>(SqLite.execGet(
                "SELECT g.id, g.brand, g.series, g.number, g.ammo FROM " + this.StorageTableName + " g " +
                "WHERE g.brand LIKE @like OR g.series LIKE @like OR g.\"number\" LIKE @like or g.ammo LIKE @like " +
                "ORDER BY g.brand;",
                (SqliteCommand command) => dFillerSearcher(command, search),
                "Read " + this.StorageTableName + ".")).downCast();
        }

        public ContainerCollection<ContainerGun> storageGetCurrent(long MMlistId)
        {
            return new ContainerCollection<ContainerGun>(SqLite.execGet(
                "SELECT gn.id, gn.brand, gn.series, gn.number, gn.ammo FROM " + this.StorageTableName + " AS gn " +
                "JOIN mlist_gun mg ON gn.id = mg.gun_id WHERE mg.mlist_id = @mlist_id",
                (SqliteCommand command) => dFillerSCurrent(command, MMlistId),
                "Get " + this.StorageTableName + " by mlist."));
        }

        public ContainerCollection<ContainerGun> storageGetCurrentByOrder(long mOrderId)
        {
            return new ContainerCollection<ContainerGun>(SqLite.execGet(
                "SELECT gn.id, gn.brand, gn.series, gn.number, gn.ammo FROM " + this.StorageTableName + " AS gn " +
                "JOIN order_gun og ON gn.id = og.gun_id WHERE og.order_id = @order_id",
                (SqliteCommand command) => { command.Parameters.Add(new SqliteParameter("@order_id", mOrderId)); },
                "Get " + this.StorageTableName + " by order."));
        }

        public ContainerCollection<ContainerGun> storageGetCurrentByEmployee(long mEmployeeId)
        {
            return new ContainerCollection<ContainerGun>(SqLite.execGet(
                "SELECT gn.id, gn.brand, gn.series, gn.number, gn.ammo FROM " + this.StorageTableName + " AS gn " +
                "JOIN order_gun og ON og.gun_id = gn.id " +
                "JOIN orders o ON o.id = og.order_id " +
                "WHERE o.employee_id = @employee_id",
                (SqliteCommand command) => { command.Parameters.Add(new SqliteParameter("@employee_id", mEmployeeId)); },
                "Get " + this.StorageTableName + " by employee."));
        }

        public ContainerCollection<ContainerGun> storageGetCurrentByEmployee(long mEmployeeId, string like)
        {
            return new ContainerCollection<ContainerGun>(SqLite.execGet(
                "SELECT gn.id, gn.brand, gn.series, gn.number, gn.ammo FROM " + this.StorageTableName + " AS gn " +
                "JOIN order_gun og ON og.gun_id = gn.id " +
                "JOIN orders o ON o.id = og.order_id " +
                "WHERE o.employee_id = @employee_id " +
                "AND (gn.brand LIKE @like OR gn.series LIKE @like OR gn.number LIKE @like OR gn.ammo LIKE @like)",
                (SqliteCommand command) =>
                {
                    command.Parameters.Add(new SqliteParameter("@employee_id", mEmployeeId));
                    command.Parameters.Add(new SqliteParameter("@like", like));
                },
                "Get " + this.StorageTableName + " by employee and like param."));
        }
    }
}