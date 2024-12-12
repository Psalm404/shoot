using System;
using System.Collections;
using UnityEngine;

public class CrossController : MonoBehaviour
{
    private Animator animator; // Animator ���
    public GameObject arrowPrefab; // ����Ԥ����
    public Transform spawnPoint; // ��������λ��
    public float force = 1500f; // ʩ���ڼ��ϵ�������λΪ N��

    public Camera mainCamera; // �����
    public float zoomFOV = 40f; // ����ʱ����Ұ��С
    public float normalFOV = 60f; // Ĭ����Ұ��С
    public float zoomSpeed = 0.2f; // ��Ұ�仯�ٶ�

    private float charge_time;
    private bool isHolding;

    private void Start()
    {
        // ��ʼ�������
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is not assigned and no main camera found in the scene!");
        }

        // ��ʼ�� Animator ����������
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found! Please attach this script to an object with an Animator.");
        }

        if (arrowPrefab == null)
        {
            Debug.LogError("Arrow Prefab is not assigned! Please assign it in the inspector.");
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Spawn Point is not assigned! Please assign it in the inspector.");
        }
    }

    private void Update()
    {
        // ������������£�������ʼ��
        if (Input.GetMouseButtonDown(0)) // 0 ��ʾ������
        {
            charge_time = Time.time;
            animator.SetBool("Fire", true);
            isHolding = true;
            animator.SetBool("Holding", true);

            // �л����Ŵ��ӽ�
            if (mainCamera != null)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeCameraFOV(zoomFOV));
            }
        }

        // �������Ҽ����£������ͷţ�
        if (Input.GetMouseButtonDown(1)) // 1 ��ʾ����Ҽ�
        {
            if (!isHolding)
            {
                return;
            }
            if (Time.time - charge_time > 0.3f)
            {
                isHolding = false;
                animator.SetBool("Holding", false);
                animator.SetBool("Fire", false);

                // �ָ������ӽ�
                if (mainCamera != null)
                {
                    StopAllCoroutines();
                    StartCoroutine(ChangeCameraFOV(normalFOV));
                }

                // ����Э�̣��ӳ� 0.2 �����ɹ���
                if (arrowPrefab != null && spawnPoint != null)
                {
                    StartCoroutine(InitializeArrowAfterDelay(0.2f));
                }
            }
        }
    }

    // �޸��������Ұ��Э��
    private IEnumerator ChangeCameraFOV(float targetFOV)
    {
        if (mainCamera == null)
        {
            yield break;
        }

        float startFOV = mainCamera.fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < zoomSpeed)
        {
            // ƽ������ FOV
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime / zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.fieldOfView = targetFOV;
    }

    // ��ʼ������Э��
    private IEnumerator InitializeArrowAfterDelay(float delay)
    {
        // �ȴ�ָ����ʱ��
        yield return new WaitForSeconds(delay);

        // ��ָ��λ�����ɼ�
        GameObject newArrow = Instantiate(arrowPrefab, spawnPoint.position, spawnPoint.rotation, null);
        Debug.Log("rotation:" + spawnPoint.rotation + newArrow.transform.rotation);

        // ��ȡ���ĸ��������ʩ����
        Rigidbody rb = newArrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // ʩ����
            rb.AddForce(rb.transform.forward * force);
        }
        else
        {
            Debug.LogError("New arrow does not have a Rigidbody!");
        }
    }
}