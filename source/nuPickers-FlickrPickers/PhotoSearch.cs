﻿
namespace nuPickers.FlickrPickers
{
    using System.Collections.Generic;
    using FlickrNet;
    using nuPickers.Shared.DotNetDataSource;

    /// <summary>
    /// Implementing IDotNetDataSource, enables this class to be used with any "nuPicker: DotNet ... Picker"
    /// Implementing IDotNetDataSourceTypeahead, enables this class to recieve any typeahead text from a "nuPicker DotNet TypeaheadList Picker"
    /// </summary>
    public class PhotoSearch : IDotNetDataSource, IDotNetDataSourceTypeahead
    {
        [DotNetDataSource(Title="Flickr API Key", Description="(required)")]
        public string Key { get; set; }

        [DotNetDataSource(Title="Flickr API Secret", Description="(required)")]
        public string Secret { get; set; }

        [DotNetDataSource(Title="Max Photos", Description="maximum number of photos to return (defaults to 100)")]
        public string MaxPhotos { get; set; }

        [DotNetDataSource(Description="comma delimited list of tags")]
        public string Tags { get; set; }

        [DotNetDataSource(Title="Flickr Username", Description="photos from this this user")]
        public string Username { get; set; }  

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
            // prepare collection to return
            List<KeyValuePair<string, string>> photos = new List<KeyValuePair<string, string>>();     

            // begin query
            PhotoSearchOptions photoSearchOptions = new PhotoSearchOptions();            
            photoSearchOptions.MediaType = MediaType.Photos;
            photoSearchOptions.SafeSearch = SafetyLevel.Safe;
            photoSearchOptions.Extras = PhotoSearchExtras.OriginalUrl;
            photoSearchOptions.SortOrder = PhotoSearchSortOrder.Relevance;

            int maxPhotos;
            if (int.TryParse(this.MaxPhotos, out maxPhotos))
            {
                photoSearchOptions.PerPage = maxPhotos;
            }

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

            PhotoCollection photoCollection;

            try
            {
                Flickr.CacheDisabled = true;
                Flickr flickr = new Flickr(this.Key, this.Secret);
                flickr.InstanceCacheDisabled = true;

                photoCollection = flickr.PhotosSearch(photoSearchOptions);
            }
            catch
            {
                photoCollection = null;
            }

            if (photoCollection != null)
            {
                foreach (var photo in photoCollection)
                {
                    // build an item for the picker, appending flickr data into data-attributes within the label (these could then be parsed by a Custom Label macro)
                    photos.Add(new KeyValuePair<string, string>(photo.PhotoId, "<span data-original-url='" + photo.OriginalUrl + @"'
                                                                                      data-owner-name='" + photo.OwnerName + @"'
                                                                                      data-title='" + photo.Title + @"'>
                                                                                    <img src='" + photo.LargeSquareThumbnailUrl + @"' />
                                                                                </span>"));
                }
            }

            return photos;
        }

    }
}
