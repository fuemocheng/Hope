using Sentry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestError : MonoBehaviour
{

    private GameObject testOb = null;

    void Start()
    {
        //StartCoroutine(DelayDo());
    }


    void Update()
    {
        
    }

    IEnumerator DelayDo()
    {
        Debug.LogError("TestError Start!");
        yield return new WaitForSeconds(2f);
        testOb.transform.localPosition = Vector3.zero;
    }

    public void OnClickTestError()
    {
        //Debug.LogError("TestError OnClickTestError!");
        //SentrySdk.CaptureMessage("Test event - 2");
        testOb.transform.localPosition = Vector3.zero;

    }

    public void OnClickTestCrash()
    {
        Debug.LogError("TestError OnClickTestCrash!");
        UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.Abort);
    }
}
