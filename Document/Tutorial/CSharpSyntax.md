# C# 语法记录
## 1.StructLayoutAttribute 结构体布局
### e.g.
```
[StructLayout(LayoutKind.Auto)]
public partial struct UpdatableVersionList
{
    ...
}
```
### 链接
[StructLayoutAttribute(结构体布局)](https://blog.csdn.net/bigpudding24/article/details/50727792)  
StructLayoutAttribute
我们可以在定义struct时，在struct上运用StructLayoutAttribute特性来控制成员的内存布局;  
默认情况下，struct实例中的字段在栈上的布局(Layout)顺序与声明中的顺序相同;  
即在struct上运用[StructLayoutAttribute(LayoutKind.Sequential)]特性，这样做的原因是结构常用于和非托管代码交互的情形;  

默认(LayoutKind.Sequential)情况下，CLR对struct的Layout的处理方法与C/C++中默认的处理方式相同，即按照结构中占用空间最大的成员进行对齐(Align);  
使用LayoutKind.Explicit的情况下，CLR不对结构体进行任何内存对齐(Align)，而且我们要小心就是FieldOffset;  
使用LayoutKind.Auto的情况下，CLR会对结构体中的字段顺序进行调整，使实例占有尽可能少的内存，并进行4byte的内存对齐(Align);  

[C#中结构体与字节流互相转换](https://www.cnblogs.com/fengye87626/p/3805879.html)
#### a.定义与C++对应的C#结构体
在c#中的结构体不能定义指针，不能定义字符数组，只能在里面定义字符数组的引用:    
C++的消息结构体如下：  
```
//消息格式 4+16+4+4= 28个字节
struct cs_message{
    u32_t cmd_type;
    char username[16];
    u32_t dstID;
    u32_t srcID;
};
```
C#定义的结构体如下:  
```
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct my_message
{
    public UInt32 cmd_type;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string username;
    public UInt32 dstID;
    public UInt32 srcID;
    public my_message(string s)
    {
        cmd_type = 0;
        username = s;
        dstID = 0;
        srcID = 0;
    }
}
```
在C++的头文件定义中，使用了 #pragma pack 1 字节按1对齐，所以C#的结构体也必须要加上对应的特性，LayoutKind.Sequential属性让结构体在导出到非托管内存时按出现的顺序依次布局,而对于C++的char数组类型，C#中可以直接使用string来对应，当然了，也要加上封送的特性和长度限制。

#### b.结构体与byte[]的互相转换
定义一个类，里面有2个方法去实现互转：  
```
public class Converter
{
    public Byte[] StructToBytes(Object structure)
    {
        Int32 size = Marshal.SizeOf(structure);
        Console.WriteLine(size);
        IntPtr buffer = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.StructureToPtr(structure, buffer, false);
            Byte[] bytes = new Byte[size];
            Marshal.Copy(buffer, bytes, 0, size);
            return bytes;
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    public Object BytesToStruct(Byte[] bytes, Type strcutType)
    {
        Int32 size = Marshal.SizeOf(strcutType);
        IntPtr buffer = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.Copy(bytes, 0, buffer, size);
            return Marshal.PtrToStructure(buffer, strcutType);
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }
}
```

## 2.MarshalAs属性 指示如何在托管代码和非托管代码之间封送数据
### 使用方法：  
[MarshalAs(UnmanagedType unmanagedType, 命名参数)]  
实际上相当于构造一个MarshalAsAttribute类的对象  
常用的UnmanagedType枚举值：（详细内容查MSDN）  
```
BStr   长度前缀为双字节的 Unicode 字符串；
LPStr  单字节、空终止的 ANSI 字符串。；
LPWStr  一个 2 字节、空终止的 Unicode 字符串；
ByValArray 用于在结构中出现的内联定长字符数组，应始终使用MarshalAsAttribute的SizeConst字段来指示数组的大小。
```
注意：  
在用Marshal.SizeOf()，即获取对象的非托管大小时，获得的是自己定义的大小；  
但在实际处理的时候，是按照实际的大小来获取的  
示例：  
定义一个固定大小的结构体，代码如下：  
结构的声明：  
```
struct Info  
{  
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]  
    public char[] name;  
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]  
    public char[] cipher;  
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]  
    public char[] signature;  
}  
```
结构的使用：  
```
Info myInfo;  
myInfo.name = name.ToCharArray();  
myInfo.cipher = cipher.ToCharArray();  
myInfo.signature = signature.ToCharArray();  
```
注意：  
int size = Marshal.SizeOf(myInfo);    
size=16+16+256  
可见，获取到的非托管大小为288  

## 3.FlagAttribute 指示可以将枚举作为位域（即一组标志）处理
[Enum, Flags and bitwise operators](http://www.alanzucconi.com/2015/07/26/enum-flags-and-bitwise-operators/)
Flag 特性微软的解释是：指示可以将枚举作为位域（即一组标志）处理，FlagsAttribute属性就是枚举类型的一项可选属性，它的主要作用是可以将枚举作为位域处理(P.S. C#不支持位域)。所谓位域是单个存储单元内相邻二进制位的集合。

eg.
```
// Powers of two
[Flags] 
public enum AttackType 
{
    // Decimal     // Binary
    None   = 0,    // 000000
    Melee  = 1,    // 000001
    Fire   = 2,    // 000010
    Ice    = 4,    // 000100
    Poison = 8     // 001000
}

```
### Bitwise OR  按位或
What the bitwise OR does is setting the bit in the i-th position to 1 if either one of its operands has the i-th bit to 1.  
按位 OR 的作用是 如果其任一操作数的第 i 个位为 1，则将第 i 个位置的位设置为 1。
```
attackType = AttackType.Melee | AttackType.Fire;
// OR
attackType = AttackType.Melee;
attackType |= AttackType.Fire;
```
```
// Label          Binary   Decimal
// Melee:         000001 = 1
// Fire:          000010 = 2
// Melee | Fire:  000011 = 3
```
```
[Flags] 
public enum AttackType 
{
    // Decimal                  // Binary
    None         = 0,           // 000000
    Melee        = 1,           // 000001
    Fire         = 2,           // 000010
    Ice          = 4,           // 000100
    Poison       = 8,           // 001000

    MeleeAndFire = Melee | Fire // 000011
}
```

### Bitwise AND  按位与
按位 OR 的作用是 如果两个操作数的第 i 个位都为 1，则将第 i 个位置的位设置为 1。
```
attackType = AttackType.Melee | AttackType.Fire;
bool isIce = (attackType & AttackType.Ice) != 0;    //判断attackType是否含有Ice
```
```
// Label                Binary   Decimal
// Ice:                 000100 = 4
// MeleeAndFire:        000011 = 3
// MeleeAndFire & Ice:  000000 = 0          MeleeAndFire不含有Ice

// Fire:                000010 = 2
// MeleeAndFire:        000011 = 3
// MeleeAndFire & Fire: 000010 = 2          MeleeAndFire含有Fire
```

### Bitwise NOT 按位非
按位非,它所做的只是反转整数的所有位
```
attackType = AttackType.Melee | AttackType.Fire         // 000011
                                                        // ~ AttackType.Fire = 111101
attackType &= ~ AttackType.Fire;                        // 000001   去除Fire
attackType |= AttackType.Ice;                           // 000101   AttackType.Melee | AttackType.Ice
```
通过按位非 AttackType.Fire 属性，我们留下了一个全 1 的位掩码，除了相对于 Fire 属性的位置为零。  
当与attackType 进行 AND 运算时，它将保持所有其他位不变并取消设置fire 属性。

### Bitwise XOR 按位异或
参与运算的两个值，如果两个相应bit位相同，则结果为0，否则为1。
```
0^0 = 0
1^0 = 1
0^1 = 1
1^1 = 0
按位异或的3个特点：
（1） 0^0=0，0^1=1 0异或任何数＝任何数
（2） 1^0=1，1^1=0 1异或任何数＝任何数取反
（3） 任何数异或自己＝把自己置0
```
It allows to toggle a value.它允许切换值
```
attackType = AttackType.Melee | AttackType.Fire;    //000011

attackType ^= AttackType.Fire;      // Toggle fire  000001
attackType ^= AttackType.Ice;       // Toggle ice   000101
```

### Bitwise shifts 按位移位
可以使用按位移位轻松创建 2 的 n 次幂
```
[Flags] 
public enum AttackType 
{
    //               // Binary  // Dec
    None   = 0,      // 000000  0
    Melee  = 1 << 0, // 000001  1
    Fire   = 1 << 1, // 000010  2
    Ice    = 1 << 2, // 000100  4
    Poison = 1 << 3, // 001000  8
}
```

### Conclusion
```
public static AttackType SetFlag (AttackType a, AttackType b)
{
    return a | b;
}

public static AttackType UnsetFlag (AttackType a, AttackType b)
{
    return a & (~b);
}

// Works with "None" as well
public static bool HasFlag (AttackType a, AttackType b)
{
    return (a & b) == b;
}

public static AttackType ToogleFlag (AttackType a, AttackType b)
{
    return a ^ b;
}
```

