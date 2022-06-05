using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MList.Forms
{
    public partial class TableFormEmployee : Form
    {
        List<Storage.SqLiteStorage.Employee> list;
        public TableFormEmployee(string title)
        {
            InitializeComponent();
        }

        public Storage.SqLiteStorage.Status init()
        {
            return Storage.SqLiteStorage.getInstance().Get(out this.list);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
