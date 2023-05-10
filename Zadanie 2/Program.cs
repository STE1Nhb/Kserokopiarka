namespace ver2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOn();
            Console.WriteLine("\nResult:\n");
            multiDev.ScanAndPrint();
            multiDev.PowerOff();
            Console.WriteLine("\n");
            multiDev.ScanAndPrint();
            multiDev.ScanAndPrint();
        }
    }
}