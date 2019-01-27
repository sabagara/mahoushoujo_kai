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

    ScoreManager scoreManager;

    private GameObject telephonePolePrefab;
    private GameObject hurdlePrefab;
    private GameObject cornPrefab;
    private GameObject drainpipePrefab;
    private GameObject carPrefab;
    private GameObject masahiroPrefab;
    private float[] enemyPosZTable = { 6.0f, 3.0f, 0.0f, -3.0f, -6.0f };
    private int[] telephonePolePosIdx = { 1, 4 };

    private bool isPillowGen = false;

    private GameObject pillowPrefab;
    private float[] pillowPosZTable = { 200.0f, 700.0f, 1100.0f };

    enum PlayerStatus {
        RUN,
        GAME_OVER,
        GAME_CLEAR,
    }
    PlayerStatus playerStatus;

    void Start()
    {
        SoundController.Instance.playBgm(SoundController.SOUND.BGM_GAME_MAIN);

        //init Global
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        scoreManager.setPillowNum(0);

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
        cornPrefab = (GameObject)Resources.Load("Prefabs/Corn");
        drainpipePrefab = (GameObject)Resources.Load("Prefabs/Drainpipe");
        carPrefab = (GameObject)Resources.Load("Prefabs/Car");
        masahiroPrefab = (GameObject)Resources.Load("Prefabs/Masahiro");

        pillowPrefab = (GameObject)Resources.Load("Prefabs/Pillow");

        playerStatus = PlayerStatus.RUN;

        isPillowGen = false;
    }

    void Update()
    {
        if (isJump == false)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && futonCurrentHomePosNo < futonHomePosMax)
            {
                futonCurrentHomePosNo += 1;
                SoundController.Instance.play(SoundController.SOUND.SE_SHIFT);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && futonCurrentHomePosNo > 0)
            {
                futonCurrentHomePosNo -= 1;
                SoundController.Instance.play(SoundController.SOUND.SE_SHIFT);
            }
        }
        futonXPosTarget = futonHomePos[futonCurrentHomePosNo];

        futonPos.x = Mathf.Lerp(futonPos.x, futonXPosTarget, sideStepSpeed * Time.deltaTime);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isJump == false)
        {
            futonDirection.y = jumpPower;
            isJump = true;
            SoundController.Instance.play(SoundController.SOUND.SE_SHIFT);
        }


        if (playerStatus == PlayerStatus.RUN)
        {
            futonPos.z += runSpeed * Time.deltaTime;
            mainCameraPos.z += runSpeed * Time.deltaTime;

            futonDirection.y = futonDirection.y - gravityAcceleration * Time.deltaTime;
            futonPos += futonDirection * Time.deltaTime;

            if (futonPos.y < 0.0f)
            {
                futonDirection.y = 0.0f;
                futonPos.y = 0.0f;
                isJump = false;
            }
        }
        else if (playerStatus == PlayerStatus.GAME_OVER)
        {
            futonPos += futonDirection * Time.deltaTime;
            futon.transform.Rotate(new Vector3(-700.0f, 0, 0) * Time.deltaTime, Space.World);
        }
        else if (playerStatus == PlayerStatus.GAME_CLEAR)
        {

        }


        futon.transform.position = futonPos;
        mainCamera.transform.position = mainCameraPos;

        // ステージ自動生成 & 削除
        if (lastStagePos - futonPos.z < 30.0f)
        {
            lastStagePos += 15.0f;
            crossroadsCount -= 1;

            foreach (float pillowPosZ in pillowPosZTable)
            {
                if (pillowPosZ <= lastStagePos && lastStagePos < (pillowPosZ + 15.0f))
                {
                    isPillowGen = true;
                }
            }

            if (lastStagePos >= goalPositon)
            {
                addBlock(blockGoalPrefab, lastStagePos);
            }
            else if (crossroadsCount <= 0 && Random.Range(0, 100) < 40)
            {
                addBlock(blockCrossroadsPrefab, lastStagePos);
                addCar(lastStagePos);
                crossroadsCount = 2;
            }
            else
            {
                addBlock(blockDefaultPrefab, lastStagePos);
                addObject(lastStagePos);
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

    private void addObject(float posZ)
    {
        int[] reservedPillow = { Random.Range(0, futonHomePos.Length), Random.Range(0, enemyPosZTable.Length) };

        for (int zposIdx = 0; zposIdx < enemyPosZTable.Length; ++zposIdx)
        {
            float zpos = enemyPosZTable[zposIdx];
            for (int xposIdx = 0; xposIdx < futonHomePos.Length; ++xposIdx)
            {
                float xpos = futonHomePos[xposIdx];

                if (isPillowGen && reservedPillow[0] == xposIdx && reservedPillow[1] == zposIdx)
                {
                    Vector3 generatePos = new Vector3(xpos, 0.0f, posZ + zpos);
                    GameObject cloneBlock = Instantiate(pillowPrefab, generatePos, Quaternion.identity) as GameObject;
                    cloneBlock.transform.SetParent(stageObject.transform);
                    isPillowGen = false;
                }
                else if ((zposIdx % 2 == 1) && (xposIdx == futonPosLeftIdx || xposIdx == futonPosRightIdx) && Random.Range(0, 100) < 20)
                {
                    Vector3 generatePos = new Vector3(xpos, 0.0f, posZ + zpos);
                    GameObject cloneBlock = Instantiate(telephonePolePrefab, generatePos, Quaternion.identity) as GameObject;
                    cloneBlock.transform.SetParent(stageObject.transform);
                }
                else if ((zposIdx % 2 == 0) && Random.Range(0, 100) < 20)
                {
                    int rate = Random.Range(0, 100);
                    GameObject enemyPrefab = hurdlePrefab;
                    if (rate < 1)
                    {
                        enemyPrefab = masahiroPrefab;
                    }
                    else if (rate < 10)
                    {
                        enemyPrefab = drainpipePrefab;
                    }
                    else if (rate < 60)
                    {
                        enemyPrefab = cornPrefab;
                    }

                    Vector3 generatePos = new Vector3(xpos, 0.0f, posZ + zpos);
                    GameObject cloneBlock = Instantiate(enemyPrefab, generatePos, Quaternion.identity) as GameObject;
                    cloneBlock.transform.SetParent(stageObject.transform);
                }
            }
        }

    }

    private void addCar(float posZ)
    {
        if (Random.Range(0, 100) < 70)
        {
            Vector3 generatePos;
            Quaternion carQuot = Quaternion.identity;
            if (Random.Range(0, 100) < 50)
            {
                generatePos = new Vector3(-80.0f, 0.0f, posZ + 2.5f);
            }
            else
            {
                generatePos = new Vector3(80.0f, 0.0f, posZ - 2.5f);
                carQuot = Quaternion.Euler(0, 180, 0);
            }
            GameObject cloneCar = Instantiate(carPrefab, generatePos, carQuot) as GameObject;
            cloneCar.transform.SetParent(stageObject.transform);
        }
    }


    public void gameOver()
    {
        if (playerStatus == PlayerStatus.RUN)
        {
            Invoke("changeGameOverScene", 1); // 1秒後にchangeGameOverSceneを呼び出し
            playerStatus = PlayerStatus.GAME_OVER; // 以降、ゲームオーバー開始処理を実行しない。

            GameObject effectDeadPrefab = (GameObject)Resources.Load("Prefabs/EffectDead");
            GameObject cloneBlock = Instantiate(effectDeadPrefab, futonPos, Quaternion.identity) as GameObject;
            cloneBlock.transform.SetParent(stageObject.transform);
            SoundController.Instance.play(SoundController.SOUND.SE_BAD_end);
            futonDirection.y = 20.0f;
        }
    }
    public void gameClear()
    {
        if (playerStatus == PlayerStatus.RUN)
        {
            Invoke("changeGameClearScene", 3); // 3秒後にchangeGameClearSceneを呼び出し
            playerStatus = PlayerStatus.GAME_CLEAR; // 以降、ゲームオーバー開始処理を実行しない。

            GameObject effectGoalPrefab = (GameObject)Resources.Load("Prefabs/EffectGoal");
            GameObject cloneBlock = Instantiate(effectGoalPrefab, futonPos, Quaternion.identity) as GameObject;
            cloneBlock.transform.SetParent(stageObject.transform);
            SoundController.Instance.play(SoundController.SOUND.SE_CLEAR);
        }
    }

    private void changeGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void changeGameClearScene()
    {
        SceneManager.LoadScene("GameClear");
    }

    public void getPillow()
    {
        scoreManager.incrementPillow();

        GameObject effectDeadPrefab = (GameObject)Resources.Load("Prefabs/EffectPillow");
        GameObject cloneBlock = Instantiate(effectDeadPrefab, futonPos, Quaternion.identity) as GameObject;
        cloneBlock.transform.SetParent(futon.transform);
        SoundController.Instance.play(SoundController.SOUND.SE_PILLOW);

        if (scoreManager.getPillowNum() == 1)
        {
            GameObject pillow1 = GameObject.Find("PillowScore1");
            pillow1.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            GameObject pillow2 = GameObject.Find("PillowScore2");
            pillow2.GetComponent<Renderer>().enabled = true;
        }
    }
}
