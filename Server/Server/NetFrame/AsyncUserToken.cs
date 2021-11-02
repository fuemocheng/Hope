using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    public class AsyncUserToken
    {
        /// <summary>
        /// Socket 套接字 连接
        /// </summary>
        public Socket UserSocket { get; set; }

        /// 异步接受网络数据的对象
        public SocketAsyncEventArgs RecvSAEA { get; set; }
        /// 异步发送网络数据的对象
        public SocketAsyncEventArgs SendSAEA { get; set; }

        public LengthEncode lengthEncode;
        public LengthDecode lengthDecode;
        public ObjectEncode messageEncode;
        public ObjectDecode messageDecode;

        private bool m_bIsReading = false;
        private bool m_bIsWriting = false;

        /// 接受消息的缓存
        private List<byte> cache = new List<byte>();
        /// 发送消息的队列
        private Queue<byte[]> writeQueue = new Queue<byte[]>();

        public delegate void SendProcess(SocketAsyncEventArgs e);
        public SendProcess sendProcess;

        public delegate void CloseProcess(AsyncUserToken token, string error);
        public CloseProcess closeProcess;

        public AbsHandlerCenter handlerCenter;

        public AsyncUserToken()
        {
            RecvSAEA = new SocketAsyncEventArgs();
            SendSAEA = new SocketAsyncEventArgs();
            RecvSAEA.UserToken = this;
            SendSAEA.UserToken = this;

            //设置接收消息对象的缓冲区大小
            RecvSAEA.SetBuffer(new byte[1024], 0, 1024);
        }

        /// <summary>
        /// 网络消息到达
        /// </summary>
        /// <param name="buff"></param>
        public void Receive(byte[] buff)
        {
            //消息写入缓存
            cache.AddRange(buff);
            if (!m_bIsReading)
            {
                m_bIsReading = true;
                OnHandle();
            }
        }

        /// <summary>
        /// 缓存中有数据进行处理
        /// </summary>
        private void OnHandle()
        {
            //解码消息存储对象
            byte[] buff = null;

            //当粘包解码器存在的时候，进行粘包处理
            if (null != lengthDecode)
            {
                buff = lengthDecode(ref cache);

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
                if (cache.Count == 0)
                {
                    m_bIsReading = false;
                    return;
                }
                buff = cache.ToArray();
                cache.Clear();
            }

            //反序列化方法是否存在 此方法必须存在
            if (null == messageDecode) { throw new Exception("Message decode process is null"); }

            //进行消息反序列化
            object message = messageDecode(buff);

            //TODO: 通知应用层，处理消息


            //尾递归 防止在消息存储过程中 有其他消息到达而没有经过处理
            OnHandle();
        }


        public void Write(byte[] value)
        {
            if (null == UserSocket)
            {
                //此连接已经断开
                closeProcess(this, "Error: 调用已断开的Socket连接");
                return;
            }
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
            //取出第一条代发消息
            byte[] buff = writeQueue.Dequeue();
            //设置消息发送异步对象的发送数据缓冲区数据
            SendSAEA.SetBuffer(buff, 0, buff.Length);
            //开启异步发送
            bool willRaiseEvent = UserSocket.SendAsync(SendSAEA);
            //是否挂起
            if (!willRaiseEvent)
            {
                sendProcess(SendSAEA);
            }
        }

        public void SendCallback()
        {
            //与OnHandle尾递归相同
            OnWrite();
        }

        public void Close()
        {
            try
            {
                writeQueue.Clear();
                cache.Clear();
                m_bIsReading = false;
                m_bIsWriting = false;
                UserSocket.Shutdown(SocketShutdown.Both);
                UserSocket.Close();
                UserSocket = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
