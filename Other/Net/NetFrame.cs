using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetFrame : AbstractProto<NetFrame>
{
    public ulong frameId;
    public NetFrameInput[] inputDatas;
}

public class NetFrameNotify : AbstractProto<NetFrameNotify>
{
    public ulong frameId;
    public NetFrameInput[] inputDatas;
}

public class NetFrameInput
{
    public int index;
    public byte[] data;
}