﻿using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MList.Storage.Container
{
    internal class ContainerEmployee : iConteiner
    {
        public string firstName;
        public string lastName;
        public string middleName;

        public ContainerEmployee() : base()
        {
            this.firstName = "";
            this.lastName = "";
            this.middleName = "";
        }
        public ContainerEmployee(long id, string lastName, string firstName, string middleName) : base(id)
        {
            this.lastName = lastName;
            this.firstName = firstName;
            this.middleName = middleName ;
        }
        public ContainerEmployee(DataGridViewRow row) : base(row)
        {
            try
            {
                this.lastName = getStringFromCell(row.Cells[1]);
                this.firstName = getStringFromCell(row.Cells[2]);
                this.middleName = getStringFromCell(row.Cells[3]);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ParceException("DataGridViewRow");
            }
        }
        public ContainerEmployee(SqliteDataReader reader) : base(reader)
        {
            try
            {
                this.firstName = reader.GetString(1);
                this.lastName = reader.GetString(2);
                this.middleName = reader.GetString(3);
            }
            catch (Exception)
            {
                throw new ParceException("SqliteDataReader");
            }
        }
        public override void storageFillParameterCollection(ref SqliteParameterCollection parameterCollection)
        {
            parameterCollection.Add(new SqliteParameter("@first_name", this.firstName));
            parameterCollection.Add(new SqliteParameter("@last_name", this.lastName));
            parameterCollection.Add(new SqliteParameter("@middle_name", this.middleName));
        }
        override public void gridRowFill(ref DataGridViewRow row)
        {
            base.gridRowFill(ref row);
            row.Cells[0].Value = this.lastName;
            row.Cells[1].Value = this.firstName;
            row.Cells[2].Value = this.middleName;
        }
        public override void fillItemList(ref List<Tuple<Label, TextBox>> lItems)
        {
            throw new NotImplementedException();
        }
    }
}
