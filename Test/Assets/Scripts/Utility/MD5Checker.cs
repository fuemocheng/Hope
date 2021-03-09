using System;
using System.IO;
using System.Security.Cryptography;

public class MD5Checker
{
    //�첽check�ص�
    public delegate void AsyncCheckHandler(AsyncCheckEventArgs e);
    public event AsyncCheckHandler AsyncCheckProgress;

    //֧�����й�ϣ�㷨
    private HashAlgorithm hashAlgorithm;
    //�ļ���ȡ��
    private Stream inputStream;
    //����
    private byte[] asyncBuffer;

    public AsyncCheckState CompleteState { get; private set; }
    public float Progress { get; private set; }
    public string GetMD5 { get; private set; }

    public static string Check(string path)
    {
        try
        {
            var fs = new FileStream(path, FileMode.Open);
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
            byte[] buffer = md5Provider.ComputeHash(fs);
            string md5 = BitConverter.ToString(buffer);
            md5 = md5.Replace("-", "");
            fs.Close();
            return md5;
        }
        catch (ArgumentException aex)
        {
            throw new ArgumentException(string.Format("<{0}>, ������: {1}", path, aex.Message));
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format("��ȡ�ļ� {0} , MD5ʧ��: {1}", path, ex.Message));
        }
    }

    public static string Check_Stream(string path)
    {
        try
        {
            int bufferSize = 1024 * 256;//�Զ��建������С256K
            var buffer = new byte[bufferSize];
            Stream inputStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
            int readLength = 0;//ÿ�ζ�ȡ����
            var output = new byte[bufferSize];
            while ((readLength = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                //����MD5
                hashAlgorithm.TransformBlock(buffer, 0, readLength, output, 0);
            }
            //��������㣬�������(������һ��ѭ���Ѿ�����������㣬���Ե��ô˷���ʱ���������������Ϊ0)
            hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
            string md5 = BitConverter.ToString(hashAlgorithm.Hash);
            hashAlgorithm.Clear();
            inputStream.Close();
            md5 = md5.Replace("-", "");
            return md5;
        }
        catch (ArgumentException aex)
        {
            throw new ArgumentException(string.Format("<{0}>, ������: {1}", path, aex.Message));
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format("��ȡ�ļ� {0} , MD5ʧ��: {1}", path, ex.Message));
        }
    }

    public void AsyncCheck(string path)
    {
        CompleteState = AsyncCheckState.Checking;
        try
        {
            int bufferSize = 1024 * 256;//��������С��1MB 1048576

            asyncBuffer = new byte[bufferSize];

            //���ļ���
            inputStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize, true);
            hashAlgorithm = new MD5CryptoServiceProvider();

            //�첽��ȡ���ݵ�������
            inputStream.BeginRead(asyncBuffer, 0, asyncBuffer.Length, new AsyncCallback(AsyncComputeHashCallback), null);
        }
        catch (ArgumentException aex)
        {
            throw new ArgumentException(string.Format("<{0}>, ������: {1}", path, aex.Message));
        }
        catch (Exception ex)
        {
            throw new Exception(string.Format("��ȡ�ļ�{0} ,MD5ʧ��: {1}", path, ex.Message));
        }
    }

    private void AsyncComputeHashCallback(IAsyncResult result)
    {
        int bytesRead = inputStream.EndRead(result);
        //����Ƿ񵽴���ĩβ
        if (inputStream.Position < inputStream.Length)
        {
            //�������
            Progress = (float)inputStream.Position / inputStream.Length;
            string pro = string.Format("{0:P0}", Progress);

            if (null != AsyncCheckProgress)
                AsyncCheckProgress(new AsyncCheckEventArgs(AsyncCheckState.Checking, pro));

            var output = new byte[asyncBuffer.Length];
            //�ֿ�����ϣֵ
            hashAlgorithm.TransformBlock(asyncBuffer, 0, asyncBuffer.Length, output, 0);

            //�첽��ȡ��һ�ֿ�
            inputStream.BeginRead(asyncBuffer, 0, asyncBuffer.Length, new AsyncCallback(AsyncComputeHashCallback), null);
            return;
        }
        else
        {
            //�������ֿ��ϣֵ
            hashAlgorithm.TransformFinalBlock(asyncBuffer, 0, bytesRead);
        }

        Progress = 1;
        string md5 = BitConverter.ToString(hashAlgorithm.Hash).Replace("-", "");
        CompleteState = AsyncCheckState.Completed;
        GetMD5 = md5;
        if (null != AsyncCheckProgress)
            AsyncCheckProgress(new AsyncCheckEventArgs(AsyncCheckState.Completed, GetMD5));

        inputStream.Close();
    }
}

public enum AsyncCheckState
{
    Completed,
    Checking
}

public class AsyncCheckEventArgs : EventArgs
{
    public string Value { get; private set; }

    public AsyncCheckState State { get; private set; }

    public AsyncCheckEventArgs(AsyncCheckState state, string value)
    {
        this.Value = value; this.State = state;
    }
}
