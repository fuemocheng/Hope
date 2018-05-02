using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Platform:   UNITY_EDITOR_WIN
/// Application.dataPath:               F:/ProjectName/Assets
/// Application.streamingAssetsPath:    F:/ProjectName/Assets/StreamingAssets
/// Application.persistentDataPath:     C:/Users/username/AppData/LocalLow/company name/product name
/// Application.temporaryCachePath:     C:/Users/username/AppData/Local/Temp/company name/product name
/// 
/// Platform:   STANDALONE_WIN          F:/Export/GameRoot/AppName.exe
/// Application.dataPath:               F:/Export/GameRoot/AppName_Data
/// Application.streamingAssetsPath:    F:/Export/GameRoot/AppName_Data/StreamingAssets
/// Application.persistentDataPath:     C:/Users/username/AppData/LocalLow/company name/product name
/// Application.temporaryCachePath:     C:/Users/username/AppData/Local/Temp/company name/product name
/// 
/// Platform:   UNITY_ANDROID
/// Application.dataPath:               /data/app/package name-1.apk
/// Application.streamingAssetsPath:    jar:file:///data/app/package name-1.apk!/assets
/// Application.persistentDataPath:     /storage/sdcard/Android/data/package name/files
/// Application.temporaryCachePath:     /storage/sdcard/Android/data/package name/cache
///
/// Platform:   UNITY_EDITOR_OSX
/// Application.dataPath:               /data/app/package name-1.apk
/// Application.streamingAssetsPath:    jar:file:///data/app/package name-1.apk!/assets
/// Application.persistentDataPath:     /storage/sdcard/Android/data/package name/files
/// Application.temporaryCachePath:     /storage/sdcard/Android/data/package name/cache
///
/// Platform:   UNITY_IOS
/// Application.dataPath:               /data/app/package name-1.apk
/// Application.streamingAssetsPath:    jar:file:///data/app/package name-1.apk!/assets
/// Application.persistentDataPath:     /storage/sdcard/Android/data/package name/files
/// Application.temporaryCachePath:     /storage/sdcard/Android/data/package name/cache
/// 
/// </summary>
public class PlatformPath : MonoBehaviour
{

    void Start()
    {
        PrintPlatformPath();
        CheckPlatform();
    }

    void Update()
    {

    }

    private void PrintPlatformPath()
    {

#if UNITY_EDITOR_WIN
        Debug.Log("Platform:" + "UNITY_EDITOR_WIN");
#elif UNITY_EDITOR_OSX
        Debug.Log("Platform:" + "UNITY_EDITOR_OSX");
#elif UNITY_STANDALONE_WIN
        Debug.Log("Platform:" + "UNITY_STANDALONE_WIN");
#elif UNITY_STANDALONE_OSX
        Debug.Log("Platform:" + "UNITY_STANDALONE_OSX");
#elif UNITY_ANDROID
        Debug.Log("Platform:" + "UNITY_ANDROID");
#elif UNITY_IOS
        Debug.Log("Platform:" + "UNITY_IOS");
#endif

        Debug.Log("Application.dataPath:" + Application.dataPath);
        Debug.Log("Application.streamingAssetsPath:" + Application.streamingAssetsPath);
        Debug.Log("Application.persistentDataPath:" + Application.persistentDataPath);
        Debug.Log("Application.temporaryCachePath:" + Application.temporaryCachePath);

    }

    /// <summary>
    /// 运行时判断平台
    /// </summary>
    private void CheckPlatform()
    {
        if (Application.platform == RuntimePlatform.Android)
        {

        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

        }
    }
}
