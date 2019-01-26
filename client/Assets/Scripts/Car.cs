using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Vector3 targetPos;
    private Vector3 nowPos;

    private float carSpeed = 2.0f;

    void Start()
    {
        nowPos = this.transform.position;
        if (nowPos.x < 0.0f)
        {
            targetPos = new Vector3(-2.0f, nowPos.y, nowPos.z);
        }
        else
        {
            targetPos = new Vector3(2.0f, nowPos.y, nowPos.z);
        }
    }

    void Update()
    {
        nowPos = Vector3.Lerp(nowPos, targetPos, carSpeed * Time.deltaTime);
        this.transform.position = nowPos;
    }
}
