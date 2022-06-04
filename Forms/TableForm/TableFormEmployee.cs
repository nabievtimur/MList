using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MList.src;

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
    }
}
