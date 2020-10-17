using System;
using System.IO;
using System.Net;
using System.Text;
using com.csiro.xml.datatype;

namespace HTTPResponse
{
    class WebFetch : Interfaces.ITestRun
    {
        // for debug
        const bool debug = false;
        
        private StringBuilder sb;
        private byte[] buf;
        private HttpWebRequest request;
        private HttpWebResponse response;
        public const string website = "http://www.yahoo.com";

        public void init(Config config)
        {
            if (debug) Console.WriteLine("WebFetch.preRun() called");
            sb = new StringBuilder();

            // used on each read operation
            buf = new byte[8192];

            request = (HttpWebRequest) WebRequest.Create(website);
        }

        public void preRun()
        {
            if (debug) Console.WriteLine("WebFetch.preRun() called");
        }

        public int run()
        {
            response = (HttpWebResponse) request.GetResponse();

            // Read data via the response stream
            Stream resStream = response.GetResponseStream();
            
            string tempString = null;
            int count = 0;

            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.ASCII.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?
            Console.WriteLine(sb.ToString());
            
            return 1;
        }

        public void run(ref string[] pnames, ref long[] pvalues)
        {
            if (debug) Console.WriteLine("WebFetch.run(args) called");
        }

        public void postRun()
        {
            if (debug) Console.WriteLine("WebFetch.postRun() called");
        }

        public void done()
        {
            if (debug) Console.WriteLine("WebFetch.done() called");
        }

        
    }
}
