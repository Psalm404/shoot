
using UnityEngine;
public class ScoreManager
{
    private static ScoreManager _instance;
    BearTarget _bearTarget;
    MilitaryTarget _militaryTarget;
    WolfTarget _wolfTarget;
    DeerTarget _deerTarget;
    public int totalScore;

    // ����������
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


    // �¼�֪ͨ UI ��
    public event System.Action<int> OnBearScoreChanged;

    private int _bear;
    public int Bear
    {
        get { return _bear; }
        set
        {
            if (_bear != value) // ����ֵ�����仯ʱ֪ͨ
            {
                _bear = value;
                OnBearScoreChanged?.Invoke(_bear); // �����¼�
            }
        }
    }

    // �¼�֪ͨ UI ��
    public event System.Action<int> OnDeerScoreChanged;

    private int _deer;
    public int Deer
    {
        get { return _deer; }
        set
        {
            if (_deer != value) // ����ֵ�����仯ʱ֪ͨ
            {
                _deer = value;
                OnDeerScoreChanged?.Invoke(_deer); // �����¼�
            }
        }
    }

    // �¼�֪ͨ UI ��
    public event System.Action<int> OnWolfScoreChanged;

    private int _wolf;
    public int Wolf
    {
        get { return _wolf; }
        set
        {
            if (_wolf != value) // ����ֵ�����仯ʱ֪ͨ
            {
                _wolf = value;
                OnWolfScoreChanged?.Invoke(_wolf); // �����¼�
            }
        }
    }

    // �¼�֪ͨ UI ��
    public event System.Action<int> OnBoardScoreChanged;

    private int _board;
    public int Board
    {
        get { return _board; }
        set
        {
            if (_board != value) // ����ֵ�����仯ʱ֪ͨ
            {
                Debug.Log("board is hitted");
                _board = value;
                OnBoardScoreChanged?.Invoke(_board); // �����¼�
            }
        }
    }

    // �¼�֪ͨ UI ��
    public event System.Action<int> OnRabbitScoreChanged;

    private int _rabbit;
    public int Rabbit
    {
        get { return _rabbit; }
        set
        {
            if (_rabbit != value) // ����ֵ�����仯ʱ֪ͨ
            {
                Debug.Log("rabbit is hited" + value);
                _rabbit = value;
                OnRabbitScoreChanged?.Invoke(_rabbit); // �����¼�
            }
        }
    }

    public event System.Action<int> OnTotalScoreChanged;

    public int TotalScore
    {
        get { return totalScore; }
        set
        {
            if (totalScore != value) // ����ֵ�����仯ʱ֪ͨ
            {
                Debug.Log("score is changed" + value);
                totalScore = value;
                OnTotalScoreChanged?.Invoke(totalScore); // �����¼�
            }
        }
    }

    // ������Ϸ����
    public void ResetGame()
    {
        Debug.Log("Resetting game...");

        // ������е÷�
        Bear = 0;
        Deer = 0;
        Wolf = 0;
        Board = 0;
        Rabbit = 0;
        TotalScore = 0;

   


        // ֪ͨ���з����Ѹ���
        OnTotalScoreChanged?.Invoke(TotalScore);
        OnRabbitScoreChanged?.Invoke(_rabbit);
        OnBoardScoreChanged?.Invoke(_board);
        OnDeerScoreChanged?.Invoke(_deer); 
        OnWolfScoreChanged?.Invoke(_wolf); 
        OnBearScoreChanged?.Invoke(_bear); 
    }

    // ˽�й��캯������ֹ�ⲿʵ����
    private ScoreManager() { }
}


