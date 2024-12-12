using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public abstract class Target : MonoBehaviour
{
    public int score = 0;
    public bool isHited = false;
    public int bonusScore = 20;
    public abstract int GetScore();
    public abstract void Hit();

    public abstract void ResetGame();

    public CenterCollisionCheck cc;
    public UI ui;

    private bool isHitCenter = false;
    public Text comment;
    public AudioClip shootSound;
    private AudioSource audioSource;
    

    void Start()
    {
        // 初始化音效播放器
        audioSource = gameObject.AddComponent<AudioSource>();

        // 从 Resources 文件夹加载音效
        shootSound = Resources.Load<AudioClip>("Audio/击中靶子"); // "Audio" 是 Resources 下的子文件夹
        if (shootSound == null)
        {
            Debug.LogError("Shoot sound not found in Resources/Audio!");
        }

        // 查找 Canvas 下的 comment 对象
        GameObject commentObject = GameObject.Find("Canvas/comment");

        // 获取 comment 对象的 Text 组件
        if (commentObject != null)
        {
            comment = commentObject.GetComponent<Text>();
        }
        else
        {
            Debug.LogError("Comment object not found!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (!isHited)
            {
                Hit();

                // 播放射箭音效
                if (shootSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(shootSound);
                }

                if (cc != null && cc.IsHitCenter() && !isHitCenter)
                {
                    score += GetScore()+bonusScore;
                    Debug.Log("HitCenter! Score: " + score);
                    isHitCenter = true;
                    ScoreManager.Instance.TotalScore += score;
                    comment.text = "Perfect!";
                    StartCoroutine(HideCommentAfterDelay(2f)); // 显示并在 2 秒后隐藏评论
                }
                else
                {
                    score += GetScore();
                    Debug.Log("HitBoard! Score: " + score);
                    ScoreManager.Instance.TotalScore += score;
                    Debug.Log(ScoreManager.Instance.totalScore);
                    comment.text = "Good!";
                    StartCoroutine(HideCommentAfterDelay(2f)); // 显示并在 2 秒后隐藏评论
                }
            }
            isHited = true;
        }
    }

    private IEnumerator HideCommentAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定时间
        comment.text = ""; // 清除评论文本
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) {
            ResetGame();
        
        }
    }


}
