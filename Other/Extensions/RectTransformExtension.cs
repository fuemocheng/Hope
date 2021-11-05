using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtension
{
    public static RectTransform GetRectTransform(this Transform trans)
    {
        return trans.GetComponent<RectTransform>();
    }

    public static Canvas GetRootCanvas(this RectTransform rt)
    {
        Canvas[] canvases = rt.GetComponentsInParent<Canvas>();
        Canvas canvas = null;
        foreach (var c in canvases)
        {
            if (c.isRootCanvas)
            {
                canvas = c;
                break;
            }
        }

        return canvas;
    }

    public static Rect GetWorldRect(this RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        var rect = new Rect();
        rect.xMin = corners[0].x;
        rect.xMax = corners[2].x;
        rect.yMin = corners[0].y;
        rect.yMax = corners[1].y;

        return rect;
    }

    public static bool ContainScreenPoint(this RectTransform rt, Vector2 pt)
    {
        var canvas = rt.GetRootCanvas();
        var outPos = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, pt, canvas.worldCamera, out outPos);

        LogUtils.Log(outPos);
        return rt.rect.Contains(outPos);
    }

    public static void SetAnchoredPositionByScreenPosition(this RectTransform rt, Vector2 screenPos)
    {
        var canvas = rt.GetRootCanvas();

        Camera camera = canvas.worldCamera;
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt.parent as RectTransform, screenPos, camera, out localPos);

        rt.anchoredPosition = localPos;
    }
    public static void SetLocalPositionByScreenPosition(this RectTransform rt, Vector2 screenPos)
    {
        var canvas = rt.GetRootCanvas();

        Camera camera = canvas.worldCamera;
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt.parent as RectTransform, screenPos, camera, out localPos);

        rt.localPosition = localPos;
    }

    public static Vector2 GetScreenPosition(this RectTransform rt)
    {
        Canvas canvas = rt.GetComponentInParent<Canvas>();
        Camera camera = canvas.worldCamera;

        return camera.WorldToScreenPoint(rt.position);
    }

    //anchoredPosition转换到Canvas空间坐标系，原点在中心，可再转换成屏幕坐标
    public static Vector2 TransformAnchoredPointToCanvasSpace(this RectTransform rt, Vector2 anchoredPosition)
    {
        var relativePos = anchoredPosition;
        var parent = rt;
        if (parent != null)
        {
            //将anchoredPosition转换成localPos
            relativePos -= new Vector2(parent.rect.size.x * (parent.pivot.x - 0.5f), parent.rect.size.y * (parent.pivot.y - 0.5f));
        }

        while (parent != null && parent.GetComponent<Canvas>() == null)
        {
            Vector2 parentPos = parent.localPosition;
            var pivotOffset = Vector2.zero;
            if (parent.parent != null)
            {
                var prt = parent.parent.GetRectTransform();
                var ppSize = prt.rect.size;
                var anchor = (parent.anchorMin + parent.anchorMax) / 2;

                //pivotOffset += new Vector2(ppSize.x * (anchor.x - 0.5f), ppSize.y * (anchor.y - 0.5f));

                var pSize = parent.rect.size * parent.localScale;
                //pivotOffset -= new Vector2(pSize.x * (parent.pivot.x - 0.5f), pSize.y * (parent.pivot.y - 0.5f));
            }

            relativePos = relativePos + pivotOffset;
            relativePos = parent.localRotation * relativePos;
            relativePos = parent.localScale * relativePos;
            relativePos = parentPos + relativePos;

            parent = parent.parent as RectTransform;
        }

        //实际上这时是localPosition，因为是canvas，anchor是一个集中点，跟anchoredPosition相等
        return relativePos;
    }

    //anchoredPosition转换成世界坐标
    public static Vector3 TransformAnchoredPoint(this RectTransform rt, Vector2 anchoredPosition)
    {
        Canvas canvas = rt.GetComponentInParent<Canvas>();
        Camera camera = canvas.worldCamera;

        var canvasPos = rt.TransformAnchoredPointToCanvasSpace(anchoredPosition);

        var canvasRt = canvas.transform.GetRectTransform();
        //var screenSpace = new Vector3(
        //                    canvasPos.x + 0.5f * canvasRt.sizeDelta.x * canvas.scaleFactor,
        //                    canvasPos.y + 0.5f * canvasRt.sizeDelta.y * canvas.scaleFactor,
        //                    Mathf.Abs(camera.transform.position.z - rt.position.z));

        var screenSpace = new Vector3(
                            canvasPos.x * (Screen.width / canvasRt.sizeDelta.x) + 0.5f * Screen.width,
                            canvasPos.y * (Screen.height / canvasRt.sizeDelta.y) + 0.5f * Screen.height,
                            (camera.transform.position - canvasRt.position).magnitude);

        return camera.ScreenToWorldPoint(screenSpace);
    }

    //不同坐标系下Node1在Node2的本地位置
    public static Vector2 AnchoredPosNode1InNode2Local(RectTransform node1, RectTransform node2)
    {
        Canvas canvas = node1.GetComponentInParent<Canvas>();
        Camera camera = canvas.worldCamera;

        //Debug.Log ("===========: " + node1.position);
        Vector2 screenPos = camera.WorldToScreenPoint(node1.position);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(node2, screenPos, camera, out localPos);
        return localPos;
    }
}
