using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int pillowNum = 0;

    public void setPillowNum(int num)
    {
        pillowNum = num;
    }

    public void incrementPillow()
    {
        pillowNum += 1;
    }

    public int getPillowNum()
    {
        return pillowNum;
    }
}
