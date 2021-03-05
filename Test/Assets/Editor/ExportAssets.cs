using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExportAssets : MonoBehaviour
{

    [@MenuItem("Tools/Build Asset Bundles")]
    static void BuildAssetBundles()
    {
        BuildPipeline.BuildAssetBundles(Application.dataPath + "/AssetBundles", 
            BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows64);
    }
    
}
