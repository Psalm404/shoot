using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerTarget : Target {

    private int deerScore = 60;
    public override void Hit()
    {
        ScoreManager.Instance.Deer++;
    }
    public override int GetScore()
    {
        return deerScore; 
    }
    public override void ResetGame()
    {
        isHited = false;
    }
}

