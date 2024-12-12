using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryTarget :Target
{
    private int BoardScore = 40;
    public override void Hit()
    {
        ScoreManager.Instance.Board++;
    }
    public override int GetScore()
    {
        return BoardScore; // ���İ��ӵĵ÷�Ϊ 100
    }
    public override void ResetGame()
    {
        isHited = false;
    }
}
