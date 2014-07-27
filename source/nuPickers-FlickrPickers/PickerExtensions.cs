namespace nuPickers.FlickrPickers
{
    using Newtonsoft.Json.Linq;
    using nuPickers;
    using nuPickers.Shared.DotNetDataSource;
    using System.Collections.Generic;
    using System.Linq;

    public static class PickerExtensions
    {
        /// <summary>
        /// Query Flickr to get a PhotoInfo object for each picked key
        /// </summary>
        /// <param name="picker">the nuPicker Picker</param>
        /// <returns>a collection of FlickrNet.PhotoInfo objects</returns>
        public static IEnumerable<FlickrImage> GetPickedFlickrImages(this Picker picker)
        {
            List<FlickrImage> flickrImages = new List<FlickrImage>();

            foreach (string pickedKey in picker.PickedKeys)
            {
                flickrImages.Add(picker.GetFlickrImage(pickedKey));
            }

            return flickrImages;
        }

        public static FlickrImage GetFlickrImage(this Picker picker, string key)
        {
            DotNetDataSource dotNetDataSource = JObject.Parse(picker.GetDataTypePreValue("dataSource").Value).ToObject<DotNetDataSource>();

            string apiKey = dotNetDataSource.Properties.Single(x => x.Name == "Key").Value;
            string apiSecret = dotNetDataSource.Properties.Single(x => x.Name == "Secret").Value;

            return FlickrManager.GetFlickrConnection(apiKey, apiSecret).GetFlickrImage(key);
        }
    }
}
