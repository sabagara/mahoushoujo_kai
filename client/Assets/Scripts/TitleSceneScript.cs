using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneScript : MonoBehaviour
{
    private void Start()
    {
        SoundController.Instance.playBgm(SoundController.SOUND.BGM_TITLE);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("GameMain");
        }
    }
}
