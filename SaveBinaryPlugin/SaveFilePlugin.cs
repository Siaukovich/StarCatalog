using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Windows;
using Microsoft.Win32;
using StarCatalog;
using StarCatalog.Helpers;

namespace SaveBinaryPlugin
{
    public class SaveFilePlugin : IPluginable
    {
        public string Name => "Save data to file";

        private bool _saveFileSuccessfully;

        public void Start()
        {
            _saveFileSuccessfully = false;

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Choose (*.txt)|*.txt"
            };

            if (saveFileDialog.ShowDialog() != true)
            {
                _saveFileSuccessfully = true;
                return;
            }

            var fullPathToFile = saveFileDialog.FileName;
            SerializeCollectionXml(ConstellationCollectionManager.Constellations, fullPathToFile);
            InfoStateController.Reset();
        }

        private static void SerializeCollectionXml(IEnumerable collection, string fullPathToFile)
        {
            var ds = new NetDataContractSerializer();
            using (var stream = File.Create(fullPathToFile))
                using (var deflateStream = new DeflateStream(stream, CompressionLevel.Optimal))
                    ds.WriteObject(deflateStream, collection);
        }

        public void ShowFinalMessage()
        {
            if (_saveFileSuccessfully)
                MessageBox.Show("Saved!");
        }
    }
}
