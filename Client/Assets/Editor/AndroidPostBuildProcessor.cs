using System.IO;
using UnityEditor.Android;

public class AndroidPostBuildProcessor : IPostGenerateGradleAndroidProject
{
    public int callbackOrder
    {
        get
        {
            return 999;
        }
    }


    void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path)
    {
        UnityEngine.Debug.Log("AndroidPostBuildProcessor: gradle.properties Bulid path : " + path);
        path = path.Replace("\\unityLibrary", "");
        UnityEngine.Debug.Log("AndroidPostBuildProcessor: gradle.properties Bulid path-2 : " + path);
        string gradlePropertiesFile = path + "/gradle.properties";
        if (File.Exists(gradlePropertiesFile))
        {
            UnityEngine.Debug.Log("AndroidPostBuildProcessor: delete : " + gradlePropertiesFile);
            File.Delete(gradlePropertiesFile);
        }
        StreamWriter writer = File.CreateText(gradlePropertiesFile);
        writer.WriteLine("org.gradle.jvmargs=-Xmx4096M"); // �����unity�Լ���
        // �ο� https://developer.android.com/jetpack/androidx
        writer.WriteLine("android.useAndroidX=true"); // ������������androidx
        writer.WriteLine("android.enableJetifier=true"); // ������䣬Android�������д��������ļ����Զ�Ǩ�����еĵ���������ʹ�� AndroidX
        // writer.WriteLine("android.enableR8=true");
        writer.WriteLine("unityStreamingAssets=.unity3d, google-services-desktop.json, google-services.json, GoogleService-Info.plist");
        writer.Flush();
        writer.Close();

    }
}