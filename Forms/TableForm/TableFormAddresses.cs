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
    public partial class TableFormAddresses : MList.Forms.TableFormTemplate
    {
        public TableFormAddresses()
        {
            InitializeComponent();
            this.attrs = new List<Attr>();

            this.attrs.Add(new Attr("Адрес"));
        }
    }
}
