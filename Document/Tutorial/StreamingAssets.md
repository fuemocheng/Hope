# User Manual

## StreamingAssets

### Detail
1.StreamingAssets文件夹下的资源也会全都打包在.apk或者.ipa；

2.StreamingAssets和Resources的区别是，Resources会压缩文件，但是StreamingAssets不会压缩，原封不动的打包进去；并且它是一个只读的文件夹，程序运行时只能读不能写；

3.StreamingAssets在各个平台下的路径是不同的，可以用Application.streamingAssetsPath，会根据当前的平台选择对应的路径；

4.Application.persistentDataPath目录是应用程序的沙盒目录，所以打包之前是没有这个目录的，直到应用程序在手机上安装完毕才有这个目录；

5.Resources,StreamingAssets,persistentDataPath区别

(1). 在项目根目录中创建Resources文件夹来保存文件。
可以使用Resources.Load("文件名字，注：不包括文件后缀名");把文件夹中的对象加载出来。
注：此方可实现对文件实施“增删查改”等操作，但打包后不可以更改了。

(2). 直接放在项目根路径下来保存文件
在直接使用Application.dataPath来读取文件进行操作。
注：移动端是没有访问权限的。

(3). 在项目根目录中创建StreamingAssets文件夹来保存文件。

a.可使用Application.dataPath来读取文件进行操作。
```
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
string filepath = Application.dataPath +"/StreamingAssets"+"/my.xml";
#elif UNITY_IPHONE
 string filepath = Application.dataPath +"/Raw"+"/my.xml";
#elif UNITY_ANDROID
 string filepath = "jar:file://" + Application.dataPath + "!/assets/"+"/my.xml";
#endif
```

b.直接使用Application.streamingAssetsPath来读取文件进行操作。
注：此方法在pc/Mac电脑中可实现对文件实施“增删查改”等操作，但在移动端只支持读取操作。

(4). 使用Application.persistentDataPath来操作文件（荐）
该文件存在手机沙盒中，因为不能直接存放文件，
a.通过服务器直接下载保存到该位置，也可以通过Md5码比对下载更新新的资源
b.没有服务器的，只有间接通过文件流的方式从本地读取并写入Application.persistentDataPath文件下，然后再通过Application.persistentDataPath来读取操作。
注：在Pc/Mac电脑 以及Android跟Ipad、ipone都可对文件进行任意操作，另外在IOS上该目录下的东西可以被iCloud自动备份。


