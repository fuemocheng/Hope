# User Manual

## Resources

### Detail
1.可以在根目录下，也可以在子目录里，只要名子叫Resources就可以；比如目录：/xxx/xxx/Resources 和 /Resources 是一样的，无论多少个叫Resources的文件夹都可以；

2.Resources文件夹下的资源都会被打包进.apk或者.ipa；

3.Resource.Load()：编辑时和运行时都可以通过Resource.Load来直接读取；

4.Resources.LoadAssetAtPath()：可以读取Assets目录下的任意文件夹下的资源，可以在编辑时或者编辑器运行时用，但是不能在真机上用，路径是”Assets/xx/xx.xxx”，必须是这种路径，并且要带文件的后缀名；

5.AssetDatabase.LoadAssetAtPath()：它可以读取Assets目录下的任意文件夹下的资源，它只能在编辑时用；它的路径是”Assets/xx/xx.xxx” 必须是这种路径，并且要带文件的后缀名；

6.电脑上开发的时候尽量来用 Resource.Load() 或者 Resources.LoadAssetAtPath()，假如手机上选择一部分资源要打assetbundle，一部分资源Resource.Load().那么在做.apk或者.ipa的时候，现在都是用脚本来自动化打包，在打包之前可以用AssetDatabase.MoveAsset()把已经打包成assetbundle的原始文件从Resources文件夹下移动出去在打包，这样打出来的运行包就不会包行多余的文件了；打完包以后再把移动出去的文件夹移动回来；


### e.g.
