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
        public override ContainerCollection storageGet()
        {
            ContainerCollection collection = new ContainerCollection();
            Address address = new Address();

            SqliteDataReader reader = SqLite.execGet(
                "SELECT id, address FROM addresses",
                address.storageFillParameterCollectionWithId(),
                "Read addresses.")

            try
            {
                while (reader.Read()) // построчно считываем данные
                {
                    list.Add(new Address
                    {
                        id = reader.GetInt64(0),
                        address = reader.GetString(1)
                    });
                }
                reader.Close();
            }
            catch (Exception)
            {
                reader.Close();
                throw new QueryExeption();
            }

            return list;
        }
        public override ContainerCollection storageGet(string search)
        {
            throw new NotImplementedException();
        }
        public override void storageAdd(iConteiner search)
        {
            throw new NotImplementedException();
        }
        public override void storageUpdate(iConteiner search)
        {
            throw new NotImplementedException();
        }
        public override void gridInit(DataGridView table)
        {
            table.Columns.Add("id", "id");
            table.Columns.Add("address", "Адрес");
        }
    }
}
