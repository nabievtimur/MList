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
    public class TableCar : iTable
    {
        public override void gridInit(DataGridView table)
        {
            table.Columns.Add("brand", "Брэнд");
            table.Columns.Add("number", "Номер");
        }
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
