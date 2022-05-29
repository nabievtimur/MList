using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace MList.Storage
{
    public class SqLiteStorage
    {
        private SqliteConnection _connection;
        private static SqLiteStorage instance;
        public enum Status
        {
            OK,
            ERROR,
            ERROR_CONNECTION
        }

        public struct Address
        {
            internal int id;
            public string address;

            public Address(int id, string address)
            {
                this.id = id;
                this.address = address;
            }
        }

        public struct Cars
        {
            private int id;
            public string brand;
            public string number;
            
            // public Cars()
        }

        public struct Gun
        {
            private int id;
            public string brand;
            public string series;
            public int number;
            public string ammo;
        }

        public struct Employee
        {
            private int id;
            public string firstName;
            public string secondName;
            public string middleName;
        }

        public struct Orders
        {
            private int id;
            public int number;
            public int employeeId;
            public int date;
        }

        public struct MList
        {
            private int id;
            public int dateCreate;
            public int dateBegin;
            public int dateEnd;
            public int dateCoach;
            public int datePassGun;
            public int datePring;
            public string notes;
            public int timeDeep;
            public int timeArrive;
            public int timePassGun;
            public int numberMlist;
        }

        private SqLiteStorage() { }

        public static SqLiteStorage getInstance()
        {
            if (instance == null)
                instance = new SqLiteStorage();
            return instance;
        }

        public Status initConnection()
        {
            // получить путь /*Users*/Public/MList/BD
            // проверить есть ли бд
            // создать или открыть
            // 
            return Status.OK;
        }

        public Status export(string path)
        {
            
            return Status.OK;
        }

        public Status import(string path)
        {
            
            return Status.OK;
        }

        public Status get(out List<Address> adresses)
        {
            adresses = new List<Address>();

            Address address1 = new Address(0, "");
            address1.id = 1;
            
            string sqlExpression = "SELECT (id, address) FROM addresses";
            SqliteCommand command = new SqliteCommand(sqlExpression, _connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read())   // построчно считываем данные
                    {
                        Address address = new Address(reader.GetInt32(0), reader.GetString(1));
                        address.id = 5;
                        adresses.Add(address);
                    }
                }
            }
            return Status.OK;
        }

        public Status get(out List<Cars> cars)
        {
            cars = new List<Cars>();
            return Status.OK;
        }

        public Status get(out List<Gun> guns)
        {
            guns = new List<Gun>();
            return Status.OK;
        }

        public Status get(out List<Employee> employees)
        {
            employees = new List<Employee>();
            return Status.OK;
        }

        public Status get(out List<Orders> orders)
        {
            orders = new List<Orders>();
            return Status.OK;
        }

        // MLIST add

        public Status add(Address adress)
        {
            return Status.OK;
        }

        public Status add(Cars car)
        {
            return Status.OK;
        }

        public Status add(Gun gun)
        {
            return Status.OK;
        }

        public Status add(Employee employee)
        {
            return Status.OK;
        }

        public Status add(Orders order)
        {
            return Status.OK;
        }

        // add Mlist

        public Status delete(Address adresse)
        {
            return Status.OK;
        }

        public Status delete(Cars car)
        {
            return Status.OK;
        }

        public Status delete(Gun gun)
        {
            return Status.OK;
        }

        public Status delete(Employee employee)
        {
            return Status.OK;
        }

        public Status delete(Orders order)
        {
            return Status.OK;
        }

        //delete MList
    }
}