using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Storage.Table.Container
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
    public class iContainer
    {
        protected long id;
        public long getId() { return this.id; }
        public iContainer()
        {
            this.id = -1;
        }
        public iContainer(long id)
        {
            this.id = id;
        }
        public iContainer(DataGridViewRow row) : this()
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
        public virtual void storageFill(SqliteDataReader reader)
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
        
        public virtual void storageFillParameterCollection(SqliteCommand command)
        {
            command.Parameters.Clear();
        }
        public virtual void storageFillParameterCollectionWithId(SqliteCommand command)
        {
            command.Parameters.Add(new SqliteParameter("@id", this.id));
            this.storageFillParameterCollection(command);
        }

        // работа с стркой таблицы 
        public virtual void gridRowFill(ref DataGridViewRow row)
        {
            row.Cells[0].Value = this.id;
        }
        public virtual void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
        {
            lItems.Clear();
        }
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
