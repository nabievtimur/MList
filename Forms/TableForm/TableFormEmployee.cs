using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MList.src;
using MList.Storage;

namespace MList.Forms.TableForm
{
    public partial class TableFormEmployee : MList.Forms.TableFormTemplate
    {
        public TableFormEmployee()
        {
            InitializeComponent();
            this.attrs = new List<Attr>();

            this.attrs.Add(new Attr("Фамилия"));
            this.attrs.Add(new Attr("Имя"));
            this.attrs.Add(new Attr("Отчество"));
        }
        protected new void add(List<string> result)
        {
            if (result.Count != 3) return;
            List<string> sds = new List<string>(result);
            SqLiteStorage.Employee emp = new SqLiteStorage.Employee();
            emp.firstName = sds[0];
            emp.middleName = sds[1];
            emp.lastName = sds[2];

            int i = 0x00;
            foreach (string s in result)
            {
                if (i == 0)
                {
                    emp.firstName = s;
                    i++;
                }
                else if (i == 1)
                {
                    emp.middleName = s;
                    i++;
                }
                else if (i == 2)
                {
                    emp.lastName = s;
                    i++;
                }
            }

            if (SqLiteStorage.Status.OK != SqLiteStorage.getInstance().Add(new SqLiteStorage.Employee { 
                firstName = sds[0], 
                middleName = sds[1], 
                lastName = sds[2] }))
            {
                MessageBox.Show(
                    "Ошибка",
                    "Добавление в базу данных завершилось ошибкой",
                    MessageBoxButtons.OK);
            }
        }
        protected new void change(List<string> result) { }
        protected new void delete() { }
        protected new void updateGrid() 
        {
            List<SqLiteStorage.Employee> list = new List<SqLiteStorage.Employee>();
            if (SqLiteStorage.Status.OK != SqLiteStorage.getInstance().Get(out list))
            {
                MessageBox.Show(
                    "Ошибка",
                    "Чтение из базы данных",
                    MessageBoxButtons.OK);
            }

            int i = 0x00;
            foreach (SqLiteStorage.Employee emp in list)
            {
                this.getGrid().Rows[i].Cells[0].Value = emp.firstName;
                this.getGrid().Rows[i].Cells[1].Value = emp.middleName;
                this.getGrid().Rows[i].Cells[2].Value = emp.lastName;
            }
        }
    }
}
