# User Manual

## Resources

### Detail
1.可以在根目录下，也可以在子目录里，只要名子叫Resources就可以；比如目录：/xxx/xxx/Resources 和 /Resources 是一样的，无论多少个叫Resources的文件夹都可以；

2.Resources文件夹下的资源都会被打包进.apk或者.ipa；

3.Resource.Load()：编辑时和运行时都可以通过Resource.Load来直接读取；

4.Resources.LoadAssetAtPath()：可以读取Assets目录下的任意文件夹下的资源，可以在编辑时或者编辑器运行时用，但是不能在真机上用，路径是”Assets/xx/xx.xxx”，必须是这种路径，并且要带文件的后缀名；

5.AssetDatabase.LoadAssetAtPath()：它可以读取Assets目录下的任意文件夹下的资源，它只能在编辑时用；它的路径是”Assets/xx/xx.xxx” 必须是这种路径，并且要带文件的后缀名；

6.电脑上开发的时候尽量来用 Resource.Load() 或者 Resources.LoadAssetAtPath()，假如手机上选择一部分资源要打assetbundle，一部分资源Resource.Load().那么在做.apk或者.ipa的时候，现在都是用脚本来自动化打包，在打包之前可以用AssetDatabase.MoveAsset()把已经打包成assetbundle的原始文件从Resources文件夹下移动出去在打包，这样打出来的运行包就不会包行多余的文件了；打完包以后再把移动出去的文件夹移动回来；

7.构建项目的时候，所有的Resources目录下的文件会被合并为一个序列化文件。该文件会有自己的metadata信息和索引信息。内部用红黑树实现资源查找，用于索引相应的File GUID和Local ID，并且它还要记录在序列化文件中的偏移量。

官方的实际测试数据，一个拥有10000个assets的Resources目录，在低端移动设备上的初始化需要5-10秒甚至更长。但其实，这些assets并不会在一开始就全部用到。

8...

## AssetDatebase
### Detail
这是一个Unity在编辑器模式下的资源加载类，它提供了常规资源的 Create 、Delete、Save、Load等常用接口，并且是同步加载。所以我们只需要自己写一个资源管理类，用宏区分Editor模式，在Editor直接使用AssetDatabase进行资源加载，然后模拟一个异步回调，让它看起来跟AssetBundles 加载流程相似，然后在非Editor模式下，调用正常的AssetBundles 加载就可以了。

是一个静态类，他的作用是管理整个工程的所有文件（一般成为“资产”）。直观地说就是管理整个project窗口中的所有内容，比如，你可以增加、删除、修改文件等等；

这里有几个常常用到：
```
CreateAsset：    创建文件
CreateFolder：   创建文件夹
DeleteAsset：    删除文件
GetAssetPath：   获取文件相对于Assets所在目录的相对位置，如“Assets/Images/test.png”
LoadAssetAtPath：加载文件
Refresh：        刷新整个project窗口
SaveAssets：     保存所有文件
```
例子（实现右键点击文件或者文件夹，选择MyEditor/Delete Asset菜单后，删除选择的文件或者文件夹）：
### e.g.
```
using UnityEditor;
public class EditorCase
{
    [MenuItem("Assets/MyEditor/Delete Asset")]
    public static void DeleteAsset()
    {
        var obj = Selection.activeObject;
        var path = AssetDatabase.GetAssetPath(obj);
        AssetDatabase.DeleteAsset(path);
    }
}
```
