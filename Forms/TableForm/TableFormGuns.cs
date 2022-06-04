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
    public partial class TableFormGuns : MList.Forms.TableFormTemplate
    {
        public TableFormGuns()
        {
            InitializeComponent();
            this.attrs = new List<Attr>();

            this.attrs.Add(new Attr("Брэнд"));
            this.attrs.Add(new Attr("Серия"));
            this.attrs.Add(new Attr("Номер"));
            this.attrs.Add(new Attr("Патроны"));
        }
    }
}
