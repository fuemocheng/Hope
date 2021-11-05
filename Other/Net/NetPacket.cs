//#define Server_Heart_Head
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.IO;

//TODO data和zipmemory的内存池，解释协议时/解压缩返回时
public class NetPacket
{
#if Server_Heart_Head
    public const int PacketSubHeaderLen = 1 + 4 + 2 + 1;
#else
    public const int PacketSubHeaderLen = 4 + 2 + 1;
#endif
    public int number;
    public int cmd;
    public byte[] data;
    public int len;
    public byte zip;
    public int msgid;
    public byte[] _buffer = new byte[1024];

    public float remainTime;

    public void WriteBuffer(int packetSeq, ByteBuffer buffer)
    {
        //消息长度占位
        int pos = buffer.Position;
        buffer.WriteInt32(0);
#if Server_Heart_Head
        //服务区心跳标记
        buffer.Write(0);
#endif
        buffer.WriteInt32(packetSeq);
        buffer.WriteInt16((short)cmd);
        buffer.Write(0);
        buffer.Append(data);
        //buffer.InsertInt32(0, buffer.Count);

        //填充消息长
        int oldPos = buffer.Position;
        int oldCount = buffer.Count;
        buffer.Position = pos;
        buffer.WriteInt32(buffer.Count-4);
        buffer.Position = oldPos;
        buffer.Count = oldCount;


        len = buffer.Count;
    }

    public void ReadBufferData(ByteBuffer buffer, int readLen)
    {
        if (zip > 0)
        {
            var zipData = buffer.ReadBytes(readLen);
            var decompressor = new Inflater();
            decompressor.SetInput(zipData);
            var zipMemory = new MemoryStream(zipData.Length);

            //TODO alloc优化
            //byte[] buf = new byte[1024];
            while (!decompressor.IsFinished)
            {
                int count = decompressor.Inflate(_buffer);
                zipMemory.Write(_buffer, 0, count);
            }

            data = zipMemory.ToArray();
        }
        else
        {
            data = buffer.ReadBytes(readLen);
        }
    }
    public void ReadBuffer(ByteBuffer buffer,int len)
    {
#if Server_Heart_Head
        //服务器心跳标记 客户端暂时不用
        buffer.ReadByte();
#endif
        number = buffer.ReadInt32();
        cmd = buffer.ReadInt16();
        zip = buffer.ReadByte();

        ReadBufferData(buffer, len - NetPacket.PacketSubHeaderLen);
    }

    public void ReadBuffer(ByteBuffer buffer)
    {
        var len = buffer.ReadInt32();
#if Server_Heart_Head
        //服务器心跳标记 客户端暂时不用
        buffer.ReadByte();
#endif
        number = buffer.ReadInt32();
        cmd = buffer.ReadInt16();
        zip = buffer.ReadByte();

        ReadBufferData(buffer, buffer.Available);
    }
}