using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame.Coding;

namespace NetFrame
{
    public delegate byte[] LengthEncode(byte[] value);
    public delegate byte[] LengthDecode(ref List<byte> value);

    public delegate byte[] ObjectEncode(NetPacket value);
    public delegate NetPacket ObjectDecode(byte[] value);
}
