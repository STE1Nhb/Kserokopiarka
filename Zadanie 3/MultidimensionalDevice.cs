using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ver3
{
    public class MultidimensionalDevice : Copier, IFax
    {
        private Fax fax = new();
        public int SendCounter { get => fax.SendCounter; }
        public int AcceptCounter { get => fax.AcceptCounter; }

        public void Send(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            document = new ImageDocument("");

            if (state == IDevice.State.on)
            {
                fax.PowerOn();
                fax.Send(out document);
                fax.PowerOff();
            }
        }

        public void Accept(in IDocument document)
        {
            if (state == IDevice.State.on)
            {
                fax.PowerOn();
                fax.Accept(in document);
                fax.PowerOff();
            }
        }

        private class Fax : BaseDevice, IFax
        {
            public int SendCounter { get; private set; } = 0;
            public int AcceptCounter { get; private set; } = 0;
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

            public void Accept(in IDocument docIn)
            {
                if (state == IDevice.State.on)
                {
                    Print(docIn);
                    Console.WriteLine(DateTime.Now.ToString() + "Accept:\t" + docIn.GetFileName().ToString() + docIn.GetFormatType().ToString());
                    AcceptCounter++;
                }
            }
            public void Send(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
            {
                document = new ImageDocument("");

                if (state == IDevice.State.on)
                {
                    switch (formatType)
                    {
                        case IDocument.FormatType.JPG:
                            document = new ImageDocument("ImageSend" + SendCounter);
                            break;
                        case IDocument.FormatType.PDF:
                            document = new PDFDocument("PDFSend" + SendCounter);
                            break;
                        case IDocument.FormatType.TXT:
                            document = new TextDocument("TextSend" + SendCounter);
                            break;
                    }

                    Scan(out document, formatType);
                    Console.WriteLine(DateTime.Now.ToString() + "Send:\t" + document.GetFileName().ToString() + document.GetFormatType().ToString());
                    SendCounter++;
                }
            }

            public void Print(in IDocument document)
            {
                if (state == IDevice.State.on)
                {
                    Console.WriteLine(DateTime.Now.ToString() + " Print: " + $"{document.GetFileName}");
                    AcceptCounter++;
                }
            }

            public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
            {
                document = new ImageDocument("");

                if (state == IDevice.State.on)
                {
                    if (formatType == IDocument.FormatType.JPG)
                    {
                        document = new ImageDocument("ImageScan" + SendCounter);
                        Console.WriteLine(DateTime.Now.ToString() + " Scan : " + $"ImageScan" + SendCounter + ".jpg");
                        SendCounter++;
                    }
                    else if (formatType == IDocument.FormatType.PDF)
                    {
                        document = new PDFDocument("PDFScan" + SendCounter);
                        Console.WriteLine(DateTime.Now.ToString() + " Scan : " + $"PDFScan" + SendCounter + ".pdf");
                        SendCounter++;
                    }
                    else if (formatType == IDocument.FormatType.TXT)
                    {
                        document = new TextDocument("TextScan" + SendCounter);
                        Console.WriteLine(DateTime.Now.ToString() + " Scan : " + $"TextScan" + SendCounter + ".txt");
                        SendCounter++;
                    }
                }
            }
        }
    }
}
