﻿using System.Collections;
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
    private GameObject stageObject;
    private List<GameObject> blockList;

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
        stageObject = GameObject.Find("Stage");
        blockList = new List<GameObject>();

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

        // ステージ自動生成 & 削除
        if (lastStagePos - futonPos.z < 30.0f)
        {
            lastStagePos += 15.0f;
            addBlock(lastStagePos);

            destroyOutBlock(futonPos.z - 10.0f);
        }
    }

    // ステージの初期化
    private void initializeStage()
    {
        // stage
        for (int i = 0; i < 3; ++i)
        {
            lastStagePos = i * 15.0f;
            addBlock(lastStagePos);
        }
    }

    private void addBlock(float posZ)
    {
        Vector3 generatePos = new Vector3(0.0f, 0.0f, posZ);
        GameObject cloneBlock = Instantiate(blockDefaultPrefab, generatePos, Quaternion.identity) as GameObject;
        cloneBlock.transform.SetParent(stageObject.transform);
        blockList.Add(cloneBlock);
    }

    private void destroyOutBlock(float deletePos)
    {
        List<GameObject> deleteTarget = new List<GameObject>(); 

        foreach (GameObject item in blockList)
        {
            if (item.transform.position.z < deletePos)
            {
                deleteTarget.Add(item);
            }
        }

        foreach (GameObject item in deleteTarget)
        {
            Destroy(item);
            blockList.Remove(item); 
        }
    }
}