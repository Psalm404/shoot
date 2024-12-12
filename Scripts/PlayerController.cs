using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    public float moveSpeed;
    public float jumpSpeed;
    private float horizontalMove, verticalMove;
    private Vector3 dir;

    public float gravity;

    private Vector3 velocity;

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    [SerializeField]
    private bool isGround { get; set; }

    [SerializeField]
    private float runSpeed, walkSpeed;

    // 脚步音效
    public AudioClip footstepSound;
    private AudioSource audioSource;
    private float stepInterval = 0.5f; // 每步之间的时间间隔
    private float stepTimer;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        audioSource = gameObject.AddComponent<AudioSource>(); // 添加音频播放器组件
        audioSource.clip = footstepSound;
        audioSource.loop = false;
     
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player is triggered with " + other.gameObject.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("player is collided with " + collision.gameObject.name);
    }

    private void Update()
    {
        // 检测是否按下 Z 键
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ResetGame();
        }

        // 切换跑步和行走速度
        if (Input.GetKey(KeyCode.LeftShift))
        {
            audioSource.pitch = 3.0f;
            moveSpeed = runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            audioSource.pitch = 1.5f;
            moveSpeed = walkSpeed;
        }

        isGround = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayer);
        horizontalMove = Input.GetAxis("Horizontal") * moveSpeed;
        verticalMove = Input.GetAxis("Vertical") * moveSpeed;

        dir = transform.forward * verticalMove + transform.right * horizontalMove;
        cc.Move(dir * Time.deltaTime);

        // 脚步声播放逻辑
        if (isGround && (horizontalMove != 0 || verticalMove != 0))
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval) // 如果超过步伐间隔
            {
                PlayFootstepSound();
                stepTimer = 0f; // 重置计时器
            }
        }
        else
        {
            stepTimer = 0f; // 如果未移动或不在地面，重置计时器
        }

        // 跳跃和重力逻辑
        if (isGround && velocity.y <= 0)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = jumpSpeed;
        }

        cc.Move(velocity * Time.deltaTime);
    }

    private void PlayFootstepSound()
    {
        if (footstepSound != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(footstepSound); // 播放脚步音效
        }
    }

    public bool IsGrounded
    {
        get { return isGround; }
        set { }
    }

    public void ResetGame()
    {
        // 重置分数
        Debug.Log("reset");
        ScoreManager.Instance.ResetGame();

        // 找到并销毁所有弓箭实例
        Arrow[] arrows = FindObjectsOfType<Arrow>();
        foreach (Arrow arrow in arrows)
        {
            Destroy(arrow.gameObject);
        }

        Debug.Log("All arrows destroyed and game reset.");
    }
}
