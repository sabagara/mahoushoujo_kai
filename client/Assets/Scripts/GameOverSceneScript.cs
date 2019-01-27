using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneScript : MonoBehaviour
{
    private void Start()
    {
        SoundController.Instance.playBgm(SoundController.SOUND.BGM_GAME_OVER);
        SoundController.Instance.play(SoundController.SOUND.SE_DEADKOICHI_end);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
