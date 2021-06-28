using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sentry;

public class SentryInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SentrySdk.Init("https://05521a3f3f8d48ce938f6f86ea7be3da@o554065.ingest.sentry.io/5683402");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
