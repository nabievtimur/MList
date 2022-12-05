namespace MList.Printer
{
    public class MsWord
    {
        // static
        private static MsWord instance;
        public static MsWord getInstance()
        {
            if (instance == null)
                instance = new MsWord();
            return instance;
        }
        
        
    }
}