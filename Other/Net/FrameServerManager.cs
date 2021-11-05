using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//客户端主机模式，帧同步服务器
public class FrameServerManager : NetServerManager
{
    const float deltaTime = 0.01f;
    private List<NetFrameInput> frameInputList;
    private List<NetFrameInput> frameInputPlayerList;
    private ulong frameId;

    public new static NetServerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new NetServerManager();
                _instance.Init();
            }
            else if (!(_instance is NetServerManager))
            {
                var last = _instance;
                _instance = new NetServerManager();
                _instance.Init();
                (_instance as NetServerManager).InheritFrom(last);

                last.DoDestroy();
            }
            return _instance as NetServerManager;
        }
    }

    public override void Init()
    {
        base.Init();

        frameInputList = new List<NetFrameInput>();
        frameInputPlayerList = new List<NetFrameInput>();
        frameId = 0;
    }

    public void SetFrameServerInfo()
    {
        //todo
    }

    public override void DoUpdate()
    {
        PluginUtilities.ProfilerBegin("FrameServerManager.DoUpdate");
        base.DoUpdate();

        if (this.ElapseTime(deltaTime))
        {
            frameInputList.Clear();
            foreach (var server in servers.Values)
            {
                var list = server.receivePacketList;
                frameInputPlayerList.Clear();

                for (int i = 0; i < list.Count; i++)
                {
                    var frame = NetFrame.decoder(list[i].data);
                    for (int j = 0; j < frame.inputDatas.Length; j++)
                    {
                        frameInputPlayerList.Add(frame.inputDatas[j]);
                    }
                    frameInputPlayerList.Sort((NetFrameInput a, NetFrameInput b) =>
                    {
                        return a.index - b.index;
                    });
                    frameInputList.AddRange(frameInputPlayerList);

                    LogUtils.Log("Net server recv frame, input =", frame.inputDatas);
                }

                list.Clear();
            }

            NetFrameNotify notify = new NetFrameNotify();
            notify.frameId = frameId;
            notify.inputDatas = frameInputList.ToArray();

            Notify(notify);

            frameId += 1;
        }
        PluginUtilities.ProfilerEnd();
    }
}
