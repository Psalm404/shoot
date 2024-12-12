using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody arrow_rigid;
    public float shoot_force;
    public Vector3 last_pos;
    public bool is_hited = false;

    private float freezeTime;

    // ���һ����Ч�ֶ�
    public AudioClip shootSound; // �����Ч
    private AudioSource audioSource; // ��Ч������

    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log("trigger" + collision.gameObject.name);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (arrow_rigid.constraints == RigidbodyConstraints.FreezeAll)
        {
            return;
        }
        if (collision.gameObject.GetComponent<CharacterController>())
        {
            Debug.Log("Return");
            return;
        }
        if (!collision.gameObject.GetComponent<Arrow>())
        {
            transform.parent = collision.transform;
            arrow_rigid.constraints = RigidbodyConstraints.FreezeAll;

            freezeTime = Time.time;
            is_hited = true;
        }
    }

    void Start()
    {
        // ��ʼ����Ч������
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = shootSound;

        // ���������Ч
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }

        last_pos = transform.position;
        arrow_rigid.AddForce(transform.forward * shoot_force);
        Debug.Log(arrow_rigid.velocity);
    }

    void Update()
    {
        if (!is_hited)
        {
            if (transform.position != last_pos)
            {
                transform.forward = transform.position - last_pos;
            }
            last_pos = transform.position;
        }
    }
}
