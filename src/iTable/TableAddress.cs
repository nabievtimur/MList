using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using MList.Storage.Table.Container;

namespace MList.Storage.Table
{
    public class TableAddress : iTable
    {
        public override void gridInit(DataGridView table)
        {
            table.Columns.Add("id", "id");
            table.Columns.Add("address", "Адрес");
        }
        public override void storageAdd(iContainer container)
        {
            if (container is ContainerAddress)
            {
                SqLite.exec(
                    "INSERT INTO addresses (address) VALUES (@address)",
                    container.storageFillParameterCollection,
                    "Add new adress."); ;
            }
            throw new NotSupportedException();
        }
        public override void storageUpdate(iContainer container)
        {
            SqLite.exec(
                "UPDATE addresses SET address = @address WHERE id = @id",
                container.storageFillParameterCollectionWithId,
                "Update address.");
        }
        public override void storageDelete(DataGridViewRow row)
        {
            storageDelete(new ContainerAddress(row));
        }
        public override ContainerCollection<iContainer> storageGet()
        {
            return new ContainerCollection<ContainerAddress>(SqLite.execGet(
                "SELECT id, address FROM addresses",
                dFillerEmpty,
                "Read addresses.")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(string search)
        {
            return new ContainerCollection<ContainerAddress>(SqLite.execGet(
                "SELECT id, address FROM addresses as ad WHERE ad.address LIKE @like",
                (SqliteCommand command) => dFillerSearcher(command, search),
                "Search address.")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(long mlistId)
        {
            throw new NotImplementedException();
        }
        public ContainerCollection<iContainer> storageGetCurrentArrive(long mlistId)
        {
            return new ContainerCollection<ContainerAddress>(SqLite.execGet(
                "SELECT ad.id, ad.address FROM addresses AS ad " +
                    "JOIN mlist_arrive_address maa ON ad.id = maa.arrive_address_id WHERE maa.mlist_id = @mlist_id",
                (SqliteCommand command) => dFillerSCurrent(command, mlistId),
                "Read MList addresses arrive")).downCast();
        }
        public ContainerCollection<iContainer> storageGetCurrentDeep(long mlistId)
        {
            return new ContainerCollection<ContainerAddress>(SqLite.execGet(
                "SELECT ad.id, ad.address FROM addresses AS ad " +
                    "JOIN mlist_deep_address maa ON ad.id = maa.deep_address_id WHERE maa.mlist_id = @mlist_id",
                (SqliteCommand command) => dFillerSCurrent(command, mlistId),
                "Read MList addresses deep")).downCast();
        }
    }
}
