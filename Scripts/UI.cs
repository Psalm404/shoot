using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text rabbitScoreText; // UI 的文本组件
    public Text bearScoreText;
    public Text boardScoreText;
    public Text deerScoreText;
    public Text wolfScoreText;
    public Text totalScoreText;
    private void Start()
    {
       // rabbitScoreText = GameObject.Find("Canvas/target_item").transform.Find("Rabbit").GetComponent<Text>(); 
        ScoreManager.Instance.OnRabbitScoreChanged += UpdateRabbitScore;
        rabbitScoreText.text = "0" + " / 5";
        ScoreManager.Instance.OnBearScoreChanged += UpdateBearScore;
        bearScoreText.text = "0" + " / 4";
        ScoreManager.Instance.OnDeerScoreChanged += UpdateDeerScore;
        deerScoreText.text = "0" + " / 5";
        ScoreManager.Instance.OnBoardScoreChanged += UpdateBoardScore;
        boardScoreText.text = "0" + " /" + " 11";
        ScoreManager.Instance.OnWolfScoreChanged += UpdateWolfScore;
        wolfScoreText.text = "0" + " / 5";

        ScoreManager.Instance.OnTotalScoreChanged += UpdateTotalScore;
    }

    private void OnDestroy()
    {
        // 取消订阅事件，防止内存泄漏
        ScoreManager.Instance.OnRabbitScoreChanged -= UpdateRabbitScore;
        ScoreManager.Instance.OnBearScoreChanged -= UpdateBearScore;
        ScoreManager.Instance.OnDeerScoreChanged -= UpdateDeerScore;
        ScoreManager.Instance.OnBoardScoreChanged -= UpdateBoardScore;
        ScoreManager.Instance.OnWolfScoreChanged -= UpdateWolfScore;
        ScoreManager.Instance.OnTotalScoreChanged -= UpdateTotalScore;
    }

    // 更新 UI 显示
    private void UpdateRabbitScore(int newNum)
    {
        rabbitScoreText.text = newNum.ToString() + " / 5";
    }

    private void UpdateBearScore(int newNum) { 
        
        bearScoreText.text = newNum.ToString() + " / 4";
    }

    private void UpdateDeerScore(int newNum)
    {

        deerScoreText.text = newNum.ToString() + " / 5";
    }

    private void UpdateWolfScore(int newNum)
    {

        wolfScoreText.text = newNum.ToString() + " / 6";
    }

    private void UpdateBoardScore(int newNum)
    {
        boardScoreText.text = newNum.ToString() + " / " + "11";
    }

    private void UpdateTotalScore(int newNum) {
        totalScoreText.text = newNum.ToString();
    }

}
