using System;
using Google.GData.GoogleBase;
using com.csiro.xml.datatype;

namespace GoogleTester
{
    // Class implementing the testing framework
    class GoogleTestRun : GoogleTester, Interfaces.ITestRun
    {
        // for debug
        const bool debug = false;

        // max number of results
        private const int MAX_RESULTS = 5;
        private string[] queryArr;

        public GoogleTestRun()
        {
            if (debug) Console.WriteLine("GoogleTestRun() called");
        }

        public void init(Config config)
        {
            if (debug) Console.WriteLine("GoogleTestRun.init() called");

            // Keywords to search
            string[] q = new string[] { "boxing", "weather", "cars" };
            queryArr = Init(q, "837738399", "Google-CsharpQuery-0.1");
        }

        public void preRun()
        {
            if (debug) Console.WriteLine("GoogleTestRun.preRun() called");
        }

        public int run()
        {
            if (debug) Console.WriteLine("GoogleTestRun.run() called");

            // Creates a query on the snippets feed.
            GBaseQuery query = new GBaseQuery(uriFactory.SnippetsFeedUri);

            for (int i = 0; i < queryArr.GetLength(0); i++)
            {
                query.GoogleBaseQuery = queryArr[i];
                query.NumberToRetrieve = MAX_RESULTS;

                System.Console.WriteLine("Sending request to: " + query.Uri);

                // Connects to the server and gets the result, which is
                // then parsed to create a GBaseFeed object.
                GBaseFeed result = service.Query(query);

                PrintResult(result);
            }
            return 1;
        }

        public void run(ref string[] pnames, ref long[] pvalues)
        {
            if (debug) Console.WriteLine("GoogleTestRun.run(args) called");
        }

        public void postRun()
        {
            if (debug) Console.WriteLine("GoogleTestRun.postRun() called");
        }

        public void done()
        {
            if (debug) Console.WriteLine("GoogleTestRun.done() called");
        }

        private void PrintResult(GBaseFeed result)
        {
            if (result.TotalResults == 0)
            {
                System.Console.WriteLine("No matches.");
                return;
            }

            foreach (GBaseEntry entry in result.Entries)
            {
                System.Console.WriteLine(entry.GBaseAttributes.ItemType +
                                         ": " + entry.Title.Text +
                                         " - " + entry.Id.Uri);
            }
        }
    }
}