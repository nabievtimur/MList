using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace MList.Storage
{
    public class SqLiteStorage
    {
        private SqliteConnection _connection;
        public enum Status
        {
            OK,
            ERROR,
            ERROR_CONNECTION
        }

        public struct Address
        {
            int id;
            string address;
        }

        public struct Cars
        {
            int id;
            string brand;
            string number;
        }

        public struct Gun
        {
            int id;
            string brand;
            string series;
            int number;
            string ammo;
        }

        public struct Employee
        {
            int id;
            string firstName;
            string secondName;
            string middleName;
        }

        public struct Orders
        {
            int id;
            int number;
            int employeeId;
            int date;
        }

        public struct MList
        {
            int id;
            int dateCreate;
            int dateBegin;
            int dateEnd;
            int dateCoach;
            int datePassGun;
            int datePring;
            string notes;
            int timeDeep;
            int timeArrive;
            int timePassGun;
            int numberMlist;
        }

        SqLiteStorage()
        {
            
        }

        public Status initConnection()
        {
            // получить путь /*Users*/Public/MList/BD
            // проверить есть ли бд
            // создать или открыть
            // 
            return Status.OK;
        }

        public Status export()
        {
            
            return Status.OK;
        }

        public Status import()
        {
            
            return Status.OK;
        }

        public Status get(out List<Address> adresses)
        {
            adresses = new List<Address>();
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