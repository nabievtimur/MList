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
    public class TableEmployee : iTable
    {
        public override void gridInit(DataGridView table)
        {
            table.Columns.Add("lastName", "Фамилия");
            table.Columns.Add("firstName", "Имя");
            table.Columns.Add("middleName", "Отчество");
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
