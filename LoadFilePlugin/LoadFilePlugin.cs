using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Windows;
using Microsoft.Win32;
using StarCatalog;
using StarCatalog.Helpers;

namespace LoadFilePlugin
{
    public class LoadFilePlugin : IPluginable
    {
        public string Name => "Load from file";

        private bool _readFileSuccessfully;

        public void Start()
        {
            _readFileSuccessfully = true;

            var openFileDialog = new OpenFileDialog()
            {
                Filter = "Text (*.txt)|*.txt"
            };

            if (openFileDialog.ShowDialog() != true)
            {
                _readFileSuccessfully = false;
                return;
            }

            var fullPathToFile = openFileDialog.FileName;
            IEnumerable<Constellation> collection;
            try
            {
                collection = DeserializeXml<IEnumerable<Constellation>>(fullPathToFile);
            }
            catch (Exception e) when (e is FileNotFoundException)
            {
                MessageBox.Show("File not found!");
                _readFileSuccessfully = false;
                return;
            }
            catch (Exception e) when (e is SerializationException || e is InvalidDataException)
            {
                MessageBox.Show("Something went wrong while reading from file!");
                _readFileSuccessfully = false;
                return;
            }

            ConstellationCollectionManager.ClearCollection();
            ConstellationCollectionManager.AddRange(collection);
            InfoStateController.Reset();
        }

        private static T DeserializeXml<T>(string fullPathToFile)
        {
            var ds = new NetDataContractSerializer();
            using (var stream = File.OpenRead(fullPathToFile))
                using (var deflateStream = new DeflateStream(stream, CompressionMode.Decompress))
                    return (T)ds.ReadObject(deflateStream);
        }

        public void ShowFinalMessage()
        {
            if (_readFileSuccessfully)
               MessageBox.Show("Loaded!");
        }
    }
}
