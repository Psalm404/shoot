using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfTarget : Target
{

    private int WolfScore = 50;
    public override void Hit()
    {
        ScoreManager.Instance.Wolf++;
    }
    public override int GetScore()
    {
        return WolfScore; 
    }
    public override void ResetGame()
    {
        isHited = false;
    }
}

