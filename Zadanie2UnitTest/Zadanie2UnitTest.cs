using ver1;
using ver2;
namespace ver2UnitTest
{
    [TestClass]
    public class ConsoleRedirectionToStringWriter : IDisposable
    {
        private StringWriter stringWriter;
        private TextWriter originalOutput;

        public ConsoleRedirectionToStringWriter()
        {
            stringWriter = new StringWriter();
            originalOutput = Console.Out;
            Console.SetOut(stringWriter);
        }

        public string GetOutput()
        {
            return stringWriter.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(originalOutput);
            stringWriter.Dispose();
        }
    }


    [TestClass]
    public class UnitTestMultiDevice
    {
        [TestMethod]
        public void MultiDevice_GetState_StateOff()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOff();

            Assert.AreEqual(IDevice.State.off, multiDev.GetState());
        }

        [TestMethod]
        public void MultiDevice_GetState_StateOn()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOn();

            Assert.AreEqual(IDevice.State.on, multiDev.GetState());
        }


        // weryfikacja, czy po wywo³aniu metody `Print` i w³¹czonej kopiarce w napisie pojawia siê s³owo `Print`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        [TestMethod]
        public void MultiDevice_Print_DeviceOn()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDev.Print(in doc1);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        // weryfikacja, czy po wywo³aniu metody `Print` i wy³¹czonej kopiarce w napisie NIE pojawia siê s³owo `Print`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        [TestMethod]
        public void MultiDevice_Print_DeviceOff()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDev.Print(in doc1);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        // weryfikacja, czy po wywo³aniu metody `Scan` i wy³¹czonej kopiarce w napisie NIE pojawia siê s³owo `Scan`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        [TestMethod]
        public void MultiDevice_Scan_DeviceOff()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                multiDev.Scan(out doc1);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Scan"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        // weryfikacja, czy po wywo³aniu metody `Scan` i wy³¹czonej kopiarce w napisie pojawia siê s³owo `Scan`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        [TestMethod]
        public void MultiDevice_Scan_DeviceOn()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                multiDev.Scan(out doc1);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        // weryfikacja, czy wywo³anie metody `Scan` z parametrem okreœlaj¹cym format dokumentu
        // zawiera odpowiednie rozszerzenie (`.jpg`, `.txt`, `.pdf`)
        [TestMethod]
        public void MultiDev_Scan_FormatTypeDocument()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                multiDev.Scan(out doc1, formatType: IDocument.FormatType.JPG);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".jpg"));

                multiDev.Scan(out doc1, formatType: IDocument.FormatType.TXT);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".txt"));

                multiDev.Scan(out doc1, formatType: IDocument.FormatType.PDF);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".pdf"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }


        // weryfikacja, czy po wywo³aniu metody `ScanAndPrint` i wy³¹czonej kopiarce w napisie pojawiaj¹ siê s³owa `Print`
        // oraz `Scan`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        [TestMethod]
        public void MultiDevice_ScanAndPrint_DeviceOn()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                multiDev.ScanAndPrint();
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        // weryfikacja, czy po wywo³aniu metody `ScanAndPrint` i wy³¹czonej kopiarce w napisie NIE pojawia siê s³owo `Print`
        // ani s³owo `Scan`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        [TestMethod]
        public void MultiDevice_ScanAndPrint_DeviceOff()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                multiDev.ScanAndPrint();
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultiDevice_PrintCounter()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOn();

            IDocument doc1 = new PDFDocument("aaa.pdf");
            multiDev.Print(in doc1);
            IDocument doc2 = new TextDocument("aaa.txt");
            multiDev.Print(in doc2);
            IDocument doc3 = new ImageDocument("aaa.jpg");
            multiDev.Print(in doc3);

            multiDev.PowerOff();
            multiDev.Print(in doc3);
            multiDev.Scan(out doc1);
            multiDev.PowerOn();

            multiDev.ScanAndPrint();
            multiDev.ScanAndPrint();

            // 5 wydruków, gdy urz¹dzenie w³¹czone
            Assert.AreEqual(5, multiDev.PrintCounter);
        }

        [TestMethod]
        public void MultiDevice_ScanCounter()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOn();

            IDocument doc1;
            multiDev.Scan(out doc1);
            IDocument doc2;
            multiDev.Scan(out doc2);

            IDocument doc3 = new ImageDocument("aaa.jpg");
            multiDev.Print(in doc3);

            multiDev.PowerOff();
            multiDev.Print(in doc3);
            multiDev.Scan(out doc1);
            multiDev.PowerOn();

            multiDev.ScanAndPrint();
            multiDev.ScanAndPrint();

            // 4 skany, gdy urz¹dzenie w³¹czone
            Assert.AreEqual(4, multiDev.ScanCounter);
        }

        [TestMethod]
        public void MultiDevice_PowerOnCounter()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOn();
            multiDev.PowerOn();
            multiDev.PowerOn();

            IDocument doc1;
            multiDev.Scan(out doc1);
            IDocument doc2;
            multiDev.Scan(out doc2);

            multiDev.PowerOff();
            multiDev.PowerOff();
            multiDev.PowerOff();
            multiDev.PowerOn();

            IDocument doc3 = new ImageDocument("aaa.jpg");
            multiDev.Print(in doc3);

            multiDev.PowerOff();
            multiDev.Print(in doc3);
            multiDev.Scan(out doc1);
            multiDev.PowerOn();

            multiDev.ScanAndPrint();
            multiDev.ScanAndPrint();

            // 3 w³¹czenia
            Assert.AreEqual(3, multiDev.Counter);
        }

        [TestMethod]
        public void MultiDevice_Accept_DeviceOn() 
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDev.Accept(in doc1);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Accept"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultiDevice_Accept_DeviceOff()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDev.Accept(in doc1);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Accept"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }


        [TestMethod]
        public void MultiDevice_Send_DeviceOn()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDev.Send(out IDocument doc2, IDocument.FormatType.PDF);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Send"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultiDevice_Send_DeviceOff()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDev.Send(out IDocument doc2, IDocument.FormatType.PDF);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Send"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }


        [TestMethod]
        public void MultiDev_Send_FormatTypeDocument()
        {
            var multiDev = new MultifunctionalDevice();
            multiDev.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                multiDev.Send(out doc1);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Send"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".jpg"));

                multiDev.Send(out doc1, formatType: IDocument.FormatType.TXT);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Send"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".txt"));

                multiDev.Send(out doc1, formatType: IDocument.FormatType.PDF);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".pdf"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }
    }
}