using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MList.src
{
    public class Attr
    {
        protected string sName;
        protected string sValue;
        public Attr(string sName)
        {
            this.sName = sName;
            this.sValue = "";
        }

        public string getName() { return this.sName; }
        public string getValue() { return this.sValue; }
        public void setValue(string sValue) { this.sValue = sValue; }

        public bool check(string sValue) { return !(sValue.Length == 0); }
    }

    public class AttrName : Attr
    {
        AttrName(string sName) : base(sName) { }
        public new bool check(string sValue) { return !(sValue.Length == 0); }
    }
}
