
using UnityEngine;
public class ScoreManager
{
    private static ScoreManager _instance;
    BearTarget _bearTarget;
    MilitaryTarget _militaryTarget;
    WolfTarget _wolfTarget;
    DeerTarget _deerTarget;
    public int totalScore;

    // 单例访问器
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ScoreManager();
            }
            return _instance;
        }
    }


    // 事件通知 UI 类
    public event System.Action<int> OnBearScoreChanged;

    private int _bear;
    public int Bear
    {
        get { return _bear; }
        set
        {
            if (_bear != value) // 仅当值发生变化时通知
            {
                _bear = value;
                OnBearScoreChanged?.Invoke(_bear); // 触发事件
            }
        }
    }

    // 事件通知 UI 类
    public event System.Action<int> OnDeerScoreChanged;

    private int _deer;
    public int Deer
    {
        get { return _deer; }
        set
        {
            if (_deer != value) // 仅当值发生变化时通知
            {
                _deer = value;
                OnDeerScoreChanged?.Invoke(_deer); // 触发事件
            }
        }
    }

    // 事件通知 UI 类
    public event System.Action<int> OnWolfScoreChanged;

    private int _wolf;
    public int Wolf
    {
        get { return _wolf; }
        set
        {
            if (_wolf != value) // 仅当值发生变化时通知
            {
                _wolf = value;
                OnWolfScoreChanged?.Invoke(_wolf); // 触发事件
            }
        }
    }

    // 事件通知 UI 类
    public event System.Action<int> OnBoardScoreChanged;

    private int _board;
    public int Board
    {
        get { return _board; }
        set
        {
            if (_board != value) // 仅当值发生变化时通知
            {
                Debug.Log("board is hitted");
                _board = value;
                OnBoardScoreChanged?.Invoke(_board); // 触发事件
            }
        }
    }

    // 事件通知 UI 类
    public event System.Action<int> OnRabbitScoreChanged;

    private int _rabbit;
    public int Rabbit
    {
        get { return _rabbit; }
        set
        {
            if (_rabbit != value) // 仅当值发生变化时通知
            {
                Debug.Log("rabbit is hited" + value);
                _rabbit = value;
                OnRabbitScoreChanged?.Invoke(_rabbit); // 触发事件
            }
        }
    }

    public event System.Action<int> OnTotalScoreChanged;

    public int TotalScore
    {
        get { return totalScore; }
        set
        {
            if (totalScore != value) // 仅当值发生变化时通知
            {
                Debug.Log("score is changed" + value);
                totalScore = value;
                OnTotalScoreChanged?.Invoke(totalScore); // 触发事件
            }
        }
    }

    // 重置游戏方法
    public void ResetGame()
    {
        Debug.Log("Resetting game...");

        // 清空所有得分
        Bear = 0;
        Deer = 0;
        Wolf = 0;
        Board = 0;
        Rabbit = 0;
        TotalScore = 0;

   


        // 通知所有分数已更新
        OnTotalScoreChanged?.Invoke(TotalScore);
        OnRabbitScoreChanged?.Invoke(_rabbit);
        OnBoardScoreChanged?.Invoke(_board);
        OnDeerScoreChanged?.Invoke(_deer); 
        OnWolfScoreChanged?.Invoke(_wolf); 
        OnBearScoreChanged?.Invoke(_bear); 
    }

    // 私有构造函数，防止外部实例化
    private ScoreManager() { }
}


