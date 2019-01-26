using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

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
    private GameObject blockGoalPrefab;
    private GameObject blockCrossroadsPrefab;
    private float lastStagePos = 0.0f;
    private GameObject stageObject;
    private int crossroadsCount;

    private int futonHomePosMax = 3;
    private float[] futonHomePos = { -3.6f, -1.3f, 1.3f, 3.6f };
    private int futonPosLeftIdx = 0;
    private int futonPosRightIdx = 3;

    private float runSpeed = 20.0f;
    private float sideStepSpeed = 10.0f;
    private float jumpPower = 15.0f;
    private float gravityAcceleration = 45.0f;

    private float goalPositon = 20.0f * 60.0f;//runSpeed * goalSec;

    GlobalManager globalManager;

    private GameObject telephonePolePrefab;
    private GameObject hurdlePrefab;
    private float[] enemyPosZTable = { 6.0f, 3.0f, 0.0f, -3.0f, -6.0f };
    private int[] telephonePolePosIdx = { 1, 4 };


    void Start()
    {
        //init Global
        globalManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
        globalManager.setPillowNum(0);

        // init Camera
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
        blockGoalPrefab = (GameObject)Resources.Load("Prefabs/BlockGoal");
        blockCrossroadsPrefab = (GameObject)Resources.Load("Prefabs/BlockCrossroads");
        stageObject = GameObject.Find("Stage");
        crossroadsCount = 0;

        initializeStage();

        // Enemy
        telephonePolePrefab = (GameObject)Resources.Load("Prefabs/TelephonePole");
        hurdlePrefab = (GameObject)Resources.Load("Prefabs/Hurdle");
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
            crossroadsCount -= 1;

            if (lastStagePos >= goalPositon)
            {
                addBlock(blockGoalPrefab, lastStagePos);
            }
            else if (crossroadsCount <= 0 && Random.Range(0, 100) < 40)
            {
                addBlock(blockCrossroadsPrefab, lastStagePos);
                crossroadsCount = 2;
            }
            else
            {
                addBlock(blockDefaultPrefab, lastStagePos);
                addEnemyDefault(lastStagePos);
            }
        }
    }

    public Vector3 getFutonPos()
    {
        return futonPos;
    }

    // ステージの初期化
    private void initializeStage()
    {
        // stage
        for (int i = 0; i < 3; ++i)
        {
            lastStagePos = i * 15.0f;
            addBlock(blockDefaultPrefab, lastStagePos);
        }
    }

    private void addBlock(GameObject genPrefab, float posZ)
    {
        Vector3 generatePos = new Vector3(0.0f, 0.0f, posZ);
        GameObject cloneBlock = Instantiate(genPrefab, generatePos, Quaternion.identity) as GameObject;
        cloneBlock.transform.SetParent(stageObject.transform);
    }

    private void addEnemyDefault(float posZ)
    {
        int[] posIdxTable = { futonPosLeftIdx, futonPosRightIdx };
        foreach (int idx in posIdxTable)
        {
            if (Random.Range(0, 100) < 40)
            {
                Vector3 generatePos = new Vector3(futonHomePos[idx], 0.0f, posZ + enemyPosZTable[telephonePolePosIdx[Random.Range(0, 1)]]);
                GameObject cloneBlock = Instantiate(telephonePolePrefab, generatePos, Quaternion.identity) as GameObject;
                cloneBlock.transform.SetParent(stageObject.transform);
            }
        }


        foreach (float zpos in enemyPosZTable)
        {
            foreach (float xpos in futonHomePos)
            {
                if (Random.Range(0, 100) < 10)
                {
                    Vector3 generatePos = new Vector3(xpos, 0.0f, posZ + zpos);
                    GameObject cloneBlock = Instantiate(hurdlePrefab, generatePos, Quaternion.identity) as GameObject;
                    cloneBlock.transform.SetParent(stageObject.transform);
                }
            }
        }

    }

    public void gameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void gameClear()
    {
        SceneManager.LoadScene("GameClear");
    }
}