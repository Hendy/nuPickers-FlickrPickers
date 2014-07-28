namespace nuPickers.FlickrPickers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// this is a singleton that handles returning the appropriate flickr api connection
    /// </summary>
    internal sealed class FlickrManager : MarshalByRefObject
    {
        private static readonly Lazy<FlickrManager> lazy = new Lazy<FlickrManager>(() => new FlickrManager());

        private static readonly object lockObject = new object();

        /// <summary>
        /// Collection of flickrConnections where each represents a unique key / secret combination
        /// </summary>
        private List<FlickrConnection> FlickrConnections { get; set; } 


        private FlickrManager()
        {
            this.FlickrConnections = new List<FlickrConnection>();
        }

        /// <summary>
        /// Attempts to add connection to the collection
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        private void AddFlickrConnection(string apiKey, string apiSecret)
        {
            // prevent race condition that could create multiple identical connections
            lock(lockObject)
            {
                // check again - another thread might have beaten us here...
                if (!this.FlickrConnections.Any(x => x.ApiKey == apiKey && x.ApiSecret == apiSecret))
                {
                    this.FlickrConnections.Add(new FlickrConnection(apiKey, apiSecret));
                }
            }
        }

        /// <summary>
        /// returns the same instance of the FlickrApi obj for the specified parameters
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <returns></returns>
        internal static FlickrConnection GetFlickrConnection(string apiKey, string apiSecret)
        {
            // get the singleton instance of FlickrManager
            FlickrManager flickrManager = lazy.Value;

            // try and get an existing connection
            FlickrConnection flickrConnection = flickrManager.FlickrConnections.SingleOrDefault(x => x.ApiKey == apiKey && x.ApiSecret == apiSecret);

            // if connection not found
            if (flickrConnection == null)
            {
                // ensure new connection is / has been added
                flickrManager.AddFlickrConnection(apiKey, apiSecret);

                // expect to find connection
                flickrConnection = flickrManager.FlickrConnections.Single(x => x.ApiKey == apiKey && x.ApiSecret == apiSecret);
            }

            return flickrConnection;
        }
    }
}