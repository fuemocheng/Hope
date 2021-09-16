using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    private Camera _camera;

    public GameObject[] gameObjects;

    //ˮƽ����
    float halfHFov = 0;

    void Start()
    {
        _camera = Camera.main;
        if (_camera == null) return;
        float distance = 10;
        //����ת����
        float halfFov = (_camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float halfHeight = Mathf.Tan(halfFov) * distance;
        float halfWidth = halfHeight * _camera.aspect;
        //ˮƽ����
        halfHFov = Mathf.Atan(halfWidth / distance);  //ˮƽ����
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
