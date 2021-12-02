using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestControl : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    private int fingerId = int.MinValue;

    void Start()
    {
        Debug.LogError("Test Control Start");
        Input.multiTouchEnabled = true;
    }

    void Update()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
