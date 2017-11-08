using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace StarCatalog
{
    [DataContract]
    public class Star : CelestialBody, IEquatable<Star>, IDataErrorInfo
    {
        #region Private Fields

        private double _temperature;

        #endregion

        #region Propereties

        [DataMember]
        public new double Radius
        {
            get => _radius;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _radius = value;
                SetLuminosity();
            }
        }

        [DataMember]
        public double Temperature
        {
            get => _temperature;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _temperature = value;
                SetLuminosity();
                SetType();
            }
        }

        [DataMember]
        public List<Planet> Planets { get; set; } = new List<Planet>();

        [DataMember]
        public double Luminosity { get; private set; }

        [DataMember]
        public StarType Type { get; private set; }

        [DataMember]
        public Constellation Host { get; set; }

        public int AmountOfPlanets => Planets.Count;

        #endregion

        #region Constructor

        public Star(string name, float radius, double mass, float temperature, Constellation host) 
            : base(name, radius, mass)
        {
            this.Temperature = temperature;
            this.Host = host;
        }

        public Star()
        {
        }

        #endregion

        #region Custom Methods

        public void AddPlanet(string name, float radius, double mass,
            float siderealDay, float siderealYear, float orbitRadius)
        {
            var newPlanet = new Planet(name, radius, mass, siderealDay, siderealYear, this, orbitRadius);
            Planets.Add(newPlanet);
        }

        public void AddPlanet(Planet planet)
        {
            Planets.Add(planet);
        }

        public string FullName => base.ToString() +
                                  $"Temprature in Kelvins: {Temperature}\n" +
                                  $"Luminosity in Watts: {Luminosity}\n" +
                                  $"Type: {Type}\n";

        #endregion

        #region Helpers

        private void SetLuminosity()
        {
            const float sigma = 5.67e-8f;
            this.Luminosity = 4 * Math.PI * Math.Pow(this.Radius, 2) * 
                sigma * Math.Pow(this.Temperature, 4);
        }

        private void SetType()
        {
            if (this.Temperature <= 2400)
                this.Type = StarType.M;
            else if (this.Temperature <= 5200)
                this.Type = StarType.K;
            else if (this.Temperature <= 6000)
                this.Type = StarType.G;
            else if (this.Temperature <= 7500)
                this.Type = StarType.F;
            else if (this.Temperature <= 10_000)
                this.Type = StarType.A;
            else if (this.Temperature <= 30_000)
                this.Type = StarType.B;
            else
                this.Type = StarType.O;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Star other)
        {
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && 
                   this.Radius.Equals(other.Radius) && 
                   this.Luminosity.Equals(other.Luminosity) && 
                   this.Temperature.Equals(other.Temperature);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            return obj is Star s &&
                   this.Equals(s);
        }

        #endregion

        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case nameof(Name):
                        if (String.IsNullOrEmpty(Name))
                        {
                            error = "Invalid name";
                        }
                        break;
                    case nameof(Radius):
                        if (Radius <= 0)
                        {
                            error = "Invalid radius";
                        }
                        break;
                    case nameof(Mass):
                        if (Mass <= 0)
                        {
                            error = "Invalid mass";
                        }
                        break;
                    case nameof(Temperature):
                        if (Temperature <= 0)
                        {
                            error = "Invalid temperature";
                        }
                        break;
                }
                return error;
            }
        }

        public string Error { get; }
    }
}