using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordInfo.SimplifiedTypes
{
    [ProtoContract]
    public struct SimpleVector3
    {
        [ProtoMember(1)]
        public float X { get; set; }
        [ProtoMember(2)]
        public float Y { get; set; }
        [ProtoMember(3)]
        public float Z { get; set; }
        public SimpleVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
