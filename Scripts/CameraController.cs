using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float mouseX, mouseY;
    public float mouseSensitivity;
    public float xRotation = 0;
    public Transform player;
    private float recoverySpeed = 5f; // 恢复速度，可以调整这个值

    // 参数
    public float amplitude = 0.08f; // 摇晃幅度 
    public float frequency = 6.5f;   // 摇晃频率
    public float runMultiplier = 1.5f; // 奔跑加速系数

    private float shakeTime = 0f;  // 当前时间
    private Vector3 originalPosition; // 摄像机的原始位置

    public void Start()
    {
        originalPosition = transform.localPosition;
    }
    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY     ;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);
          player.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);


        bool isWalking = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        PlayerController pp = GameObject.Find("Player").GetComponent<PlayerController>();
        if (pp.IsGrounded)
        // 调用摇晃函数
            ApplyCameraShake(isWalking, isRunning);
    }

    void ApplyCameraShake(bool isWalking, bool isRunning)
    {
        if (isWalking)
        {
            // 调整频率和幅度
            float currentAmplitude = amplitude * (isRunning ? runMultiplier : 1f);
            float currentFrequency = frequency * (isRunning ? runMultiplier : 1f);

            // 计算摇晃
            shakeTime += Time.deltaTime * currentFrequency;
            float offsetX = Mathf.Sin(shakeTime) * currentAmplitude; // 左右摇晃
            float offsetY = Mathf.Cos(shakeTime * 2) * currentAmplitude * 0.5f; // 上下摇晃
            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
        }
        else
        {
            // 平滑恢复原位
            shakeTime = Mathf.Lerp(shakeTime, 0, Time.deltaTime * recoverySpeed); // 平滑降低频率
            Vector3 currentPosition = transform.localPosition;

            // 逐步靠近原始位置
            transform.localPosition = Vector3.Lerp(currentPosition, originalPosition, Time.deltaTime * recoverySpeed);
        }
    }
}
