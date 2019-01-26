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

    private GameObject mainCamera;
    private Vector3 mainCameraPos;

    private GameObject blockDefaultPrefab;
    private float lastStagePos = 0.0f;

    private int futonHomePosMax = 3;
    private float[] futonHomePos = { -3.6f, -1.3f, 1.3f, 3.6f };

    private float runSpeed = 20.0f;
    private float sideStepSpeed = 10.0f;
    private float jumpPower = 15.0f;
    private float gravityAcceleration = 45.0f;

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        mainCameraPos = mainCamera.transform.position;

        // player
        futon = GameObject.Find("Futon");
        futonPos = futon.transform.position;

        futonCurrentHomePosNo = 1;
        futonXPosTarget = futonHomePos[futonCurrentHomePosNo];
        futonPos.x = futonHomePos[futonCurrentHomePosNo];

        futon.transform.position = futonPos;

        isJump = false;

        // stage
        blockDefaultPrefab = (GameObject)Resources.Load("Prefabs/BlockDefault");

        initializeStage();
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

        futonPos.x = Mathf.Lerp(futonPos.x, futonXPosTarget, sideStepSpeed * Time.deltaTime);

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

        futonPos.z += runSpeed * Time.deltaTime;

        futon.transform.position = futonPos;

        mainCameraPos.z += runSpeed * Time.deltaTime;
        mainCamera.transform.position = mainCameraPos;
    }

    // ステージの初期化
    private void initializeStage()
    {
        // stage
        for (int i = 0; i < 10; ++i)
        {
            Vector3 generatePos = new Vector3(0.0f, 0.0f, i * 15.0f);
            GameObject cloneSheep = Instantiate(blockDefaultPrefab, generatePos, Quaternion.identity) as GameObject;
            lastStagePos = i * 15.0f;
        }
    }
}