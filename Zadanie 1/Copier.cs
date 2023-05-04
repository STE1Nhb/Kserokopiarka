using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ver1
{
    public class Copier : BaseDevice, IPrinter, IScanner
    {
        public int PrintCounter { get; private set; } = 0;
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

        public void Print(in IDocument document)
        {
            if (state == IDevice.State.on)
            {
                Console.WriteLine(DateTime.Now.ToString() + " Print: " + $"{document.GetFileName}");
                PrintCounter++;
            }
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
                    document = new ImageDocument("PDFScan" + ScanCounter);
                    Console.WriteLine(DateTime.Now.ToString() + " Scan : " + $"PDFScan" + ScanCounter + ".pdf");
                    ScanCounter++;
                }
                else if (formatType == IDocument.FormatType.TXT)
                {
                    document = new ImageDocument("TextScan" + ScanCounter);
                    Console.WriteLine(DateTime.Now.ToString() + " Scan : " + $"TextScan" + ScanCounter + ".txt");
                    ScanCounter++;
                }
            }
        }
        public void ScanAndPrint()
        {
            if(state == IDevice.State.on)
            {
                Scan(out IDocument document);
                Print(document);
            }
        }
    }
}
