using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    private Camera _camera;

    public GameObject[] gameObjects;

    //水平弧度
    float halfHFov = 0;

    void Start()
    {
        _camera = Camera.main;
        if (_camera == null) return;
        float distance = 10;
        //度数转弧度
        float halfFov = (_camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float halfHeight = Mathf.Tan(halfFov) * distance;
        float halfWidth = halfHeight * _camera.aspect;
        //水平弧度
        halfHFov = Mathf.Atan(halfWidth / distance);  //水平弧度
    }

    void Update()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] == null)
                continue;

            Vector3 forward = _camera.transform.forward;
            Vector3 roleToCam = gameObjects[i].transform.position - _camera.transform.position;

            float angle = Vector3.Angle(forward, roleToCam) * Mathf.Deg2Rad;

            if (angle > halfHFov)
                gameObjects[i].SetActive(false);
            else
                gameObjects[i].SetActive(true);

        }
    }
}
