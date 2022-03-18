# adb 真机连接Untiy
1.Unity打包，选择Development Build
  Unity 包名 PackageName: com.companyname.productname
  安装到手机，手机打开USB调试
2.ADB连接Android设备
  [https://pan.baidu.com/s/1mgGkNZM] 下载adb工具
  参见: [https://blog.csdn.net/weixin_38061311/article/details/100920687]
  
  在工具包地址打开cmd, 
  输入:
  adb forward tcp:34999 localabstract:Unity-com.companyname.productname
  等待连接成功，注意端口号有没有被占用
	Unity5.5： ADB@127.0.0.1:54999
	Unity5.6： ADB@127.0.0.1:34999
  
3.手机运行游戏
  UnityEditor Console栏选择对应的设备
  
Profiler使用
[https://blog.csdn.net/xingqing_myz/article/details/75839649]