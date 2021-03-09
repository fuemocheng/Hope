using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestUpdateAsset : MonoBehaviour
{

    public UpdateAssets updateAssets;

    void Start()
    {
        StartCoroutine(InitUpdateAsset(LoadScene));
    }

    
    void Update()
    {
            
    }

    IEnumerator InitUpdateAsset(Action afterUpdateAction)
    {
        if (updateAssets)
        {
            yield return updateAssets.OnStart();
        }
        if (afterUpdateAction != null)
        {
            afterUpdateAction();
        }
    }

    public void LoadScene()
    {
        Debug.LogError("LoadScene");

    }

}
