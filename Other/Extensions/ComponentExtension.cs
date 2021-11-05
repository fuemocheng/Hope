using UnityEngine;

public static class ComponentExtension
{
    public static void CopyComponent(this Component original, Component destination)
    {
        System.Type type = original.GetType();
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            if (field.FieldType.IsValueType)
                field.SetValue(destination, field.GetValue(original));
        }
    }
}
