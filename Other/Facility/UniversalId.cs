using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using UnityEngine;
using System.Diagnostics;
using System;

sealed class UniversalId
{
    // Maximal value for 64bit systems is 2^22.  See man 5 proc.
    // See https://github.com/netty/netty/issues/2706
    const int MaxProcessId = 4194304;
    const int SequenceLen = 4;
    const int TimestampLen = 8;
    const int RandomLen = 4;
    static int nextSequence;
    static int seed = (int)(Stopwatch.GetTimestamp() & 0xFFFFFFFF); //used to safly cast long to int, because the timestamp returned is long and it doesn't fit into an int
    readonly byte[] data = new byte[SequenceLen + TimestampLen + RandomLen];
    int hashCode;

    double shortValue;

    static UniversalId()
    {
    }

    public static UniversalId NewInstance()
    {
        var id = new UniversalId();
        id.Init();
        return id;
    }

    public double AsShortText()
    {
        double asShortText = this.shortValue;
        if (asShortText == 0)
        {
            this.shortValue = asShortText = BitConverter.ToDouble(this.data, 0);
        }

        return asShortText;
    }


    void Init()
    {
        int i = 0;

        // sequence
        i = this.WriteInt(i, Interlocked.Increment(ref nextSequence));

        // timestamp (kind of)
        long ticks = Stopwatch.GetTimestamp();
        long nanos = (ticks / Stopwatch.Frequency) * 1000000000;
        long millis = (ticks / Stopwatch.Frequency) * 1000;
        i = this.WriteLong(i, SwapLong(nanos) ^ millis);

        // random
        int random = UnityEngine.Random.Range(0, Interlocked.Increment(ref seed));
        this.hashCode = random;
        i = this.WriteInt(i, random);
    }

    int WriteInt(int i, int value)
    {
        uint val = (uint)value;
        this.data[i++] = (byte)(val >> 24);
        this.data[i++] = (byte)(val >> 16);
        this.data[i++] = (byte)(val >> 8);
        this.data[i++] = (byte)value;
        return i;
    }

    int WriteLong(int i, long value)
    {
        ulong val = (ulong)value;
        this.data[i++] = (byte)(val >> 56);
        this.data[i++] = (byte)(val >> 48);
        this.data[i++] = (byte)(val >> 40);
        this.data[i++] = (byte)(val >> 32);
        this.data[i++] = (byte)(val >> 24);
        this.data[i++] = (byte)(val >> 16);
        this.data[i++] = (byte)(val >> 8);
        this.data[i++] = (byte)value;
        return i;
    }


    /// <summary>
    ///     Toggles the endianness of the specified 64-bit long integer.
    /// </summary>
    public static long SwapLong(long value)
        => ((SwapInt((int)value) & 0xFFFFFFFF) << 32)
            | (SwapInt((int)(value >> 32)) & 0xFFFFFFFF);

    /// <summary>
    ///     Toggles the endianness of the specified 32-bit integer.
    /// </summary>
    public static int SwapInt(int value)
        => ((SwapShort((short)value) & 0xFFFF) << 16)
            | (SwapShort((short)(value >> 16)) & 0xFFFF);

    /// <summary>
    ///     Toggles the endianness of the specified 16-bit integer.
    /// </summary>
    public static short SwapShort(short value) => (short)(((value & 0xFF) << 8) | (value >> 8) & 0xFF);


    static readonly char[] HexdumpTable = new char[256 * 4];
}