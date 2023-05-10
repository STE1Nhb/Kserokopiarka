using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ver3
{
    public class Copier : BaseDevice, IPrinter, IScanner
    {
        private Printer printer = new Printer();
        private Scanner scanner = new Scanner();
        public int PrintCounter { get => printer.PrintCounter; }
        public int ScanCounter { get => scanner.ScanCounter; }
        public new int Counter { get; private set; } = 0;

        public new void PowerOn()
        {
            if (state == IDevice.State.off)
                Counter++;
            base.PowerOn();
        }
        public new void PowerOff()
        {
            base.PowerOff();
        }
        public void Print(in IDocument document)
        {
            if (state == IDevice.State.on)
            {
                printer.PowerOn();

                printer.Print(document);
                printer.PowerOff();
            }
        }

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            document = new ImageDocument("");
            
            if (state == IDevice.State.on)
            {
                scanner.PowerOn();

                scanner.Scan(out document, formatType);
                scanner.PowerOff();
            }
        }

        public void ScanAndPrint()
        {
            if (state == IDevice.State.on)
            {
                Scan(out IDocument document);
                Print(document);
            }
        }

        private class Printer : BaseDevice, IPrinter
        {
            public int PrintCounter { get; private set; } = 0;
            public new int Counter { get; private set; } = 0;
            public new void PowerOn()
            {
                if (state == IDevice.State.off)
                    Counter++;
                base.PowerOn();
            }
            public new void PowerOff()
            {
                base.PowerOff();
            }
            public void Print(in IDocument document)
            {
                if (state == IDevice.State.on)
                {
                    Console.WriteLine(DateTime.Now.ToString() + " Print: " + $"{document.GetFileName}");
                    PrintCounter++;
                }
            }
        }

        private class Scanner : BaseDevice, IScanner 
        {
            public int ScanCounter { get; private set; } = 0;
            public new int Counter { get; private set; } = 0;
            public new void PowerOn()
            {
                if (state == IDevice.State.off)
                    Counter++;
                base.PowerOn();
            }
            public new void PowerOff()
            {
                base.PowerOff();
            }
            public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
            {
                document = new ImageDocument("");

                if (state == IDevice.State.on)
                {
                    if (formatType == IDocument.FormatType.JPG)
                    {
                        document = new ImageDocument("ImageScan" + ScanCounter);
                        Console.WriteLine(DateTime.Now.ToString() + " Scan : " + $"ImageScan" + ScanCounter + ".jpg");
                        ScanCounter++;
                    }
                    else if (formatType == IDocument.FormatType.PDF)
                    {
                        document = new PDFDocument("PDFScan" + ScanCounter);
                        Console.WriteLine(DateTime.Now.ToString() + " Scan : " + $"PDFScan" + ScanCounter + ".pdf");
                        ScanCounter++;
                    }
                    else if (formatType == IDocument.FormatType.TXT)
                    {
                        document = new TextDocument("TextScan" + ScanCounter);
                        Console.WriteLine(DateTime.Now.ToString() + " Scan : " + $"TextScan" + ScanCounter + ".txt");
                        ScanCounter++;
                    }
                }
            }
        }
    }
}
