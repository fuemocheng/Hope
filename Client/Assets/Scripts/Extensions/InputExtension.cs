using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class InputExtension
{
    //isWorldPos 获取parent的世界坐标还是本地坐标
    //当isWorldPos = true，parent是当前canvas同一z的任意transform，一般transform都可
    //当isWorldPos = false，parent则是要转换成本地坐标的父对象
    public static Vector2 GetPointerPosition(this PointerEventData eventData, Transform parent, bool isWorldPos)
    {
        var camera = GameObject.Find("UICamera").GetComponent<Camera>();
        var pt = eventData.position;
        var outPos = new Vector2();
        var outPos3 = new Vector3();

        if (isWorldPos)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(parent as RectTransform, pt, camera, out outPos3);
            outPos = outPos3;
        }
        else
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent as RectTransform, pt, camera, out outPos);

        return outPos;
    }
}
