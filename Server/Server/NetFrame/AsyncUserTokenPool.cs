using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    class AsyncUserTokenPool
    {
        private Stack<AsyncUserToken> m_stackAUTPool;

        public AsyncUserTokenPool(int maxClient)
        {
            m_stackAUTPool = new Stack<AsyncUserToken>(maxClient);
        }

        /// <summary>
        /// 取出一个连接对象
        /// </summary>
        /// <returns></returns>
        public AsyncUserToken Pop()
        {
            return m_stackAUTPool.Pop();
        }

        /// <summary>
        /// 放入一个连接对象
        /// </summary>
        /// <param name="token"></param>
        public void Push(AsyncUserToken token)
        {
            if (null != token)
            {
                m_stackAUTPool.Push(token);
            }
        }

        public int Size
        {
            get { return m_stackAUTPool.Count; }
        }
    }
}
