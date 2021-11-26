# User Manual

## Gizmos

### Detail
1.这个目录辅助在Unity的Scene视窗里显示一些辅助标线，图标或者其他的标识，用来辅助开发定位资源；

2.OnDrawGizmos()方法，只要脚本继承了MonoBehaviour后，并且在编辑模式下就会每一帧都执行它；

3.它和Editor文件夹一样都不会被打到最终发布包里，仅仅用于开发时使用；

### e.g.

1.如下代码所示它可以在Scene视图里给某个坐标绘制一个icon。它的好处是可以传一个Vecotor3作为图片显示的位置参数2就是图片的名子，当然这个图片必须放在Gizmos文件夹下面；

```
void OnDrawGizmos()
{
     Gizmos.DrawIcon(transform.position, "0.png", true);
}
```
2.比如要做摄像机轨迹，那么肯定是要在Scene视图中做一个预览的线，那么用 Gizmos.DrawLine 和 Gizmos.DrawFrustum 就再好不过了；
