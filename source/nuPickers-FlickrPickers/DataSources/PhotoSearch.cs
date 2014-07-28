
namespace nuPickers.FlickrPickers.DataSources
{
    using FlickrNet;
    using nuPickers.Shared.DotNetDataSource;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implementing IDotNetDataSource, enables this class to be used with any "nuPicker: DotNet ... Picker"
    /// Implementing IDotNetDataSourceTypeahead, enables this class to recieve any typeahead text from a "nuPicker DotNet TypeaheadList Picker"
    /// </summary>
    public class PhotoSearch : IDotNetDataSource, IDotNetDataSourceTypeahead
    {
        [DotNetDataSource(Title = "Flickr API Key", Description = "(required)")]
        public string ApiKey { get; set; }

        [DotNetDataSource(Title = "Flickr API Secret", Description = "(required)")]
        public string ApiSecret { get; set; }

        [DotNetDataSource(Title = "Flickr Username", Description = "photos from this this user")]
        public string Username { get; set; }  

        [DotNetDataSource(Description="comma delimited list of tags")]
        public string Tags { get; set; }

        /// <summary>
        /// The current typeahead text (this is only set if using a DotNet TypeaheadList Picker)
        /// </summary>
        string IDotNetDataSourceTypeahead.Typeahead { get; set; }

        /// <summary>
        /// Helper to get at the Typeahead value
        /// </summary>
        private string Typeahead
        {
            get
            {
                return ((IDotNetDataSourceTypeahead)this).Typeahead;
            }
        }

        /// <summary>
        /// This is the main method called from the DotNetDataSource
        /// </summary>
        /// <returns>a collection of key / labels for the picker</returns>
        IEnumerable<KeyValuePair<string, string>> IDotNetDataSource.GetEditorDataItems()
        {
            // begin query
            PhotoSearchOptions photoSearchOptions = new PhotoSearchOptions();            
            photoSearchOptions.MediaType = MediaType.Photos;
            photoSearchOptions.SafeSearch = SafetyLevel.Safe;
            photoSearchOptions.Extras |= PhotoSearchExtras.OriginalUrl;
            photoSearchOptions.Extras |= PhotoSearchExtras.Tags;
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

            // if being used by a typeahead picker...
            if (!string.IsNullOrWhiteSpace(this.Typeahead))
            {
                photoSearchOptions.Text = this.Typeahead;
            }

            return FlickrManager.GetFlickrConnection(this.ApiKey, this.ApiSecret)
                                .GetFlickrImages(photoSearchOptions)
                                .Select(x => new KeyValuePair<string, string>(x.PhotoId, "<img src='" + x.SquareThumbnailUrl + "' />"));
        }

    }
}
