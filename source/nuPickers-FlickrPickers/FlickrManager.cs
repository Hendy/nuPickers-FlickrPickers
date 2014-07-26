namespace nuPickers.FlickrPickers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// this is a singleton that handles returning the appropriate flickr api connection
    /// </summary>
    internal sealed class FlickrManager
    {
        private static readonly Lazy<FlickrManager> lazy = new Lazy<FlickrManager>(() => new FlickrManager());

        private FlickrManager()
        {
        }

        private List<FlickrConnection> flickrConnections = new List<FlickrConnection>();

        /// <summary>
        /// returns the same instance of the FlickrApi obj for the specified parameters
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <returns></returns>
        internal static FlickrConnection GetFlickrConnection(string apiKey, string apiSecret)
        {
            // internally use a singleton instance of FlickrManager
            FlickrManager flickrManager = lazy.Value;

            // is there an existing flickr connection ?
            FlickrConnection flickrConnection = flickrManager.flickrConnections.SingleOrDefault(x => x.ApiKey == apiKey && x.ApiSecret == apiSecret);

            // if connection not found
            if (flickrConnection == null)
            {
                // TODO: thread safety lock ?

                // create new connection
                flickrConnection = new FlickrConnection(apiKey, apiSecret);

                // add to collection
                flickrManager.flickrConnections.Add(flickrConnection);
            }

            return flickrConnection;
        }
    }
}