namespace nuPickers.FlickrPickers.DataSources
{
    using nuPickers.Shared.DotNetDataSource;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// nuPickers DotNetDataSource for a Flickr Photoset
    /// </summary>
    public class Photoset : IDotNetDataSource, IDotNetDataSourcePaged
    {
        /// <summary>
        /// Umbraco back-office configuration option - the Flickr Api Key
        /// </summary>
        [DotNetDataSource(Title = "Flickr API Key", Description = "(required)")]
        public string ApiKey { get; set; }

        /// <summary>
        /// Umbraco back-office configuration option - the Flickr Api Secret
        /// </summary>
        [DotNetDataSource(Title = "Flickr API Secret", Description = "(required)")]
        public string ApiSecret { get; set; }

        /// <summary>
        /// Umbraco back-office configuration option - a Flickr photoset id
        /// </summary>
        [DotNetDataSource(Title = "Photoset Id", Description = "(can be found in Flickr url)")]
        public string PhotosetId { get; set; }

        /// <summary>
        /// IDotNetDataSourcePaged
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// IDotNetDataSourcePaged
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// IDotNetDataSourcePaged
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// IDotNetDataSource
        /// </summary>
        /// <param name="contextId"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, string>> GetEditorDataItems(int contextId)
        {
            var flickrConnection = FlickrManager.GetFlickrConnection(this.ApiKey, this.ApiSecret);
            IEnumerable<FlickrImage> flickrImages;

            if (this.ItemsPerPage > 0 && this.Page > 0)
            {
                int total;

                flickrImages = flickrConnection.GetFlickrImagesInPhotoset(this.PhotosetId, this.ItemsPerPage, this.Page, out total);

                this.Total = total;
            }
            else
            {
                flickrImages = flickrConnection.GetFlickrImagesInPhotoset(this.PhotosetId);
            }

            return flickrImages.Select(x => new KeyValuePair<string, string>(x.PhotoId, "<img src='" + x.SquareThumbnailUrl + "' />"));
        }
    }
}