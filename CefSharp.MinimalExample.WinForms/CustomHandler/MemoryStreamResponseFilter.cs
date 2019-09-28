using System;
using System.IO;
using System.Reflection;

namespace CefSharp.MinimalExample.WinForms.CustomHandler
{
    class MemoryStreamResponseFilter : IResponseFilter
    {
        private MemoryStream memoryStream;
        public bool isDisposed;
        public ulong requestID; // reference to request uid

        public byte[] Data
        {
            get { return memoryStream.ToArray(); }
        }

        public MemoryStreamResponseFilter(ulong identifier)
        {
            this.requestID = identifier;
            Console.WriteLine($"RequestID {requestID} {this.GetType().Name}->{MethodBase.GetCurrentMethod().Name}:  IFilter.isDisposed: {isDisposed}");
        }

        public bool InitFilter()
        {
            memoryStream = new MemoryStream();
            return true;
        }

        public FilterStatus Filter(Stream dataIn, out long dataInRead, Stream dataOut, out long dataOutWritten)
        {
            // handle dataIn stream == null. Can happen on POST requests
            if (dataIn == null) { dataInRead = 0;dataOut = null; dataOutWritten = 0; return FilterStatus.Done; }
           
            //Calculate how much data we can read, in some instances dataIn.Length is greater than dataOut.Length
            dataInRead = Math.Min(dataIn.Length, dataOut.Length);
            dataOutWritten = dataInRead;

            var readBytes = new byte[dataInRead];
            dataIn.Read(readBytes, 0, readBytes.Length);
            dataOut.Write(readBytes, 0, readBytes.Length);

            //Write buffer to the memory stream
            memoryStream.Write(readBytes, 0, readBytes.Length);

            //If we read less than the total amount avaliable then we need return FilterStatus.NeedMoreData so we can then write the rest
            if (dataInRead < dataIn.Length) return FilterStatus.NeedMoreData;


            Console.WriteLine($"RequestID {requestID} {this.GetType().Name}->{MethodBase.GetCurrentMethod().Name}:  IFilter.Data.Length: {this.Data.Length} isDisposed: {isDisposed}");
            return FilterStatus.Done;
        }

        public void Dispose()
        {
            isDisposed = true;
            Console.WriteLine($"RequestID {requestID} {this.GetType().Name}->{MethodBase.GetCurrentMethod().Name}:  IFilter.isDisposed: {isDisposed}");
            memoryStream.Dispose();
            memoryStream = null;
        }
    }
}
