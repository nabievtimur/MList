using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace MList.Storage
{
    [Serializable]
    public class ConnectionExeption : Exception
    {
        public ConnectionExeption() { }
        public ConnectionExeption(string message)
            : base(message) { }
    }

    [Serializable]
    public class FilesystemExeption : Exception
    {
        public FilesystemExeption() { }
        public FilesystemExeption(string message)
            : base(message) { }
    }

    [Serializable]
    public class QueryExeption : Exception
    {
        public QueryExeption() { }
        public QueryExeption(string message)
            : base(message) { }
    }
    public abstract class iConteiner
    {
        public long id;
        abstract public List<SqliteParameter> getByParametrList();
        abstract public List<SqliteParameter> getByParametrListWithId();
        abstract public DataGridViewRow fillRow(DataGridViewRow row);
        abstract public void fillItemList(ref List<Tuple<Label, TextBox>> lItems);
        static public Dictionary<int, iConteiner> fillTable(DataGridView table, List<iConteiner> containerList)
        {
            Dictionary<int, iConteiner> result = new Dictionary<int, iConteiner>();
            table.Rows.Clear();
            int i = 0;

            foreach (iConteiner cont in containerList)
            {
                table.Rows.Add();
                cont.fillRow(table.Rows[i]);
                result.Add(i++, cont);
            }
            table.Columns[0].Visible = false;

            return result;
        }
    }

    public class SqLite
    {
        // static
        private static SqLite instance;
        public static SqLite getInstance()
        {
            if (instance == null)
                instance = new SqLite();
            return instance;
        }
        static public SqliteConnection InitConnection()
        {
            SqliteConnection connection = null;

            string dbFolderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                "MList\\DataBase");
            string dbFilePath = Path.Combine(dbFolderPath, "mlist.db");

            // Проверяем существования БД
            if (File.Exists(dbFilePath))
            {
                try
                {
                    connection = new SqliteConnection(string.Format(
                        "Data Source={0};Cache=Shared;Mode=ReadWriteCreate;Foreign Keys=True;",
                        dbFilePath));
                    connection.Open();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    throw new ConnectionExeption("Init connection failed.");
                }
            }
            else
            {
                if (!Directory.Exists(dbFolderPath))
                {
                    try
                    {
                        Directory.CreateDirectory(dbFolderPath);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                        throw new FilesystemExeption("Create Directory failed.");
                    }
                }

                try
                {
                    connection = new SqliteConnection(string.Format(
                        "Data Source={0};Cache=Shared;Mode=ReadWriteCreate;Foreign Keys=True;",
                        dbFilePath));
                    connection.Open();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    throw new ConnectionExeption("Init connection failed.");
                }

                try
                {
                    new SqliteCommand(DbCreate.createDbSql, connection).ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    throw new QueryExeption("Create DataBase failed.");
                }
            }

            return connection;
        }
        static public void exec(String query, List<SqliteParameter> parametrs, String description)
        {
            using (var transaction = SqLite.getInstance().getConnection().BeginTransaction())
            {
                SqLite.exec(query, parametrs, description, transaction);
                transaction.Commit();
            }
        }
        static public void exec(String query, List<SqliteParameter> parametrs, String description, SqliteTransaction transaction)
        {
            SqliteCommand command = new SqliteCommand(query, SqLite.getInstance().getConnection(), transaction);

            if (parametrs != null)
            {
                foreach (SqliteParameter a in parametrs)
                {
                    command.Parameters.Add(a);
                }
            }

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                transaction.Rollback();
                throw new QueryExeption(description);
            }
        }
        static public SqliteDataReader execGet(String query, List<SqliteParameter> parametrs, String description)
        {
            using (var transaction = SqLite.getInstance().getConnection().BeginTransaction())
            {
                SqliteDataReader reader = SqLite.execGet(query, parametrs, description, transaction);
                transaction.Commit();
                return reader;
            }
        }
        static public SqliteDataReader execGet(String query, List<SqliteParameter> parametrs, String description, SqliteTransaction transaction)
        {
            SqliteCommand command = new SqliteCommand(query, SqLite.getInstance().getConnection(), transaction);

            foreach (SqliteParameter a in parametrs)
            {
                command.Parameters.Add(a);
            }

            try
            {
                return command.ExecuteReader();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                System.Diagnostics.Debug.WriteLine(e.ToString());
                throw new QueryExeption(description);
            }
        }
        static public void Delete(string DataBase, long id)
        {
            try
            {
                SqLite.exec(
                    "DELETE FROM " + DataBase + " WHERE id = @id",
                    new List<SqliteParameter> { new SqliteParameter("@id", id) },
                    "DELETE FROM " + DataBase);
            }
            catch(Exception)
            {
                throw new QueryExeption();
            }
        }

        // class
        private SqliteConnection connection;
        private SqLite() 
        {
            this.connection = InitConnection();
        }
        public SqliteConnection getConnection()
        {
            return this.connection;
        }
        
        public void Export(string path)
        {

        }

        public void Import(string path)
        {

        }
        static public void clearTable(DataGridView table)
        {
            while (table.Rows.Count > 1)
            {
                table.Rows.RemoveAt(table.Rows.Count - 1);
            }
        }
    }
}