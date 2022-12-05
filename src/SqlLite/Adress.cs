using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace MList.Storage.Container
{
    public partial class Address
    {
        public long id;
        public string address;
        public List<SqliteParameter> getByParametrList()
        {
            return new List<SqliteParameter> {
                    new SqliteParameter("@address", this.address) };
        }
        public List<SqliteParameter> getByParametrListWithId()
        {
            List<SqliteParameter> l = getByParametrList();
            l.Add(new SqliteParameter("@id", this.id));
            return l;
        }
        static private List<Address> Read(SqliteDataReader reader)
        {
            List<Address> list = new List<Address>();

            try
            {
                while (reader.Read()) // построчно считываем данные
                {
                    list.Add(new Address {
                        id = reader.GetInt64(0),
                        address = reader.GetString(1) } );
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
        static public List<Address> Get()
        {
            return Address.Read(SqLite.execGet(
                "SELECT id, address FROM addresses",
                new List<SqliteParameter>(),
                "Read addresses."));
        }
        static public List<Address> Get(string search)
        {
            return Address.Read(SqLite.execGet(
                "SELECT id, address FROM addresses as ad WHERE ad.address LIKE @like",
                new List<SqliteParameter> {
                    new SqliteParameter("@like", "%" + search + "%") },
                "Search address."));
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
                address.getByParametrListWithId(),
                "Update address.");
        }
        static public void Delete(Address address)
        {
            SqLite.Delete("addresses", address.id);
        }
        public List<Address> GetCurrentArrive(MList mlist)
        {
            return Address.Read(SqLite.execGet(
                "SELECT ad.id, ad.address FROM addresses AS ad " +
                    "JOIN mlist_arrive_address maa ON ad.id = maa.arrive_address_id WHERE maa.mlist_id = @mlist_id",
                new List<SqliteParameter> {
                    new SqliteParameter("@mlist_id", mlist.id) },
                "Read MList addresses arrive"));
        }
        public List<Address> GetCurrentDeep(MList mlist)
        {
            return Address.Read(SqLite.execGet(
                "SELECT ad.id, ad.address FROM addresses AS ad " +
                    "JOIN mlist_deep_address maa ON ad.id = maa.deep_address_id WHERE maa.mlist_id = @mlist_id",
                new List<SqliteParameter> {
                    new SqliteParameter("@mlist_id", mlist.id) },
                "Read MList addresses deep"));
        }
    }
}
