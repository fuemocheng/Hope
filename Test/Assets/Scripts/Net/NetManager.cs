using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using NetFrame;
using NetFrame.Coding;
using CmdProto;
using Google.Protobuf;

public class NetManager : Singleton<NetManager>
{
    public Socket m_socket;
    private IPEndPoint ipEndPoint;

    private SocketAsyncEventArgs connectSAEA;
    private SocketAsyncEventArgs recvSAEA;
    private SocketAsyncEventArgs sendSAEA;

    /// 接受消息的缓存 处理粘包问题
    private List<byte> recvCache = new List<byte>();
    /// 发送消息的队列
    private Queue<byte[]> writeQueue = new Queue<byte[]>();

    //正在读取或写入
    private bool m_bIsReading = false;
    private bool m_bIsWriting = false;

    // 粘包解码器, 消息解码器
    public LengthEncode lengthEncode;
    public LengthDecode lengthDecode;
    public ObjectEncode messageEncode;
    public ObjectDecode messageDecode;

    public NetManager()
    {
        lengthEncode = LengthEncoding.Encode;
        lengthDecode = LengthEncoding.Decode;
        messageEncode = MessageEncoding.Encode;
        messageDecode = MessageEncoding.Decode;
    }

    public void Connect(string ip, int port)
    {
        if(IsSocketValid())
        {
            Close("Close before connected");
        }
        
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAddress = IPAddress.Parse(ip);
        ipEndPoint = new IPEndPoint(ipAddress, port);
        
        connectSAEA = new SocketAsyncEventArgs();
        connectSAEA.RemoteEndPoint = ipEndPoint;
        connectSAEA.Completed += OnConnectedCompleted;

        int connectDelay = 5000;
        m_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, connectDelay);
        m_socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);

        m_socket.ConnectAsync(connectSAEA);

        //TODO: 超时重连,弱网连接等问题

    }

    private void OnConnectedCompleted(object sender, SocketAsyncEventArgs e)
    {
        if (e.SocketError != SocketError.Success)
        {
            LogUtils.LogError($"Connected failed!");
            return;
        }

        Socket socket = sender as Socket;
        string strIPRemote = socket.RemoteEndPoint.ToString();
        LogUtils.LogError($"Connect {strIPRemote} succeed!");

        //recvSAEA 初始化
        recvSAEA = new SocketAsyncEventArgs();
        recvSAEA.UserToken = socket;
        byte[] receiveBuffer = new byte[1024 * 4];
        recvSAEA.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
        recvSAEA.Completed += OnReceiveCompleted;
        recvSAEA.RemoteEndPoint = ipEndPoint;

        //sendSAEA 初始化
        sendSAEA = new SocketAsyncEventArgs();
        sendSAEA.UserToken = socket;
        sendSAEA.Completed += OnSendCompleted;

        m_socket.ReceiveAsync(recvSAEA);
    }

    #region Receive

    private void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
    {
        if (e.SocketError == SocketError.OperationAborted)
        {
            LogUtils.LogError($"OperationAborted!");
            return;
        }

        Socket socket = sender as Socket;

        //判断网络消息接收是否成功
        if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
        {
            //拷贝 recvSAEA 缓存中的数据
            byte[] message = new byte[e.BytesTransferred];
            Buffer.BlockCopy(e.Buffer, 0, message, 0, e.BytesTransferred);

            //处理消息
            Receive(message);

            //继续异步接收消息
            socket.ReceiveAsync(recvSAEA);
        }
        else if (e.SocketError == SocketError.ConnectionReset && e.BytesTransferred == 0)
        {
            LogUtils.LogError($"Server disconnected!");
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 网络消息到达, recvSAEA 缓存数据拷贝到 cache 中
    /// </summary>
    private void Receive(byte[] buff)
    {
        recvCache.AddRange(buff);
        if (!m_bIsReading)
        {
            m_bIsReading = true;
            OnReceive();
        }
    }

    /// <summary>
    /// 缓存中有数据进行处理, 先解码长度, 再解码消息
    /// </summary>
    private void OnReceive()
    {
        //解码消息存储对象
        byte[] buff = null;

        //当粘包解码器存在的时候，进行粘包处理
        if (null != lengthDecode)
        {
            buff = lengthDecode(ref recvCache);

            //消息未接收全 退出数据处理 等待下次消息到达
            if (null == buff)
            {
                m_bIsReading = false;
                return;
            }
        }
        else
        {
            //不用处理粘包
            //缓存区中没有数据 直接跳出数据处理 等待消息到达
            if (recvCache.Count == 0)
            {
                m_bIsReading = false;
                return;
            }
            buff = recvCache.ToArray();
            recvCache.Clear();
        }

        //反序列化方法是否存在 此方法必须存在
        if (null == messageDecode) { throw new Exception("Message decode process is null"); }

        //进行消息反序列化
        NetPacket message = messageDecode(buff);

        //TODO: 通知应用层，处理消息
        //handlerCenter.MessageReceive(this, message);

        //尾递归 防止在消息存储过程中 有其他消息到达而没有经过处理
        OnReceive();
    }

    #endregion

    #region Send

    /// <summary>
    /// 主动向客户端通知 或者 回客户端消息,  先编码消息, 再编码长度
    /// </summary>
    public void Send(Cmd cmd, IMessage message)
    {
        if (!IsSocketValid())
        {
            LogUtils.LogError("Socket is invalid!");
            return;
        }
        NetPacket netPacket = new NetPacket();
        netPacket.cmd = (int)cmd;
        netPacket.message = message;

        //先编码消息, 再编码长度
        byte[] byteArr = lengthEncode(messageEncode(netPacket));
        Write(byteArr);
    }

    public void Write(byte[] value)
    {
        writeQueue.Enqueue(value);
        if (!m_bIsWriting)
        {
            m_bIsWriting = true;
            OnWrite();
        }
    }

    private void OnWrite()
    {
        //判断发送队列是否有信息
        if (writeQueue.Count == 0)
        {
            m_bIsWriting = false;
            return;
        }
        //取出第一条待发消息
        byte[] buff = writeQueue.Dequeue();
        //设置消息发送异步对象的发送数据缓冲区数据
        sendSAEA.SetBuffer(buff, 0, buff.Length);
        //开启异步发送
        bool willRaiseEvent = m_socket.SendAsync(sendSAEA);
        //是否挂起
        if (!willRaiseEvent)
        {
            if (sendSAEA.SocketError != SocketError.Success)
            {
                Close(sendSAEA.SocketError.ToString());
            }
            else
            {
                //消息发送成功，回调
                OnWrite();
            }
        }
    }

    private void OnSendCompleted(object sender, SocketAsyncEventArgs e)
    {
        LogUtils.LogError("OnSendCompleted");
    }

    #endregion

    private bool IsSocketValid()
    {
        if (m_socket != null && m_socket.Connected)
        {
            return true;
        }
        return false;
    }

    private void Close(string error)
    {
        if (m_socket != null)
        {
            m_socket.Close();
            m_socket = null;
            LogUtils.LogError(error);

            //清理数据
            recvCache.Clear();
            writeQueue.Clear();
        }
    }
}
