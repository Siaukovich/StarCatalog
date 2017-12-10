using System;
using System.Runtime.Serialization;

namespace StarCatalog
{
    [DataContract, KnownType(typeof(Star)), KnownType(typeof(Planet))]
    public abstract class CelestialBody : INameable, IEquatable<CelestialBody>
    {
        #region Protected Fields

        protected string _name;
        protected double _radius;
        protected double _mass;

        #endregion

        #region Public Propereties

        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Name cannot be null or empty", nameof(value));

                _name = value;
            }
        }

        [DataMember]
        public double Radius
        {
            get => _radius;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _radius = value;
            }
        }

        [DataMember]
        public double Mass
        {
            get => _mass;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _mass = value;
            }
        }

        public bool IsConstellation => false;


        #endregion

        #region Constructor

        protected CelestialBody(string name, float radius, double mass)
        {
            this.Name = name;
            this.Radius = radius;
            this.Mass = mass;
        }

        protected CelestialBody()
        {
            
        }

        #endregion
        
        #region IEquatable and ToString

        public override bool Equals(object obj)
        {
            return obj is CelestialBody c1 &&
                   Equals(c1);
        }

        public bool Equals(CelestialBody other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return this.Name.Equals(other.Name) &&
                   this.Radius.Equals(other.Radius) &&
                   this.Mass.Equals(other.Mass);
        }

        public override string ToString()
        {
            return $"Name: {Name}\n" +
                   $"Radius: {Radius:E2} m\n" +
                   $"Mass: {Mass:E2} kg\n";
        }

        #endregion
    }
}