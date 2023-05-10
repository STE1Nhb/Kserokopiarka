using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ver1;

namespace ver2
{
    public interface IFax : IDevice, IPrinter, IScanner
    {
        void Send(out IDocument document, IDocument.FormatType formatType);
        void Accept(in IDocument docIn);
    }
    public class MultifunctionalDevice : Copier, IFax
    {
        public int SendCounter { get; private set; } = 0;
        public int AcceptCounter { get; private set; } = 0;
        public void Accept(in IDocument docIn)
        {
            if(state == IDevice.State.on)
            {
                base.Print(docIn);
                Console.WriteLine(DateTime.Now.ToString() + "Accept:\t" + docIn.GetFileName().ToString() + docIn.GetFormatType().ToString());
                AcceptCounter++;
            }
        }
        public void Send(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            document = new ImageDocument("");

            if (state == IDevice.State.on)
            {
                switch(formatType)
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

                base.Scan(out document, formatType);
                Console.WriteLine(DateTime.Now.ToString() + "Send:\t" + document.GetFileName().ToString() + document.GetFormatType().ToString());
                SendCounter++;
            }
        }
    }
}
