using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTarget : Target
{
    private int bearScore = 70;
    public override int GetScore()
    {
        return bearScore;

    }

    public override void Hit()
    {
        ScoreManager.Instance.Bear++;
    }
    public override void ResetGame()
    {
        isHited = false;
    }
}

