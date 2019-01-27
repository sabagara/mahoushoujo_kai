using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearSceneScript : MonoBehaviour
{
    ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        SoundController.Instance.playBgm(SoundController.SOUND.BGM_GAME_OVER);

        if (scoreManager.getPillowNum() == 0)
        {
            SoundController.Instance.play(SoundController.SOUND.SE_MostBAD_end);
            GameObject.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/clear0");
        }
        else if (scoreManager.getPillowNum() == 1)
        {
            SoundController.Instance.play(SoundController.SOUND.SE_TRUE_clear);
            GameObject.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/clear1");
        }
        else
        {
            SoundController.Instance.play(SoundController.SOUND.SE_ANOTHOR_clear);
            GameObject.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/clear2");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
