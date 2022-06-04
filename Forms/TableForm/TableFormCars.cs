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
    public partial class TableFormCars : MList.Forms.TableFormTemplate
    {
        public TableFormCars()
        {
            InitializeComponent();
            this.attrs = new List<Attr>();

            this.attrs.Add(new Attr("Брэнд"));
            this.attrs.Add(new Attr("Номер"));
        }
    }
}
