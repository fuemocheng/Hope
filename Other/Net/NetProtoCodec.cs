using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractProto<T>
{
    public delegate byte[] EncoderDelegate(T frame);
    public delegate T DecoderDelegate(byte[] data);

    public static int protoCmd;
    public static EncoderDelegate encoder;
    public static DecoderDelegate decoder;
}
