using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 以下为绝对路径 Absolute Path
///
/// Platform:   UNITY_EDITOR_WIN
/// Application.dataPath:               F:/ProjectName/Assets
/// Application.streamingAssetsPath:    F:/ProjectName/Assets/StreamingAssets
/// Application.persistentDataPath:     C:/Users/username/AppData/LocalLow/CompanyName/ProductName
/// Application.temporaryCachePath:     C:/Users/username/AppData/Local/Temp/CompanyName/ProductName
/// 
/// Platform:   STANDALONE_WIN          F:/Export/GameRoot/AppName.exe      //应用所在路径
/// Application.dataPath:               F:/Export/GameRoot/AppName_Data
/// Application.streamingAssetsPath:    F:/Export/GameRoot/AppName_Data/StreamingAssets
/// Application.persistentDataPath:     C:/Users/username/AppData/LocalLow/CompanyName/ProductName
/// Application.temporaryCachePath:     C:/Users/username/AppData/Local/Temp/CompanyName/ProductName
/// 
/// Platform:   UNITY_ANDROID
/// Application.dataPath:               /data/app/com.Company.ProductName-1.apk
/// Application.streamingAssetsPath:    jar:file:///data/app/com.Company.ProductName-1.apk!/assets
/// Application.persistentDataPath:     /storage/sdcard/Android/data/com.Company.ProductName/files
/// Application.temporaryCachePath:     /storage/sdcard/Android/data/com.Company.ProductName/cache
///
/// Platform:   UNITY_EDITOR_OSX
/// Application.dataPath:               /Users/username/Documents/ProjectName/Assets
/// Application.streamingAssetsPath:    /Users/username/Documents/ProjectName/Assets/StreamingAssets
/// Application.persistentDataPath:     /Users/username/Library/Application Support/CompanyName/ProductName
/// Application.temporaryCachePath:     /var/folders/xx/xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/T/CompanyName/ProductName
///
/// Platform:   UNITY_IOS
/// Application.dataPath:               /var/containers/Bundle/Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/ProductName.app/Data
/// Application.streamingAssetsPath:    /var/containers/Bundle/Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/ProductName.app/Data/Raw
/// Application.persistentDataPath:     /var/containers/Bundle/Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Documents
/// Application.temporaryCachePath:     /var/containers/Bundle/Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Library/Caches
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
