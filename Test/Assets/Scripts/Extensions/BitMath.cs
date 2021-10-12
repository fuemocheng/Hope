/********************************************************************
	created:	2021/01/22
	created:	22:1:2021   10:09
	author:		Five
	
	purpose:	lua 位运算
*********************************************************************/

using System;

public static class BitMath
{
    public static int And(int a, int b)
    {
        var r = a & b;
        UnityEngine.Debug.LogError($"{a} And {b} = {r}");
        return r;
    }
    public static int Or(int a, int b)
    {
        return a | b;
    }
    public static int Xor(int a, int b)
    {
        return a ^ b;
    }

    public static int EnumAnd(Enum c, Enum t)
    {
        return c.GetHashCode() & t.GetHashCode();
    }
}
