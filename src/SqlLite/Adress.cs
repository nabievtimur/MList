using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace MList.Storage.Container
{
    public partial class Address
    {
        public long id;
        public string address;
        public List<Tuple<String, object>> getByParametrList()
        {
            return new List<Tuple<String, object>> {
                    new Tuple<String, object>("@first_name", this.address) };
        }
        public List<Tuple<String, object>> getByParametrListWithId()
        {
            List<Tuple<String, object>> l = getByParametrList();
            l.Add(new Tuple<String, object>("@id", this.id));
            return l;
        }
        static public List<Address> Get()
        {
            List<Address> employees = new List<Address>();
            SqliteDataReader reader = SqLite.execGet("SELECT id, address FROM addresses", null, "Read addresses.");

            while (reader.Read()) // построчно считываем данные
            {
                employees.Add(new Address()
                {
                    id = reader.GetInt64(0),
                    address = reader.GetString(1)
                });
            }

            return employees;
        }
        public List<Address> Get(string search)
        {
            List<Address> adresses = new List<Address>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT id, address FROM addresses as ad WHERE ad.address LIKE '%@like%'",
                new List<Tuple<String, object>> {
                    new Tuple<String, object>("@like", search) },
                "Search address.");
            while (reader.Read()) // построчно считываем данные
            {
                Address address = new Address()
                {
                    id = reader.GetInt64(0),
                    address = reader.GetString(1),
                };
                adresses.Add(address);
            }
            return adresses;
        }
        static public void Add(Address adress)
        {
            SqLite.exec(
                "INSERT INTO addresses (address) VALUES (@address)",
                adress.getByParametrList(),
                "Add new adress.");
        }
        static public void Update(Address address)
        {
            SqLite.exec(
                "UPDATE addresses SET address = @address WHERE id = @id",
                address.getByParametrList(),
                "Update address.");
        }
        static public void Delete(Address address)
        {
            SqLite.Delete("addresses", address.id);
        }
        public List<Address> GetCurrentArrive(MList mlist)
        {
            List<Address> arriveAddresses = new List<Address>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT ad.id, ad.address FROM addresses AS ad " +
                    "JOIN mlist_arrive_address maa ON ad.id = maa.arrive_address_id WHERE maa.mlist_id = @mlist_id",
                new List<Tuple<String, object>> {
                    new Tuple<String, object>("@mlist_id", mlist.id) },
                "Read MList addresses arrive");
            while (reader.Read()) // построчно считываем данные
            {
                arriveAddresses.Add(new Address {
                    id = reader.GetInt64(0),
                    address = reader.GetString(1) } );
            }
            return arriveAddresses;
        }
        public List<Address> GetCurrentDeep(MList mlist)
        {
            List<Address> deepAddresses = new List<Address>();
            SqliteDataReader reader = SqLite.execGet(
                "SELECT ad.id, ad.address FROM addresses AS ad " +
                    "JOIN mlist_deep_address maa ON ad.id = maa.deep_address_id WHERE maa.mlist_id = @mlist_id",
                new List<Tuple<String, object>> {
                    new Tuple<String, object>("@mlist_id", mlist.id) },
                "Read MList addresses deep");
            while (reader.Read()) // построчно считываем данные
            {
                deepAddresses.Add(new Address {
                    id = reader.GetInt64(0),
                    address = reader.GetString(1) } );
            }
            return deepAddresses;
        }
    }
}
