# User Manual

## StreamingAssets

### Detail
1.StreamingAssets文件夹下的资源也会全都打包在.apk或者.ipa；

2.StreamingAssets和Resources的区别是，Resources会压缩文件，但是StreamingAssets不会压缩，原封不动的打包进去；并且它是一个只读的文件夹，程序运行时只能读不能写；

3.StreamingAssets在各个平台下的路径是不同的，可以用Application.streamingAssetsPath，会根据当前的平台选择对应的路径；

4.Application.persistentDataPath目录是应用程序的沙盒目录，所以打包之前是没有这个目录的，直到应用程序在手机上安装完毕才有这个目录；

### e.g.
