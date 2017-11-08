using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace StarCatalog
{
    [DataContract]
    public class Constellation : INameable, IEquatable<Constellation>
    {
        #region Private Field

        private string _name;
        private string _imageUri;

        #endregion

        #region Public Propereties

        [DataMember]
        public Coordinates Coordinates { get; set; }
        [DataMember]
        public List<Star> Stars { get; set; } = new List<Star>();
        public int Index { get; set; }
        [DataMember]
        public string Host { get; set; } = "None";
        public bool IsConstellation => true;

        [DataMember]
        public string ImageUri
        {
            get => _imageUri;
            set
            {
                const string defaultPicturePath = @"\StarCatalog;component\Images\NoImage.png";

                if (String.IsNullOrEmpty(value))
                {
                    _imageUri = defaultPicturePath;
                    return;
                }
                // Get drives letters in one string.
                var drives = DriveInfo.GetDrives()
                                      .Select(d => d.Name[0].ToString())
                                      .Aggregate((n1, n2) => n1 + n2);

                // Patern that catches full path.
                var fullPathPattern = @"^[" + drives + @"]:(\\.+)*\.(jpg|png)$";
                var validFullName = Regex.IsMatch(value, fullPathPattern);
                if (validFullName && File.Exists(value))
                {
                    _imageUri = value;
                }
                else
                {
                    var fullPath = Environment.CurrentDirectory;
                    const string cutOffPart = @"Debug\bin";
                    value = fullPath.Substring(0, fullPath.Length - cutOffPart.Length) + @"Images\" + value;
                    if (!String.IsNullOrEmpty(value) && File.Exists(value))
                        _imageUri = value;
                    else
                        _imageUri = defaultPicturePath;
                }
            }
        }

        public int AmountOfStars => Stars.Count;
        public int AmountOfPlanets => Stars.Sum(star => star.Planets.Count);
        
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                // Only letters and whitespaces are allowed.
                if (!IsValidName(value))
                    throw new ArgumentException("Constellation name must be in latin!");

                _name = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Image name must be tha same as picture name in folder Images.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="coordinates"></param>
        public Constellation(string name, Coordinates coordinates)
        {
            this.Name = name;
            this.Coordinates = coordinates;
            this.ImageUri = name + ".png";
        }

        public Constellation()
        {
        }

        #endregion

        #region Helpers

        public void AddStar(string name, float radius, double mass, float temperature)
        {
            var newStar = new Star(name, radius, mass, temperature, this);
            this.Stars.Add(newStar);
        }

        public void AddStar(Star star)
        {
            this.Stars.Add(star);
        }

        public static bool IsValidName(string name)
        {
            return name.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c));
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Constellation other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;
                   
            return this.Name.Equals(other.Name) &&
                   this.Coordinates.Equals(other.Coordinates);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            return obj is Constellation c &&  
                   Equals(c);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_name != null ? _name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Coordinates != null ? Coordinates.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Stars != null ? Stars.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion
    }
}