using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.IO;

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
                transaction.Commit();
            }
        }
        static public SqliteDataReader execGet(String query, List<SqliteParameter> parametrs, String description)
        {
            using (var transaction = SqLite.getInstance().getConnection().BeginTransaction())
            {
                SqliteDataReader reader = null;
                SqliteCommand command = new SqliteCommand(query, SqLite.getInstance().getConnection(), transaction);

                foreach (SqliteParameter a in parametrs)
                {
                    command.Parameters.Add(a);
                }

                try
                {
                    reader = command.ExecuteReader();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    throw new QueryExeption(description);
                }
                transaction.Commit();
                return reader;
            }
        }
        static public void Delete(string DataBase, long id)
        {
            SqLite.exec(
                "DELETE FROM " + DataBase + " WHERE id = @id",
                new List<SqliteParameter> { new SqliteParameter("@id", id) },
                "DELETE FROM " + DataBase);
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
    }
}