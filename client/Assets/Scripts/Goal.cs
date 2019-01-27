using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameMain gameMain;

    private void Start()
    {
        gameMain = GameObject.Find("GameMain").GetComponent<GameMain>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Futon")
        {
            gameMain.gameClear();
        }
    }
}
