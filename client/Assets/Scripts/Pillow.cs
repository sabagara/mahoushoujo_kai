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
    private void Update()
    {
        this.transform.Rotate(new Vector3(0.0f, 100.0f, 0.0f) * Time.deltaTime, Space.World);
    }
}
