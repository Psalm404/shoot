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
        // ��ʼ����Ч������
        audioSource = gameObject.AddComponent<AudioSource>();

        // �� Resources �ļ��м�����Ч
        shootSound = Resources.Load<AudioClip>("Audio/���а���"); // "Audio" �� Resources �µ����ļ���
        if (shootSound == null)
        {
            Debug.LogError("Shoot sound not found in Resources/Audio!");
        }

        // ���� Canvas �µ� comment ����
        GameObject commentObject = GameObject.Find("Canvas/comment");

        // ��ȡ comment ����� Text ���
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

                // ���������Ч
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
                    StartCoroutine(HideCommentAfterDelay(2f)); // ��ʾ���� 2 �����������
                }
                else
                {
                    score += GetScore();
                    Debug.Log("HitBoard! Score: " + score);
                    ScoreManager.Instance.TotalScore += score;
                    Debug.Log(ScoreManager.Instance.totalScore);
                    comment.text = "Good!";
                    StartCoroutine(HideCommentAfterDelay(2f)); // ��ʾ���� 2 �����������
                }
            }
            isHited = true;
        }
    }

    private IEnumerator HideCommentAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // �ȴ�ָ��ʱ��
        comment.text = ""; // ��������ı�
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) {
            ResetGame();
        
        }
    }


}
