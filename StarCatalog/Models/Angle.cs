using System;
using System.Runtime.Serialization;

namespace StarCatalog
{
    [DataContract]
    public class Angle
    {
        [DataMember]
        public float Value { get; set; }

        public Angle(float value = 0)
        {
            this.Value = value;
        }

        public static bool operator <=(Angle angle1, Angle angle2) => 
            angle1.Value <= angle2.Value;

        public static bool operator >=(Angle angle1, Angle angle2) => 
            angle1.Value >= angle2.Value;
            
        /// <summary>
        /// Tries to parse from string to Angle.
        /// Returns true if success, false otherwise.
        /// </summary>
        /// <param name="str">Must be in form "DD MM SS.SS"</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryParse(string str, out Angle value)
        {
            value = new Angle();
            if (!Single.TryParse(str, out float result))
                return false;

            value.Value = result;
            return true;
        }

        public override bool Equals(object other)
        {
            return other is Angle a && this.Value.Equals(a.Value);
        }

        public override string ToString() => Value.ToString();       
    }
}