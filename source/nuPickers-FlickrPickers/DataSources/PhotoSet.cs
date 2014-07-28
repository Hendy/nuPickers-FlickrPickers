
namespace nuPickers.FlickrPickers.DataSources
{
    using nuPickers.Shared.DotNetDataSource;
    using System.Collections.Generic;
    using System.Linq;

    public class PhotosInGallery : IDotNetDataSource
    {
        [DotNetDataSource(Title = "Flickr API Key", Description = "(required)")]
        public string ApiKey { get; set; }

        [DotNetDataSource(Title = "Flickr API Secret", Description = "(required)")]
        public string ApiSecret { get; set; }

        [DotNetDataSource(Title = "Photoset Id", Description = "(can be found in Flickr url)")]
        public string PhotosetId { get; set; }  

        IEnumerable<KeyValuePair<string, string>> IDotNetDataSource.GetEditorDataItems()
        {
            return FlickrManager.GetFlickrConnection(this.ApiKey, this.ApiSecret)
                       .GetFlickrImagesInPhotoset(this.PhotosetId)
                       .Select(x => new KeyValuePair<string, string>(x.PhotoId, "<img src='" + x.SquareThumbnailUrl + "' />"));
        }
    }
}