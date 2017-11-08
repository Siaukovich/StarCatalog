using System;
using System.ComponentModel;

namespace StarCatalog
{
    class ConstellationChecker : IDataErrorInfo
    {
        private string _name;
        private float _rightAscension;
        private float _declination;

        public string Name
        {
            get => _name;
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException(nameof(value));

                _name = value;
            }
        }

        public float RightAscension
        {
            get => _rightAscension;
            set
            {
                var c = new Constellation("Checker", new Coordinates(new Angle(), new Angle(value)));
                _rightAscension = value;
            }
        }

        public float Declination
        {
            get => _declination;
            set
            {
                if (value < -90 || value > 90)
                    throw new ArgumentOutOfRangeException(nameof(value));

                var c = new Constellation("Checker", new Coordinates(new Angle(value), new Angle()));
                _declination = value;
            }
        }

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
                    case nameof(Declination):
                        if (Declination < -90 || Declination > 90)
                        {
                            error = "Invalid declination";
                        }
                        break;
                    case nameof(RightAscension):
                        if (RightAscension < 0 || RightAscension >= 360)
                        {
                            error = "Invalid right ascension";
                        }
                        break;
                }
                return error;
            }
        }

        public string Error { get; }
    }
}
