using System;
using System.Runtime.Serialization;

namespace StarCatalog
{
    [DataContract]
    public class Coordinates
    {
        #region Private Fields

        private Angle _declination;
        private Angle _rightAscension;

        #endregion

        #region Public Fields

        [DataMember]
        public Angle Declination
        {
            get => this._declination;
            set
            {
                var minValue = new Angle(-90);
                var maxValue = new Angle(90);
                var valueIsValid = value >= minValue && value <= maxValue;
                //if (!valueIsValid)
                //    throw new ArgumentOutOfRangeException(nameof(value), "DEC must be between -90 and 90");

                this._declination = value;
            }
        }

        [DataMember]
        public Angle RightAscension
        {
            get => this._rightAscension;
            set
            {
                var minValue = new Angle(0);
                var maxValue = new Angle(360f);
                var valueIsValid = value >= minValue && value <= maxValue;
                if (!valueIsValid)
                    throw new ArgumentOutOfRangeException(nameof(value), "RA must be between 0 and 360");

                this._rightAscension = value;
            }
        }


        #endregion

        #region Constructors

        public Coordinates(Angle declination, Angle rightAscension)
        { 
            this.Declination = declination;
            this.RightAscension = rightAscension;
        }

        public Coordinates()
        {
            this.Declination = new Angle();
            this.RightAscension = new Angle();
        }

        #endregion

        #region Helpers

        public override bool Equals(object other)
        {
            return other is Coordinates e &&
                   this.Declination.Equals(e.Declination) &&
                   this.RightAscension.Equals(e.RightAscension);
        }

        public override string ToString()
        {
            return $"Declination: {this._declination} \n" +
                   $"Right ascension: {this._rightAscension}\n";
        }

        #endregion
    }
}