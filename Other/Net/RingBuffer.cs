/********************************************************************
	created:	2020/11/4
	created:	2020-11-4 22:18:46
	author:		Five
	
	purpose:	RingBuffer is a circular buffer
*********************************************************************/

using System;

public sealed class TooManyDataToWriteException : Exception
{
    public TooManyDataToWriteException() : base("RingBuffer Error : too many data to write") { }
}

public sealed class IsFullException : OutOfMemoryException
{
    public IsFullException() : base("RingBuffer Error : is full") { }
}

public sealed class IsEmptyException : SystemException
{
    public IsEmptyException() : base("RingBuffer Error : is empty") { }
}

public sealed class RingBuffer
{
    private byte[] m_buf;

    private int m_size;

    private int m_readIndex;  // 读索引

    private int m_writeIndex; // 写索引

    private bool m_isFull;

    /// <summary>
    /// 开启一个缓冲区
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static RingBuffer New(int size)
    {
        var result = new RingBuffer
        {
            m_size = size,
            m_buf = new byte[size],
        };
        return result;
    }

    public int Read(byte[] p)
    {
        if (p.Length == 0)
            return 0;

        int n = 0;
        lock (this)
        {
            if (m_readIndex == m_writeIndex && m_isFull == false)
            {
                throw new IsEmptyException();
            }

            if (m_writeIndex > m_readIndex)
            {
                n = m_writeIndex - m_readIndex;
                if (n > p.Length)
                {
                    n = p.Length;
                }

                Buffer.BlockCopy(m_buf, m_readIndex, p, 0, n);
                m_readIndex = (m_readIndex + n) % m_size;
                return n;
            }

            n = m_size - m_readIndex + m_writeIndex;
            if (n > p.Length)
            {
                n = p.Length;
            }

            if (m_readIndex + n < m_size)
            {
                Buffer.BlockCopy(m_buf, m_readIndex, p, 0, n);
            }
            else
            {
                var c1 = m_size - m_readIndex;
                Buffer.BlockCopy(m_buf, m_readIndex, p, 0, m_size);
                var c2 = n - c1;
                Buffer.BlockCopy(m_buf, 0, p, c1, c2);
            }

            m_readIndex = (m_readIndex + n) % m_size;

            m_isFull = false;
        }

        return n;
    }

    public byte ReadByte()
    {
        lock (this)
        {
            if (m_writeIndex == m_readIndex && m_isFull == false)
            {
                throw new IsEmptyException();
            }

            var buf = m_buf[m_readIndex];
            m_readIndex++;
            if (m_readIndex == m_size)
                m_readIndex = 0;

            m_isFull = false;
            return buf;
        }
    }

    public int Write(byte[] p)
    {
        if (p.Length == 0)
            return 0;

        lock (this)
        {
            if (m_isFull)
            {
                throw new IsFullException();
            }

            int n = 0;
            int avail;
            if (m_writeIndex > m_readIndex)
            {
                avail = m_size - m_writeIndex + m_readIndex;
            }
            else
            {
                avail = m_readIndex - m_writeIndex;
            }

            n = p.Length;

            if (m_writeIndex >= m_readIndex)
            {
                var c1 = m_size - m_writeIndex;
                if (c1 >= n)
                {
                    Buffer.BlockCopy(p, 0, m_buf, m_writeIndex, n);
                    m_writeIndex += n;
                }
                else
                {
                    Buffer.BlockCopy(p, 0, m_buf, m_writeIndex, c1);
                    var c2 = n - c1;
                    Buffer.BlockCopy(p, c1, m_buf, 0, c2);
                    m_writeIndex = c2;
                }
            }
            else
            {
                Buffer.BlockCopy(p, 0, m_buf, m_writeIndex, n);
                m_writeIndex += n;
            }

            if (m_writeIndex == m_size)
            {
                m_writeIndex = 0;
            }
            if (m_writeIndex == m_readIndex)
            {
                m_isFull = true;
            }

            return n;
        }
    }

    public void WriteByte(byte b)
    {
        lock (this)
        {
            if (m_writeIndex == m_readIndex && m_isFull)
            {
                throw new IsFullException();
            }

            m_buf[m_writeIndex] = b;
            m_readIndex++;

            if (m_writeIndex == m_size)
            {
                m_writeIndex = 0;
            }

            if (m_writeIndex == m_readIndex)
            {
                m_isFull = true;
            }
        }
    }

    public int Length()
    {
        lock (this)
        {
            if (m_readIndex == m_writeIndex)
            {
                if (m_isFull)
                {
                    return m_size;
                }
                return 0;
            }

            if (m_writeIndex > m_readIndex)
            {
                return m_writeIndex - m_readIndex;
            }

            return m_size - m_readIndex + m_writeIndex;
        }
    }

    public int Capacity()
    {
        return m_size;
    }

    public int FreeSize()
    {
        lock (this)
        {
            if (m_readIndex == m_writeIndex)
            {
                if (m_isFull)
                {
                    return m_size;
                }
                return 0;
            }

            if (m_writeIndex < m_readIndex)
            {
                return m_readIndex - m_writeIndex;
            }

            return m_size - m_writeIndex + m_readIndex;
        }
    }

    public int ReadInt16()
    {
        int a = ReadByte();
        int b = ReadByte();
        return (a << 8) + b;
    }

    byte[] m_intData = new byte[4];
    public int ReadInt32()
    {
        Read(m_intData);
        return m_intData[0] | m_intData[1] << 8 | m_intData[2] << 16 | m_intData[3] << 24;
    }

    public byte[] ReadBytes(int count)
    {
        var buf = new byte[count];
        Read(buf);
        return buf;
    }

}
