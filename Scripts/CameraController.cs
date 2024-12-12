using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float mouseX, mouseY;
    public float mouseSensitivity;
    public float xRotation = 0;
    public Transform player;
    private float recoverySpeed = 5f; // �ָ��ٶȣ����Ե������ֵ

    // ����
    public float amplitude = 0.08f; // ҡ�η��� 
    public float frequency = 6.5f;   // ҡ��Ƶ��
    public float runMultiplier = 1.5f; // ���ܼ���ϵ��

    private float shakeTime = 0f;  // ��ǰʱ��
    private Vector3 originalPosition; // �������ԭʼλ��

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
        // ����ҡ�κ���
            ApplyCameraShake(isWalking, isRunning);
    }

    void ApplyCameraShake(bool isWalking, bool isRunning)
    {
        if (isWalking)
        {
            // ����Ƶ�ʺͷ���
            float currentAmplitude = amplitude * (isRunning ? runMultiplier : 1f);
            float currentFrequency = frequency * (isRunning ? runMultiplier : 1f);

            // ����ҡ��
            shakeTime += Time.deltaTime * currentFrequency;
            float offsetX = Mathf.Sin(shakeTime) * currentAmplitude; // ����ҡ��
            float offsetY = Mathf.Cos(shakeTime * 2) * currentAmplitude * 0.5f; // ����ҡ��
            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
        }
        else
        {
            // ƽ���ָ�ԭλ
            shakeTime = Mathf.Lerp(shakeTime, 0, Time.deltaTime * recoverySpeed); // ƽ������Ƶ��
            Vector3 currentPosition = transform.localPosition;

            // �𲽿���ԭʼλ��
            transform.localPosition = Vector3.Lerp(currentPosition, originalPosition, Time.deltaTime * recoverySpeed);
        }
    }
}
