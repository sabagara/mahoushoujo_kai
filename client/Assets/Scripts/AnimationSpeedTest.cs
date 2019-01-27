using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animation>()["Take 001"].speed = 4.0f;
    }
}
