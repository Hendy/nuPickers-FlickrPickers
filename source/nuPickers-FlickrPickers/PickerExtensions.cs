namespace nuPickers.FlickrPickers
{
    using FlickrNet;
    using Newtonsoft.Json.Linq;
    using nuPickers.PropertyValueConverters;
    using nuPickers.Shared.DotNetDataSource;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class PickerExtensions
    {
        /// <summary>
        /// Query Flickr to get a PhotoInfo object for each picked key
        /// </summary>
        /// <param name="picker">the nuPicker Picker</param>
        /// <returns>a collection of FlickrNet.PhotoInfo objects</returns>
        public static IEnumerable<PhotoInfo> GetPhotoInfos(this Picker picker)
        {
            List<PhotoInfo> photoInfos = new List<PhotoInfo>();

            DotNetDataSource dotNetDataSource = 
                JObject.Parse(picker.DataTypePreValues.Single(x => string.Equals(x.Key, "dataSource", StringComparison.InvariantCultureIgnoreCase)).Value.Value)
                .ToObject<DotNetDataSource>();

            try
            {
                Flickr.CacheDisabled = true;
                Flickr flickr = new Flickr(dotNetDataSource.Properties.Single(x => string.Equals(x.Name, "Key")).Value,
                                           dotNetDataSource.Properties.Single(x => string.Equals(x.Name, "Secret")).Value);
                flickr.InstanceCacheDisabled = true;

                foreach (string pickedKey in picker.PickedKeys)
                {
                    photoInfos.Add(flickr.PhotosGetInfo(pickedKey));
                }
            }
            catch
            {
            }

            return photoInfos;
        }
    }
}
