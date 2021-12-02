# User Manual

## Editor Default Resources

### Detail

1.它必须放在Project视图的根目录下，注意中间是有空格的，如果你想放在/xxx/xxx/Editor Default Resources这样是不行的；

2.你可以把编辑器用到的一些资源放在这里，比如图片、文本文件、等等；

3.它和Editor文件夹一样都不会被打到最终发布包里，仅仅用于开发时使用；

### e.g.

1.通过EditorGUIUtility.Load去读取该文件夹下的资源；

```
TextAsset text = EditorGUIUtility.Load("test.txt") as TextAsset;
Debug.Log(text.text);
```
