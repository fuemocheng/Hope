# Server

## 编译Protobuf 生成Google.Protobuf.dll

解决方案目录
```
Config/Protobuf/src/protobuf-csharp-3.18.1/csharp/src/Google.Protobuf.sln
```
生成dll目录
```
Config/Protobuf/src/protobuf-csharp-3.18.1/csharp/src/Google.Protobuf/bin/Debug/net45/Google.Protobuf.dll
```

## 打开Server解决方案
Server/Server/Server.sln

### GameProtocol.csproj添加引用
添加上述生成的 Google.Protobuf.dll，路径上述所示

重新生成 GameProtocol.csproj

### NetFrame.csproj添加引用
添加上述生成的 Google.Protobuf.dll，路径上述所示

添加GameProtocol.csproj生成的GameProtocol.dll，路径为
```
Server/Server/GameProtocol/bin/Debug/GameProtocol.dll
```

重新生成 NetFrame.csproj

### Server.csproj添加引用
添加Google.Protobuf.dll，路径如上述所示；

添加GameProtocol.dll，路径在GameProtocol项目的Debug文件下；

添加NetFrame.dll，路径在NetFrame项目的Debug文件夹下；

重新生成 Server.csproj；


## 运行
Server项目设为启动项，即可启动
