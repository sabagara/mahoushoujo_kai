using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameMain gameMain;

    private float[] futonHomePos = { -3.6f, -1.3f, 1.3f, 3.6f };

    private void Start()
    {
        gameMain = GameObject.Find("GameMain").GetComponent<GameMain>();

        Vector3 goalPosition = gameObject.transform.position;
        int idx = Random.Range(0, 4);
        goalPosition.x = futonHomePos[idx];
        gameObject.transform.position = goalPosition;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Futon")
        {
            gameMain.gameClear();
        }
    }
}
