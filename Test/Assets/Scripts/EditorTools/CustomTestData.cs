using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomTestData : MonoBehaviour
{
    public List<CustomComData> listData;
}

[Serializable]
public class CustomComData
{
    public ECustomComType customComType;
    public Texture texture;
    public Button button;
    public GameObject gameObject;
    public int intValue;
    public string strValue;
}