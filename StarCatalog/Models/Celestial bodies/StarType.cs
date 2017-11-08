using System.Runtime.Serialization;

namespace StarCatalog
{
    [DataContract]
    public enum StarType
    {
        [EnumMember] O,
        [EnumMember] B,
        [EnumMember] A,
        [EnumMember] F,
        [EnumMember] G,
        [EnumMember] K,
        [EnumMember] M
    }
}