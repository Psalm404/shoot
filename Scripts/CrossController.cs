using System;
using System.Collections;
using UnityEngine;

public class CrossController : MonoBehaviour
{
    private Animator animator; // Animator 组件
    public GameObject arrowPrefab; // 箭的预制体
    public Transform spawnPoint; // 箭的生成位置
    public float force = 1500f; // 施加在箭上的力（单位为 N）

    public Camera mainCamera; // 摄像机
    public float zoomFOV = 40f; // 拉弓时的视野大小
    public float normalFOV = 60f; // 默认视野大小
    public float zoomSpeed = 0.2f; // 视野变化速度

    private float charge_time;
    private bool isHolding;

    private void Start()
    {
        // 初始化摄像机
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is not assigned and no main camera found in the scene!");
        }

        // 初始化 Animator 和其他参数
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
        // 检测鼠标左键按下（拉弓开始）
        if (Input.GetMouseButtonDown(0)) // 0 表示鼠标左键
        {
            charge_time = Time.time;
            animator.SetBool("Fire", true);
            isHolding = true;
            animator.SetBool("Holding", true);

            // 切换到放大视角
            if (mainCamera != null)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeCameraFOV(zoomFOV));
            }
        }

        // 检测鼠标右键按下（拉弓释放）
        if (Input.GetMouseButtonDown(1)) // 1 表示鼠标右键
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

                // 恢复正常视角
                if (mainCamera != null)
                {
                    StopAllCoroutines();
                    StartCoroutine(ChangeCameraFOV(normalFOV));
                }

                // 启动协程，延迟 0.2 秒生成弓箭
                if (arrowPrefab != null && spawnPoint != null)
                {
                    StartCoroutine(InitializeArrowAfterDelay(0.2f));
                }
            }
        }
    }

    // 修改摄像机视野的协程
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
            // 平滑过渡 FOV
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime / zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.fieldOfView = targetFOV;
    }

    // 初始化箭的协程
    private IEnumerator InitializeArrowAfterDelay(float delay)
    {
        // 等待指定的时间
        yield return new WaitForSeconds(delay);

        // 在指定位置生成箭
        GameObject newArrow = Instantiate(arrowPrefab, spawnPoint.position, spawnPoint.rotation, null);
        Debug.Log("rotation:" + spawnPoint.rotation + newArrow.transform.rotation);

        // 获取箭的刚体组件并施加力
        Rigidbody rb = newArrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 施加力
            rb.AddForce(rb.transform.forward * force);
        }
        else
        {
            Debug.LogError("New arrow does not have a Rigidbody!");
        }
    }
}