
namespace nuPickers.FlickrPickers
{
    using nuPickers.Shared.DotNetDataSource;
    using System.Collections.Generic;
    using FlickrNet;

    public class UserTags : BaseFlickrPicker, IDotNetDataSource
    {
        [DotNetDataSource(Description = "Flickr UserName")]
        public string FlickrUserName { get; set; }

        [DotNetDataSource(Description = "Tags")]
        public string Tags { get; set; }

        IEnumerable<KeyValuePair<string, string>> IDotNetDataSource.GetEditorDataItems() 
        {
            List<KeyValuePair<string, string>> photos = new List<KeyValuePair<string, string>>();
            
            string userId;
            try
            {
                FoundUser foundUser = this.Flickr.PeopleFindByUserName(this.FlickrUserName);
                userId = foundUser.UserId;
            }
            catch (FlickrApiException)
            {
                userId = null;
            }

            if (userId != null)
            {
                PhotoSearchOptions photoSearchOptions = new PhotoSearchOptions();
                photoSearchOptions.UserId = userId;
                photoSearchOptions.Tags = this.Tags;
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
