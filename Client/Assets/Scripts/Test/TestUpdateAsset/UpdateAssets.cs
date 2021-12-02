using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAssets : MonoBehaviour
{
    //保存本地的assetbundle名和对应的MD5值
    private Dictionary<string, string> LocalResVerison;
    private Dictionary<string, string> ServerResVersion;

    //需要更新的assetbundle名
    private List<string> NeedDownFiles;

    private bool NeedUpdateLocalVersionFile = false;

    private string _localUrl;
    private string _serverUrl;

    public IEnumerator OnStart()
    {
        yield return StartCoroutine(Init());
    }

    public IEnumerator Init()
    {
        LocalResVerison = new Dictionary<string, string>();
        ServerResVersion = new Dictionary<string, string>();
        NeedDownFiles = new List<string>();

        //加载本地配置version文件
        _localUrl = PathConfig.localUrl + "/" + PathConfig.GetManifestFileName() + "/";

        yield return null;

    }
}
