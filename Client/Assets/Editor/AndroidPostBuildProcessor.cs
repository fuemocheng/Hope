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
        writer.WriteLine("org.gradle.jvmargs=-Xmx4096M"); // 这句是unity自己的
        // 参考 https://developer.android.com/jetpack/androidx
        writer.WriteLine("android.useAndroidX=true"); // 加上这句才能用androidx
        writer.WriteLine("android.enableJetifier=true"); // 加上这句，Android插件会重写其二进制文件，自动迁移现有的第三方库以使用 AndroidX
        // writer.WriteLine("android.enableR8=true");
        writer.WriteLine("unityStreamingAssets=.unity3d, google-services-desktop.json, google-services.json, GoogleService-Info.plist");
        writer.Flush();
        writer.Close();

    }
}