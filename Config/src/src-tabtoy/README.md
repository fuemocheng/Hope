出现timeout的情况 控制台执行下列命令

go env -w GOPROXY=https://goproxy.io,direct

如果设置了该变量，下载源代码时将会通过这个环境变量设置的代理地址，而不再是以前的直接从代码库下载

详情参考https://goproxy.io/zh/

v1.13及以上版本执行
go env -w GO111MODULE=on
go env -w GOPROXY=https://goproxy.io,direct