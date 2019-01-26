using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private GameObject futon;
    private Vector3 futonPos;
    private Vector3 futonDirection;

    private int futonCurrentHomePosNo;
    private float futonXPosTarget;

    private bool isJump;

    private int futonHomePosMax = 3;
    private float[] futonHomePos = { -3.6f, -1.3f, 1.3f, 3.6f };
   
    private float moveSpeed = 10.0f;
    private float jumpPower = 15.0f;
    private float gravityAcceleration = 45.0f;

    void Start()
    {
        futon = GameObject.Find("Futon");
        futonPos = futon.transform.position;

        futonCurrentHomePosNo = 1;
        futonXPosTarget = futonHomePos[futonCurrentHomePosNo];
        futonPos.x = futonHomePos[futonCurrentHomePosNo];

        futon.transform.position = futonPos;

        isJump = false;
    }

    void Update()
    {
        if (isJump == false)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && futonCurrentHomePosNo < futonHomePosMax)
            {
                futonCurrentHomePosNo += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && futonCurrentHomePosNo > 0)
            {
                futonCurrentHomePosNo -= 1;
            }
        }
        futonXPosTarget = futonHomePos[futonCurrentHomePosNo];

        futonPos.x = Mathf.Lerp(futonPos.x, futonXPosTarget, moveSpeed * Time.deltaTime);

        Debug.Log("x:" + futonPos.x + " tx:" + futonXPosTarget + " posNo:" + futonCurrentHomePosNo);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isJump == false)
        {
            futonDirection.y = jumpPower;
            isJump = true;
        }

        futonDirection.y = futonDirection.y - gravityAcceleration * Time.deltaTime;
        futonPos += futonDirection * Time.deltaTime;

        if (futonPos.y < 0.0f)
        {
            futonDirection.y = 0.0f;
            futonPos.y = 0.0f;
            isJump = false;
        }

        futon.transform.position = futonPos;
    }
}