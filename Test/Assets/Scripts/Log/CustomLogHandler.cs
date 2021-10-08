using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CustomLogHandler : ILogHandler
{
    private FileStream m_FileStream;
    private StreamWriter m_StreamWriter;

    private ILogHandler m_DefaultLogHandler = Debug.unityLogger.logHandler;

    public bool EnableLog = true;

    public CustomLogHandler()
    {

#if DISABLED_LOG
        EnableLog = false;
#endif
        string filePath = Application.persistentDataPath + "/MyLogs.txt";

        m_FileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        m_StreamWriter = new StreamWriter(m_FileStream);

        // Replace the default debug log handler
        Debug.unityLogger.logHandler = this;
    }

    void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
    {
        if (!EnableLog)
            return;

        m_DefaultLogHandler.LogException(exception, context);
    }

    void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        if (!EnableLog)
            return;
        
        m_StreamWriter.WriteLine(String.Format(format, args));
        m_StreamWriter.Flush();

        m_DefaultLogHandler.LogFormat(logType, context, format, args);
    }

}
