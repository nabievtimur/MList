using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using MList.Storage.Table.Container;

namespace MList.Storage.Table
{
    public class TableEmployee : iTable
    {
        public TableEmployee()
        {
            this.StorageTableName = "Работники";
        }
        public override iContainer getAssociatedContainer()
        {
            return new ContainerEmployee();
        }
        public override iContainer getAssociatedContainer(DataGridViewRow row)
        {
            return new ContainerEmployee(row);
        }
        public override void gridInit(DataGridView table)
        {
            base.gridInit(table);
            table.Columns.Add("lastName", "Фамилия");
            table.Columns.Add("firstName", "Имя");
            table.Columns.Add("middleName", "Отчество");
        }
        public override void storageAdd(iContainer container)
        {
            SqLite.exec(
                "INSERT INTO " + this.StorageTableName + " (first_name, last_name, middle_name) VALUES (@first_name, @last_name, @middle_name)",
                container.storageFillParameterCollection,
                "Add new Employee.");
        }
        public override void storageUpdate(iContainer container)
        {
            SqLite.exec(
                "UPDATE " + this.StorageTableName + " SET first_name = @first_name, last_name = @last_name, middle_name = @middle_name WHERE id = @id",
                container.storageFillParameterCollectionWithId,
                "Update Employee.");
        }
        public override void storageDelete(DataGridViewRow row)
        {
            storageDelete(new ContainerEmployee(row));
        }
        public override ContainerCollection<iContainer> storageGet()
        {
            return new ContainerCollection<ContainerEmployee>(SqLite.execGet(
                "SELECT id, first_name, last_name, middle_name FROM " + this.StorageTableName,
                dFillerEmpty,
                "Search " + this.StorageTableName + ".")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(string search)
        {
            return new ContainerCollection<ContainerEmployee>(SqLite.execGet(
                "SELECT id, first_name, last_name, middle_name FROM " + this.StorageTableName + " " +
                    "WHERE first_name LIKE @like OR last_name LIKE @like OR middle_name LIKE @like " +
                    "ORDER BY id;",
                (SqliteCommand command) => dFillerSearcher(command, search),
                "Search employee.")).downCast();
        }
        public override ContainerCollection<iContainer> storageGet(long mlistId)
        {
            throw new NotImplementedException();
        }
    }
}
