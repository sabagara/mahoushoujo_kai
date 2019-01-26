using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCommon : MonoBehaviour
{
    private GameMain gameMain;

    private void Start()
    {
        gameMain = GameObject.Find("GameMain").GetComponent<GameMain>();
    }
    
    void Update()
    {
        if (this.transform.position.z - gameMain.getFutonPos().z < -10.0f)
        {
            Destroy(gameObject);
        }
    }
}
