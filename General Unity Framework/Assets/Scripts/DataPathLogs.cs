using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPathLogs : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Debug.Log("Application.dataPath:" + Application.dataPath);
        Debug.Log("Application.streamingAssetsPath:" + Application.streamingAssetsPath);
        Debug.Log("Application.persistentDataPath:" + Application.persistentDataPath);
        Debug.Log("Application.temporaryCachePath:" + Application.temporaryCachePath);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
