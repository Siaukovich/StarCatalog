using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;

namespace StarCatalog
{
    public static class Serializer
    {
        public static void SerializeCollectionXml(IEnumerable collection, string fullPathToFile)
        {
            var ds = new NetDataContractSerializer();
            using (var stream = File.Create(fullPathToFile))
                using (var deflateStream = new DeflateStream(stream, CompressionLevel.Optimal))
                    ds.WriteObject(deflateStream, collection);
        }

        public static T DeserializeXml<T>(string fullPathToFile)
        {
            var ds = new NetDataContractSerializer();
            using (var stream = File.OpenRead(fullPathToFile))
                using (var deflateStream = new DeflateStream(stream, CompressionMode.Decompress))
                    return (T)ds.ReadObject(deflateStream);
        }
    }
}
