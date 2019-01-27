using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillow : MonoBehaviour
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
            gameMain.getPillow();
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (this.transform.position.z - gameMain.getFutonPos().z < -10.0f)
        {
            Destroy(gameObject);
        }
    }
}
