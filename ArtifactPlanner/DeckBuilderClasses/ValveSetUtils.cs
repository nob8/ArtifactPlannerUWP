using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArtifactPlanner.DeckBuilderClasses
{
    class OfficialArtifactSetManifest
    {
        public string cdn_root;
        public string url;
        public long expire_time;
    }

    class ValveSerializationUtils
    {
        public static List<ValveJsonSet> DownloadValveSetData()
        {
            BackgroundDownloader downloader = new BackgroundDownloader();
            string manifestLocation = @"Assets\CardDefinitions\official-manifest-{0}.json";
            string dataLocation = @"Assets\CardDefinitions\official-data-{0}.json";
            string manifestUriBase = "https://playartifact.com/cardset/{0}";
            int valveSetIndex = 0;
            List<ValveJsonSet> sets = new List<ValveJsonSet>();
            while (true)
            {
                var officialManifest = ApplicationData.Current.LocalFolder
                    .CreateFileAsync(String.Format(manifestLocation, valveSetIndex.ToString("D2")), 
                        CreationCollisionOption.ReplaceExisting).GetAwaiter().GetResult();
                var download =
                    downloader.CreateDownload(new Uri(String.Format(manifestUriBase, valveSetIndex.ToString("D2"))), officialManifest);
                var results = download.StartAsync().GetAwaiter().GetResult();
                if (results.GetResponseInformation().StatusCode != 200)
                {
                    break;
                }
                var test = ValveSerializationUtils.DeserializeArtifactSetManifestFile(officialManifest);
                var officialData = ApplicationData.Current.LocalFolder
                    .CreateFileAsync(String.Format(dataLocation, valveSetIndex.ToString("D2")),
                        CreationCollisionOption.ReplaceExisting).GetAwaiter().GetResult();
                download = downloader.CreateDownload(new Uri(test.cdn_root + test.url), officialData);
                results = download.StartAsync().GetAwaiter().GetResult();
                if (results.GetResponseInformation().StatusCode != 200)
                {
                    throw new InvalidOperationException("Expected data but got error!" +
                                                        results.GetResponseInformation());
                }
                sets.Add(DeserializeArtifactSetData(officialData));
            }
            return sets;
        }

        public static OfficialArtifactSetManifest DeserializeArtifactSetManifestFile(StorageFile src)
        {
            var srcContents = (src.OpenSequentialReadAsync()).GetAwaiter().GetResult().AsStreamForRead();
            var serializer = new JsonSerializer();
            using (var sr = new StreamReader(srcContents))
            using (var reader = new JsonTextReader(sr))
            {
                while (reader.Read())
                {
                    if (reader.TokenType != JsonToken.StartObject || !(serializer.Deserialize(reader) is JObject jsonObject)) continue;
                    var interiorObject = (OfficialArtifactSetManifest)jsonObject.ToObject(typeof(OfficialArtifactSetManifest));
                    return interiorObject;
                }
            }

            return null;
        }
        public static ValveJsonSet DeserializeArtifactSetData(StorageFile src)
        {
            var srcContents = (src.OpenSequentialReadAsync()).GetAwaiter().GetResult().AsStreamForRead();
            var serializer = new JsonSerializer();
            using (var sr = new StreamReader(srcContents))
            using (var reader = new JsonTextReader(sr))
            {
                while (reader.Read())
                {
                    if (reader.TokenType != JsonToken.StartObject || !(serializer.Deserialize(reader) is JObject jsonObject)) continue;
                    var interiorObject = (ValveJsonSet)jsonObject.GetValue("card_set").ToObject(typeof(ValveJsonSet));
                    return interiorObject;
                }
            }

            return null;
        }
    }
}
