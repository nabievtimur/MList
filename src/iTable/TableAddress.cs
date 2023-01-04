using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Data.Sqlite;

using MList.Storage;
using MList.Storage.Container;

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
