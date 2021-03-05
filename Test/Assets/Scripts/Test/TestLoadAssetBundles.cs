using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestLoadAssetBundles : MonoBehaviour
{

    /// 动态加载资源
    /// 1.首先加载AssetBundle对象
    /// 2.从AssetBundle对象获取目标资源

    void Start()
    {
        Init();
    }

    /// <summary>
    /// 构造 WWW 对象的过程中会加载Bundle文件并返回一个WWW对象，完成后会在内存中创建较大的WebStream
    /// (解压后的内容，通常为原Bundle文件的4~5倍大小，纹理资源比例可能更大），因此后续的AssetBundle.LoadAsset可以直接在内存中进行；
    /// 优点：
    /// 1.后续的Load操作在内存中进行，相比 LoadFromCacheOrDownload 的IO操作开销更小；
    /// 2.不形成缓存文件，而 LoadFromCacheOrDownload 则需要额外的磁盘空间存放缓存；
    /// 3.能通过WWW.texture，WWW.bytes，WWW.audioClip等接口直接加载外部资源，而 LoadFromCacheOrDownload 只能用于加载AssetBundle；
    /// 劣势：
    /// 1.每次加载都涉及到解压操作，而 LoadFromCacheOrDownload 在第二次加载时就省去了解压的开销；
    /// 2.在内存中会有较大的WebStream，而 LoadFromCacheOrDownload 在内存中只有通常较小的SerializedFile；
    /// （此项为一般情况，但并不绝对，对于序列化信息较多的Prefab，很可能出现SerializedFile比WebStream更大的情况）；
    /// </summary>
    /// <returns></returns>
    IEnumerator GetData() 
    {
        WWW www = new WWW("file://" + Application.dataPath + "/AssetBundles/cube");
        yield return www;
        GameObject cube = www.assetBundle.LoadAsset<GameObject>("Cube_01");
        Instantiate(cube);
    }

    /// <summary>
    /// 该方法会将解压形式的Bundle内容存入磁盘中作为缓存（如果该Bundle已在缓存中，则省去这一步），
    /// 完成后只会在内存中创建较小的SerializedFile，而后续的AssetBundle.LoadAsset需要通过IO从磁盘中的缓存获取。
    /// 优缺点见 IEnumerator GetData()；
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadFromCacheOrDownload()
    {
        WWW www = WWW.LoadFromCacheOrDownload("file://" + Application.dataPath + "/AssetBundles/cube", new Hash128(1,1,1,1));
        yield return www;
        GameObject cube = www.assetBundle.LoadAsset<GameObject>("Cube_01");
        Instantiate(cube);
    }


    /// <summary>
    /// AssetBundle.LoadFromFile("")
    /// 
    /// 这个 API 在加载本地存储的未压缩 AssetBundle 时具有很高效率。
    /// 如果 AssetBundle 是未压缩，或者是数据块形式（LZ4 算法压缩）的，LoadFromFile 将从磁盘中直接加载它。
    /// 如果 AssetBundle 是高度压缩（LZMA 算法压缩）的，在将它加载进入内存前，会首先将它解压。
    /// 
    /// </summary>
    void TestLoadFromFile()
    {
        AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles/cube"));
        if(assetBundle == null)
        {
            Debug.LogError("cubebundle is null!");
            return;
        }
        var cube = assetBundle.LoadAsset<GameObject>("Cube_01");
        Instantiate(cube);
    }


    /// <summary>
    /// AssetBundle.LoadFromMemoryAsync();
    /// 
    /// 此函数采用包含 AssetBundle 数据的字节数组。也可以根据需要传递 CRC 值。
    /// 如果 AssetBundle 采用的是 LZMA 压缩方式，将在加载时解压缩 AssetBundle。LZ4 压缩包则会以压缩状态加载。
    /// 
    /// 主要用于对数据的加解密上。
    /// 
    /// 通过Bundle的二进制数据，异步创建AssetBundle对象。完成后会在内存中创建较大的WebStream。
    /// 调用时，Bundle的解压是异步进行的，因此对于未压缩的Bundle文件，该接口与LoadFromMemory()等价。
    /// 
    /// </summary>
    IEnumerator LoadFromMemoryAsync()
    {
        byte[] binary = File.ReadAllBytes(Path.Combine(Application.dataPath, "AssetBundles/cube"));
        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(binary);
        yield return createRequest;
        var cube = createRequest.assetBundle.LoadAsset<GameObject>("Cube_01");
        Instantiate(cube);
    }

    /// <summary>
    /// 从网络加载
    /// </summary>
    /// <returns></returns>
    IEnumerator DownLoadFromServer()
    {
        WWW www = new WWW("http://127.0.0.1:8080/cube");
        yield return www;

        Texture cubeTex_02 = www.assetBundle.LoadAsset<Texture>("cubeTex_02");
        Material material = www.assetBundle.LoadAsset<Material>("cubeMat_01");
        material.mainTexture = cubeTex_02;
        GameObject cube = www.assetBundle.LoadAsset<GameObject>("Cube_01");
        GameObject cubeClone = Instantiate(cube);

        www.assetBundle.Unload(false);
    }


    IEnumerator TestUnityWebRequest()
    {

        yield return null;
    }

    void Init()
    {
        /// 1.WWW GetData
        /// StartCoroutine(GetData());

        /// 2.WWW.LoadFromCacheOrDownload
        /// StartCoroutine(LoadFromCacheOrDownload());

        /// 3.AssetBundle.LoadFromFile
        /// TestLoadFromFile();

        /// 4.AssetBundle.LoadFromMemoryAsync
        /// StartCoroutine(LoadFromMemoryAsync());

        /// 5.DownLoadFromServer
        StartCoroutine(DownLoadFromServer());


    }

}
