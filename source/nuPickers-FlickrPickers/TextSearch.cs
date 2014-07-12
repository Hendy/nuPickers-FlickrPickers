namespace nuPickers.FlickrPickers
{
    using nuPickers.Shared.DotNetDataSource;
    using System.Collections.Generic;
    using FlickrNet;

    public class TextSearch : BaseFlickrPicker, IDotNetDataSource, IDotNetDataSourceTypeahead
    {
        string IDotNetDataSourceTypeahead.Typeahead { get; set; }

        IEnumerable<KeyValuePair<string, string>> IDotNetDataSource.GetEditorDataItems()
        {
            List<KeyValuePair<string, string>> photos = new List<KeyValuePair<string, string>>();

            string typeahead = ((IDotNetDataSourceTypeahead)this).Typeahead;

            if (!string.IsNullOrWhiteSpace(typeahead))
            {
                PhotoSearchOptions photoSearchOptions = new PhotoSearchOptions();
                photoSearchOptions.Text = typeahead;
                photoSearchOptions.PerPage = 10;

                foreach (var photo in this.Flickr.PhotosSearch(photoSearchOptions))
                {
                    photos.Add(new KeyValuePair<string, string>(photo.PhotoId, "<img src='" + photo.LargeSquareThumbnailUrl + "' />"));
                }
            }

            return photos;
        }
    }
}
