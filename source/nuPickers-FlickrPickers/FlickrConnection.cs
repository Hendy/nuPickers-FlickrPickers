namespace nuPickers.FlickrPickers
{
    using FlickrNet;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    internal sealed class FlickrConnection
    {
        internal string ApiKey { get; private set; }

        internal string ApiSecret { get; private set; }

        private Flickr Flickr { get; set; }

        private MemoryCache MemoryCache { get; set; }

        private PhotoSearchExtras DefaultPhotoSearchExtras
        {
            get
            {
                PhotoSearchExtras photoSearchExtras = new PhotoSearchExtras();

                photoSearchExtras |= PhotoSearchExtras.OriginalUrl;
                photoSearchExtras |= PhotoSearchExtras.Tags;

                return photoSearchExtras;
            }
        }

        internal FlickrConnection(string apiKey, string apiSecret)
        {
            this.ApiKey = apiKey;
            this.ApiSecret = apiSecret;

            this.Flickr = new Flickr(this.ApiKey, this.ApiSecret);
            this.Flickr.InstanceCacheDisabled = true;

            // scope cache to this connection only
            this.MemoryCache = new MemoryCache(this.ApiKey + this.ApiSecret);
        }

        internal IEnumerable<FlickrImage> GetFlickrImagesInPhotoset(string photosetId)
        {
            List<FlickrImage> flickrImages = new List<FlickrImage>();

            PhotosetPhotoCollection photosetPhotoCollection;

            try
            {
                photosetPhotoCollection = this.Flickr.PhotosetsGetPhotos(photosetId, this.DefaultPhotoSearchExtras);
            }
            catch
            {
                photosetPhotoCollection = null;
            }

            if (photosetPhotoCollection != null)
            {
                foreach(Photo photo in photosetPhotoCollection)
                {
                    flickrImages.Add(this.CacheFlickrImage((FlickrImage)photo));
                }
            }

            return flickrImages;
        }

        internal IEnumerable<FlickrImage> GetFlickrImages(PhotoSearchOptions photoSearchOptions)
        {
            List<FlickrImage> flickrImages = new List<FlickrImage>();

            PhotoCollection photoCollection;

            photoSearchOptions.Extras = this.DefaultPhotoSearchExtras;

            try
            {
                photoCollection = this.Flickr.PhotosSearch(photoSearchOptions);
            }
            catch
            {
                photoCollection = null;
            }

            if (photoCollection != null)
            {
                foreach (Photo photo in photoCollection)
                {
                    flickrImages.Add(this.CacheFlickrImage((FlickrImage)photo));
                }
            }

            return flickrImages; 
        }

        /// <summary>
        /// Get FlickrImage from cache, otherwise request then add to cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal FlickrImage GetFlickrImage(string key)
        {
            FlickrImage flickrImage = this.MemoryCache.Get(key) as FlickrImage;
            
            if (flickrImage == null)
            {
                PhotoInfo photoInfo;

                try
                {
                    photoInfo = this.Flickr.PhotosGetInfo(key);
                }
                catch
                {
                    photoInfo = null;
                }

                if (photoInfo != null)
                {
                    flickrImage = this.CacheFlickrImage((FlickrImage)photoInfo);    
                }                
            }

            return flickrImage;
        }

        /// <summary>
        /// Add the flickrImage to the cache
        /// </summary>
        /// <param name="flickrImage"></param>
        /// <returns></returns>
        private FlickrImage CacheFlickrImage(FlickrImage flickrImage)
        {
            this.MemoryCache.Set(
                new CacheItem(flickrImage.PhotoId, flickrImage),
                new CacheItemPolicy() { SlidingExpiration = new System.TimeSpan(0, 5, 0) });
               
            return flickrImage;
        }
    }
}