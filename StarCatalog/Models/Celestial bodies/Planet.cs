using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace StarCatalog
{
    [DataContract]
    public class Planet : CelestialBody, IEquatable<Planet>, IDataErrorInfo
    {
        #region Propereties

        /// <summary>
        /// Period of rotation around planets own axis in seconds.
        /// </summary>
        [DataMember]
        public float SiderealDay { get; set; }

        /// <summary>
        /// Period of ratation around the host star in seconds.
        /// </summary>
        [DataMember]
        public float SiderealYear { get; set; }

        [DataMember]
        public Star Host { get; set; }

        [DataMember]
        public float OrbitRadius { get; set; }

        public string FullName => base.ToString() +
                                  $"Period around axis: {SiderealDay:E2} s\n" +
                                  $"Period around star: {SiderealYear:E2} s\n" +
                                  $"Radius of orbit: {OrbitRadius} m\n";

        #endregion

        #region Constructor

        public Planet(string name, float radius, double mass, float siderealDay, 
            float siderealYear, Star host, float orbitRadius) 
            : base(name, radius, mass)
        {
            this.SiderealDay = siderealDay;
            this.SiderealYear = siderealYear;
            this.Host = host;
            this.OrbitRadius = orbitRadius;
        }

        public Planet() : base()
        {
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return base.ToString() +
                   $"Period around axis: {SiderealDay:E2} s\n" +
                   $"Period around star: {SiderealYear:E2} s\n" +
                   $"Host star: {Host.Name}\n" +
                   $"Radius of orbit: {OrbitRadius:E2} m\n";
        }

        public bool Equals(Planet other)
        {
            return base.Equals(other) &&
                   SiderealDay.Equals(other.SiderealDay) && 
                   SiderealYear.Equals(other.SiderealYear) && 
                   Equals(Host, other.Host) && 
                   OrbitRadius.Equals(other.OrbitRadius);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            return obj is Planet p &&
                   this.Equals(p);
        }

        #endregion

        public string this[string columnName]
        {
            get
            {
                var error = String.Empty;
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
                    case nameof(OrbitRadius):
                        if (OrbitRadius <= 0)
                        {
                            error = "Invalid orbit radius";
                        }
                        break;
                    case nameof(SiderealYear):
                        if (SiderealYear <= 0)
                        {
                            error = "Invalid sidereal year";
                        }
                        break;
                    case nameof(SiderealDay):
                        if (SiderealDay <= 0)
                        {
                            error = "Invalid sdereal day";
                        }
                        break;
                }
                return error;
            }
        }

        public string Error { get; }
    }
}