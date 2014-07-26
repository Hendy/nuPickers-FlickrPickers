namespace nuPickers.FlickrPickers
{
    using FlickrNet;
    using System.Runtime.Caching;

    internal sealed class FlickrConnection
    {
        internal string ApiKey { get; private set; }

        internal string ApiSecret { get; private set; }

        private Flickr Flickr { get; set; }

        internal FlickrConnection(string apiKey, string apiSecret)
        {
            this.ApiKey = apiKey;
            this.ApiSecret = apiSecret;

            this.Flickr = new Flickr(this.ApiKey, this.ApiSecret);
            this.Flickr.InstanceCacheDisabled = true;
        }

        internal PhotoInfo PhotosGetInfo(string key)
        {
            string cacheKey = this.ApiKey + key;
            PhotoInfo photoInfo = null;
            ObjectCache cache = MemoryCache.Default;

            if (!cache.Contains(cacheKey))
            {
                photoInfo = this.Flickr.PhotosGetInfo(key);
                cache.Add(
                        new CacheItem(cacheKey, photoInfo),
                        new CacheItemPolicy() { SlidingExpiration = new System.TimeSpan(0, 1, 0) });
            }

            return cache.Get(cacheKey) as PhotoInfo;
        }
    }
}