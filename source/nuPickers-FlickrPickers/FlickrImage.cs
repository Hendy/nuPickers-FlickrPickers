
namespace nuPickers.FlickrPickers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FlickrNet;

    /// <summary>
    /// Flickr API returns different objects for a photo serach (a collection of Photo objects), and getting a single photo (a PhotoInfo object)
    /// This class is used to present a common type (as Photo and PhotoInfo are subtly different and cannot be converted either way)
    /// https://flickrnet.codeplex.com/discussions/280102
    /// </summary>
    public class FlickrImage
    {
        public string PhotoId { get; internal set; }

        public DateTime DateTaken { get; internal set; }

        public DateTime DateUploaded { get; internal set; }

        public string Title { get; internal set; }

        public string Description { get; internal set; }

        public IEnumerable<string> Tags { get; set; }

        public string OriginalUrl { get; internal set; }

        public string LargeUrl { get; internal set; }

        public string MediumUrl { get; internal set; }

        public string SmallUrl { get; internal set; }

        public string ThumbnailUrl { get; internal set; }

        public string SquareThumbnailUrl { get; internal set; }

        public string WebUrl { get; internal set; }

        public static explicit operator FlickrImage(Photo photo)
        {
            return new FlickrImage()
            {
                PhotoId = photo.PhotoId,
                DateTaken = photo.DateTaken,
                DateUploaded = photo.DateUploaded,
                Title = photo.Title,
                Description = photo.Description,
                Tags = photo.Tags,
                OriginalUrl = photo.OriginalUrl,
                LargeUrl = photo.LargeUrl,
                MediumUrl = photo.MediumUrl,
                SmallUrl = photo.SmallUrl,
                ThumbnailUrl = photo.ThumbnailUrl,
                SquareThumbnailUrl = photo.SquareThumbnailUrl,
                WebUrl = photo.WebUrl
            };
        }

        public static explicit operator FlickrImage(PhotoInfo photoInfo)
        {
            return new FlickrImage()
            {
                PhotoId = photoInfo.PhotoId,
                DateTaken = photoInfo.DateTaken,
                DateUploaded = photoInfo.DateUploaded,
                Title = photoInfo.Title,
                Description = photoInfo.Description,
                Tags = photoInfo.Tags.Select(x => x.TagText),
                OriginalUrl = photoInfo.OriginalUrl,
                LargeUrl = photoInfo.LargeUrl,
                MediumUrl = photoInfo.MediumUrl,
                SmallUrl = photoInfo.SmallUrl,
                ThumbnailUrl = photoInfo.ThumbnailUrl,
                SquareThumbnailUrl = photoInfo.SquareThumbnailUrl,
                WebUrl = photoInfo.WebUrl                
            };
        }
    }
}