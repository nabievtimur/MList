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
            throw new NotImplementedException();
        }

        public override ContainerCollection<iContainer> storageGet()
        {
            throw new NotImplementedException();
        }

        public override ContainerCollection<iContainer> storageGet(string search)
        {
            throw new NotImplementedException();
        }

        public override void storageUpdate(iContainer container)
        {
            throw new NotImplementedException();
        }
    }
}
