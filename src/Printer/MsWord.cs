using System;
using System.Collections.Generic;
using MList.Storage;

namespace MList.Printer
{
    public class MsWord
    {
        [Serializable]
        public class ConnectionExeption : Exception
        {
            public ConnectionExeption() { }
            public ConnectionExeption(string message)
                : base(message) { }
        }
        //static void print(
        //    Storage.Container.MList mlist, 
        //    List<Storage.Container.Address> deepAddresses,
        //    List<Storage.Container.Address> arriveAddresses,
        //    List<Storage.Container.Car> cars,
        //    List<Storage.Container.Gun> guns)
        //{
        //    throw new NotImplementedException();
        //}
    }
}