using System;
using System.Collections.Generic;
using System.Windows.Forms;

using MList.Storage.Container;

namespace MList.Forms.TableForm
{
    public partial class TableFormMList : Form
    {
        Dictionary<int, Gun> itemsGun;
        Dictionary<int, Car> itemsCar;
        Dictionary<int, Address> itemsAddressDeep;
        Dictionary<int, Address> itemsAddressArrive;
        public TableFormMList()
        {
            InitializeComponent();


        }
        public TableFormMList(Storage.Container.MList mlist)
        {
            InitializeComponent();


        }
        void updateGrids()
        {

        }
        private void TableFormMList_Load(object sender, EventArgs e)
        {

        }
    }
}
