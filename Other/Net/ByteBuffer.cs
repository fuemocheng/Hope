/// <summary>
/// edit by five： 增加自动扩容, 扩容后, 如果超过10次以上出现剩余空间过大, 将大小回退至初始大小
/// </summary>

using System;

public class ByteBuffer
{
    public byte[] BBuffer { get; private set; }

    /// <summary>
    /// 当前容量
    /// </summary>
    /// <value></value>
    public int Capacity { get; private set; }
    /// <summary>
    /// 初始容量
    /// </summary>
    public int DefaultCapacity { get; private set; }
    /// <summary>
    /// 当前写入的字节总数
    /// </summary>
    /// <value></value>
    public int Count { get; set; }
    /// <summary>
    /// 当前读取位置
    /// </summary>
    /// <value></value>
    public int Position { get; set; }
    public int LastReadBlock { get; set; }

    // 容量冗余比例
    private float CapacityRatio { get; set; }
    // 
    public int Threashold { get; private set; }
    // 
    private int DefaultThreashold = 10;

    public ByteBuffer(int _capacity)
    {
        BBuffer = new byte[_capacity];
        Capacity = _capacity;
        DefaultCapacity = _capacity;
        Count = 0;
    }

    public int Available
    {
        get
        {
            return Count - Position;
        }
    }

    public void Append(ByteBuffer other)
    {
        var resultPos = Position + other.Count+Count;
        if (resultPos >= Capacity)
        {
            //throw new OutOfMemoryException("ByteBuffer Error : Out of Capacity");
            ReSize(resultPos);
        }

        Buffer.BlockCopy(other.BBuffer, 0, BBuffer, Count, other.Count);
        Count += other.Count;
    }

    public void Append(byte[] other)
    {
        var resultPos = Position + other.Length+Count;
        if (resultPos >= Capacity)
        {
            //throw new OutOfMemoryException("ByteBuffer Error : Out of Capacity");
            ReSize(resultPos);
        }

        Buffer.BlockCopy(other, 0, BBuffer, Position, other.Length);
        Count += other.Length;
    }

    private void ReSize(int newSize)
    {
        lock (this)
        {
            newSize = ExpandRatio(newSize);

            Capacity = newSize;
            byte[] larray = BBuffer;
            if (larray == null)
            {
                larray = new byte[newSize];
                return;
            }

            if (larray.Length != newSize)
            {
                byte[] newArray = new byte[newSize]; 
                Buffer.BlockCopy(BBuffer, 0, newArray, 0, larray.Length > newSize ? newSize : larray.Length);
                BBuffer = newArray;
            }
        }
    }

    private void ReSize()
    {
        ReSize(Capacity * 2);
    }

    private int ExpandRatio(int newSize)
    {
        var ratio = (int)System.Math.Ceiling((float)newSize / (float)Capacity);
        ratio = NextPowerOfTwo(ratio);
        return Capacity * ratio;
    }

    private int NextPowerOfTwo(int n)
    {
        if (n == 0) return 1;
        n--;
        n |= n >> 1;
        n |= n >> 2;
        n |= n >> 4;
        n |= n >> 8;
        n |= n >> 16;
        return n + 1;
    }

    public void Clear()
    {
        Position = 0;
        Count = 0;
    }

    public byte ReadByte()
    {
        Position++;

        return BBuffer[Position - 1];
    }

    public bool CanReadInt32()
    {
        return Count - Position >= 4;
    }

    public int ReadInt32()
    {
        int c1 = ReadByte();
        int c2 = ReadByte();
        int c3 = ReadByte();
        int c4 = ReadByte();
        LastReadBlock = 4;
        return (c1 << 24) | (c2 << 16) | (c3 << 8) | c4;
    }

    public int ReadInt16()
    {
        int a = ReadByte();
        int b = ReadByte();
        LastReadBlock = 2;
        return (a << 8) + b;
    }

    public byte[] ReadBytes(int count)
    {
        if (Available < count)
        {
            throw new OutOfMemoryException($"ByteBuffer Error : Out of Capacity, Available = {Available} Count = {count}");
        }
        var retBuf = new byte[count];
        Buffer.BlockCopy(BBuffer, Position, retBuf, 0, count);

        Position += count;

        return retBuf;
    }

    public void TruncateRead()
    {
        MoveBytes(Position, -Position);
        Position = 0;
    }

    public void Write(byte b)
    {
        BBuffer[Position] = b;
        Count += 1;
        Position += 1;
    }

    public void WriteInt32(int n)
    {
        BBuffer[Position + 0] = (byte)((n >> 24) & 0xff);
        BBuffer[Position + 1] = (byte)((n >> 16) & 0xff);
        BBuffer[Position + 2] = (byte)((n >> 8) & 0xff);
        BBuffer[Position + 3] = (byte)(n & 0xff);
        Count += 4;
        Position += 4;
    }

    public void WriteInt16(short n)
    {
        BBuffer[Position + 0] = (byte)((n >> 8) & 0xff);
        BBuffer[Position + 1] = (byte)(n & 0xff);
        Count += 2;
        Position += 2;
    }

    public void InsertInt32(int index, int n)
    {
        MoveBytes(index, 4);

        BBuffer[index + 0] = (byte)((n >> 24) & 0xff);
        BBuffer[index + 1] = (byte)((n >> 16) & 0xff);
        BBuffer[index + 2] = (byte)((n >> 8) & 0xff);
        BBuffer[index + 3] = (byte)(n & 0xff);
    }

    public void MoveBytes(int index, int count)
    {
        if (count > 0)
        {
            CheckCapacity(count);

            for (int i = index; i < index + count; i++)
            {
                int j = i;
                byte temp = BBuffer[j];
                byte exchange = 0;
                while (j < Count)
                {
                    j += count;
                    exchange = BBuffer[j];
                    BBuffer[j] = temp;
                    temp = exchange;
                }
            }
        }
        else
        {
            if (index + count < 0)
            {
                throw new OutOfMemoryException("ByteBuffer Error : Index + Count < 0");
            }

            var last = Count - 1;
            for (int i = last; i > last + count; i--)
            {
                int j = i;
                byte temp = BBuffer[j];
                byte exchange = 0;
                while (j >= index)
                {
                    j += count;
                    exchange = BBuffer[j];
                    BBuffer[j] = temp;
                    temp = exchange;
                }
            }
        }

        Count += count;
    }

    public void CheckCapacity(int count)
    {
        if (Capacity - Count < count)
        {
            throw new OutOfMemoryException("ByteBuffer Error : Out of Capacity");
        }
    }

    public void RevertLastRead()
    {
        Position -= LastReadBlock;
    }
}