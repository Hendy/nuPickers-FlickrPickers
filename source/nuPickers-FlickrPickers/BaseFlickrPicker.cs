
namespace nuPickers.FlickrPickers
{
    using nuPickers.Shared.DotNetDataSource;
    using FlickrNet;

    public abstract class BaseFlickrPicker
    {
        [DotNetDataSource]
        public string Key { get; set; }

        [DotNetDataSource]
        public string Secret { get; set; }

        private Flickr flickr = null;

        protected Flickr Flickr
        {
            get
            {
                if (this.flickr == null)
                {
                    Flickr.CacheDisabled = true;
                    this.flickr = new Flickr(this.Key, this.Secret);
                    this.flickr.InstanceCacheDisabled = true;
                }

                return this.flickr;
            }
        }
    }
}
