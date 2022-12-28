using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Storage.Container
{
    [Serializable]
    public class ParceException : Exception
    {
        public ParceException() { }
        public ParceException(string message)
            : base("Parce " + message + " exeption.") { }
    }

    /* brief
     *      Базовый класс хранения элемента базы данных.
     */
    public abstract class iConteiner
    {
        protected long id;
        public iConteiner()
        {
            this.id = -1;
        }
        public iConteiner(long id)
        {
            this.id = id;
        }
        public iConteiner(DataGridViewRow row) : this()
        {
            try
            {
                this.id = getLongFromCell(row.Cells[0]);
            }
            catch (IndexOutOfRangeException) 
            {
                throw new ParceException("DataGridViewRow"); 
            }
        }
        public iConteiner(SqliteDataReader reader) : this()
        {
            try
            {
                this.id = reader.GetInt64(0);
            }
            catch (Exception)
            {
                throw new ParceException("SqliteDataReader");
            }
        }
        abstract public void storageFillParameterCollection(ref SqliteParameterCollection parameterCollection);
        public void storageFillParameterCollectionWithId(ref SqliteParameterCollection parameterCollection)
        {
            parameterCollection.Add(new SqliteParameter("@id", this.id));
            this.storageFillParameterCollection(ref parameterCollection);
        }

        // работа с стркой таблицы 
        virtual public void gridRowFill(ref DataGridViewRow row)
        {
            row.Cells[0].Value = this.id;
        }
        abstract public void fillItemList(ref List<Tuple<Label, TextBox>> lItems);

        //static public Dictionary<int, iConteiner> fillTable(DataGridView table, List<iConteiner> containerList)
        //{
        //    Dictionary<int, iConteiner> result = new Dictionary<int, iConteiner>();
        //    table.Rows.Clear();
        //    int i = 0;
        //
        //    foreach (iConteiner cont in containerList)
        //    {
        //        table.Rows.Add();
        //        cont.fillRow(table.Rows[i]);
        //        result.Add(i++, cont);
        //    }
        //    table.Columns[0].Visible = false;
        //
        //    return result;
        //}

        protected string getStringFromCell(DataGridViewCell cell)
        {
            try
            {
                return cell.Value.ToString();
            }
            catch(Exception) { throw new ParceException("DataGridViewCell"); }
        }

        protected long getLongFromCell(DataGridViewCell cell)
        {
            try
            {
                return long.Parse(cell.Value.ToString());
            }
            catch (Exception) { throw new ParceException("DataGridViewCell"); }
        }
    }
}
