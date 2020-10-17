using Google.GData.GoogleBase;
using System;

namespace GoogleTester
{
    // Class implementing the Google API
    public abstract class GoogleTester
    {
        protected GBaseUriFactory uriFactory = GBaseUriFactory.Default;
        protected GBaseService service;

        private string developerKey;

        // Keywords initialisation 
        public string[] Init(string[] keywords, string dKey, string applicationName)
        {
            int keywordsIndex = 0;
            developerKey = dKey;

            // service.query does a GET on the url above and parses the result,
            // which is an ATOM feed with some extensions (called the Google Base
            // data API items feed).
            service = new GBaseService(applicationName, developerKey);

            // Return the rest of the arguments
            if (keywordsIndex > 0)
            {
                string[] newkeywords = new string[keywords.Length - keywordsIndex];
                System.Array.Copy(keywords, keywordsIndex, newkeywords, 0, newkeywords.Length);
                return newkeywords;
            }
            return keywords;
        }

        //Displays an error to stderr and quits    
        protected static void FatalError(string message)
        {
            System.Console.Error.WriteLine(message);
            System.Environment.Exit(1);
        }
    }
}
