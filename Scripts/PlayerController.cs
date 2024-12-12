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

    // �Ų���Ч
    public AudioClip footstepSound;
    private AudioSource audioSource;
    private float stepInterval = 0.5f; // ÿ��֮���ʱ����
    private float stepTimer;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        audioSource = gameObject.AddComponent<AudioSource>(); // �����Ƶ���������
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
        // ����Ƿ��� Z ��
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ResetGame();
        }

        // �л��ܲ��������ٶ�
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

        // �Ų��������߼�
        if (isGround && (horizontalMove != 0 || verticalMove != 0))
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval) // ��������������
            {
                PlayFootstepSound();
                stepTimer = 0f; // ���ü�ʱ��
            }
        }
        else
        {
            stepTimer = 0f; // ���δ�ƶ����ڵ��棬���ü�ʱ��
        }

        // ��Ծ�������߼�
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
            audioSource.PlayOneShot(footstepSound); // ���ŽŲ���Ч
        }
    }

    public bool IsGrounded
    {
        get { return isGround; }
        set { }
    }

    public void ResetGame()
    {
        // ���÷���
        Debug.Log("reset");
        ScoreManager.Instance.ResetGame();

        // �ҵ����������й���ʵ��
        Arrow[] arrows = FindObjectsOfType<Arrow>();
        foreach (Arrow arrow in arrows)
        {
            Destroy(arrow.gameObject);
        }

        Debug.Log("All arrows destroyed and game reset.");
    }
}
