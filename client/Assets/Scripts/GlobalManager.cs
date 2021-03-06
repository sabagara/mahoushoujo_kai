﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GlobalManager : MonoBehaviour
{
    private void Awake()
    {
        // スコアマネージャーの生成
        GameObject scoreManager = GameObject.Find("ScoreManager");
        if (scoreManager == null)
        {
            // シーン内にスコアマネージャーアが存在しない場合は生成(一度生成されたらシーンを切り替えても消えません)
            scoreManager = new GameObject();
            scoreManager.name = "ScoreManager";
            scoreManager.AddComponent<ScoreManager>();
        }
    }
}
