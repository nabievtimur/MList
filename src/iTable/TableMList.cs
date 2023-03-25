using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using MList.Storage.Table.Container;

namespace MList.Storage.Table
{
    public class TableMList : iTable
    {
        public override void gridInit(DataGridView table)
        {
            table.Columns.Add("number", "Номер");
            table.Columns.Add("employee", "Сотрудник");
            table.Columns.Add("createDate", "Дата создания");
            table.Columns.Add("createStart", "Дата начала");
            table.Columns.Add("startTime", "Время начала");
            table.Columns.Add("endDate", "Дата окончания");
            table.Columns.Add("endTime", "Время окончания");
            table.Columns.Add("instractionTime", "Время инструктажа");
            table.Columns.Add("returnGunDate", "Дата сдачи оружия");
            table.Columns.Add("returnGunTime", "Время сдачи оружия");
            table.Columns.Add("printDate", "Дата печати");
            table.Columns.Add("description", "Примечание");
        }
        public override void storageAdd(iContainer container)
        {
            SqLite.exec(
                "INSERT INTO mlist (date_create, date_begin, end_date, coach_date, pass_gun_date, print_date, notes, deep_time, arrive_time, pass_gun_time, num_mlist)" +
                "VALUES (@date_create, @date_begin, @end_date, @coach_date, @pass_gun_date, @print_date, @notes, @deep_time, @arrive_time, @pass_gun_time, @num_mlist)",
                container.storageFillParameterCollection,
                "Add new MLlist");
        }
        public override void storageUpdate(iContainer container)
        {
            SqLite.exec(
                "UPDATE mlist SET " +
                        "date_create   = @date_create," +
                        "date_begin    = @date_begin," +
                        "end_date      = @end_date," +
                        "coach_date    = @coach_date," +
                        "pass_gun_date = @pass_gun_date," +
                        "print_date    = @print_date," +
                        "notes         = @notes," +
                        "deep_time     = @deep_time," +
                        "arrive_time   = @arrive_time," +
                        "pass_gun_time = @pass_gun_time," +
                        "num_mlist     = @num_mlist" +
                        "where id = @id",
                container.storageFillParameterCollectionWithId,
                "Update MList");
        }
        public override void storageDelete(DataGridViewRow row)
        {
            storageDelete(new ContainerMList(row));
        }
        public override ContainerCollection<iContainer> storageGet()
        {
            return new ContainerCollection<iContainer>();
        }
        public ContainerCollection<ContainerMList> storageGetMLists()
        {
            return new ContainerCollection<ContainerMList>(SqLite.execGet(
                "SELECT ml.id, " +
                    "ml.num_mlist, " +
                    "ml.date_create, " +
                    "ml.date_begin, " +
                    "ml.end_date, " +
                    "ml.coach_date, " +
                    "ml.pass_gun_date, " +
                    "ml.pass_gun_time, " +
                    "ml.print_date, " +
                    "ml.notes, " +
                    "ml.arrive_time, " +
                    "ml.deep_time, " +
                    "e.id, " +
                    "e.last_name, " +
                    "e.first_name, " +
                    "e.middle_name " +
                    "FROM mlist AS ml " +
                    "JOIN mlist_employees me ON ml.id = me.mlist_id " +
                    "JOIN employees e ON e.id = me.employee_id",
                dFillerEmpty,
                "Reads MList."));
        }
        public override ContainerCollection<iContainer> storageGet(string search)
        {
            throw new NotImplementedException();
        }
        public ContainerCollection<ContainerMList> storageGetMLists(string search)
        {
            return new ContainerCollection<ContainerMList>(SqLite.execGet(
                "SELECT ml.id, " +
                    "ml.num_mlist, " +
                    "ml.date_create, " +
                    "ml.date_begin, " +
                    "ml.end_date, " +
                    "ml.coach_date, " +
                    "ml.pass_gun_date, " +
                    "ml.pass_gun_time, " +
                    "ml.print_date, " +
                    "ml.notes, " +
                    "ml.arrive_time, " +
                    "ml.deep_time, " +
                    "e.id, " +
                    "e.last_name, " +
                    "e.first_name, " +
                    "e.middle_name " +
                    "FROM mlist AS ml " +
                    "JOIN mlist_employees me ON ml.id = me.mlist_id " +
                    "JOIN employees e ON e.id = me.employee_id",
                dFillerEmpty,
                "Reads MList."));
        }
        public override ContainerCollection<iContainer> storageGet(long mlistId)
        {
            throw new NotImplementedException();
        }
    }
}
