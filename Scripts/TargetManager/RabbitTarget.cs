using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitTarget : Target
{
    private int rabbitScore = 60;
    public override void Hit()
    {
        Debug.Log("HIT RABIT");
        ScoreManager.Instance.Rabbit++;
    }
    public override int GetScore()
    {
        return rabbitScore; 
    }

    public override void ResetGame()
    {
        isHited = false;
    }
}
