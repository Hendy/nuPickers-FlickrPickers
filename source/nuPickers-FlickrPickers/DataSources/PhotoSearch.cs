namespace nuPickers.FlickrPickers.DataSources
{
    using FlickrNet;
    using nuPickers.Shared.DotNetDataSource;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// nuPickers DotNetDataSource for searching Flickr images
    /// </summary>
    public class PhotoSearch : IDotNetDataSource, IDotNetDataSourceTypeahead, IDotNetDataSourceKeyed
    {
        /// <summary>
        /// Umbraco back-office configuration option - 
        /// </summary>
        [DotNetDataSource(Title = "Flickr API Key", Description = "(required)")]
        public string ApiKey { get; set; }

        /// <summary>
        /// Umbraco back-office configuration option - 
        /// </summary>
        [DotNetDataSource(Title = "Flickr API Secret", Description = "(required)")]
        public string ApiSecret { get; set; }

        /// <summary>
        /// Umbraco back-office configuration option - 
        /// </summary>
        [DotNetDataSource(Title = "Flickr Username", Description = "(screen name)")]
        public string Username { get; set; }

        /// <summary>
        /// Umbraco back-office configuration option - 
        /// </summary>
        [DotNetDataSource(Description="comma delimited list of tags")]
        public string Tags { get; set; }

        /// <summary>
        /// IDotNetDataSourceTypeahead - Any typeahead text
        /// </summary>
        public string Typeahead { get; set; }

        /// <summary>
        /// IDotNetDataSourceKeyed
        /// </summary>
        [DefaultValue(null)]
        public string[] Keys { private get; set; }

        /// <summary>
        /// This is the query method called from the DotNetDataSource
        /// </summary>
        /// <param name="contextId"></param>
        /// <returns>a collection of key / labels for the picker</returns>
        public IEnumerable<KeyValuePair<string, string>> GetEditorDataItems(int contextId)
        {
            var flickrConnection = FlickrManager.GetFlickrConnection(this.ApiKey, this.ApiSecret);
            IEnumerable<FlickrImage> flickrImages;

            if (this.Keys != null)
            {
                flickrImages = this.Keys.Select(x => flickrConnection.GetFlickrImage(x));
            }
            else
            {
                PhotoSearchOptions photoSearchOptions = new PhotoSearchOptions();
                photoSearchOptions.MediaType = MediaType.Photos;
                photoSearchOptions.SafeSearch = SafetyLevel.Safe;
                photoSearchOptions.SortOrder = PhotoSearchSortOrder.Relevance;

                if (!string.IsNullOrWhiteSpace(this.Tags))
                {
                    photoSearchOptions.TagMode = TagMode.AllTags;
                    photoSearchOptions.Tags = this.Tags;
                }

                if (!string.IsNullOrWhiteSpace(this.Username))
                {
                    photoSearchOptions.Username = this.Username;
                }

                if (!string.IsNullOrWhiteSpace(this.Typeahead))
                {
                    photoSearchOptions.Text = this.Typeahead;
                }

                flickrImages = flickrConnection.GetFlickrImages(photoSearchOptions);                                
            }

            return flickrImages.Select(x => new KeyValuePair<string, string>(x.PhotoId, "<img src='" + x.SquareThumbnailUrl + "' />"));
        }
    }
}