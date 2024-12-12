using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform player; // ��Ҷ���
    public List<GameObject> waypoints; // ·���ϵĹյ�
    public List<GameObject> stoppoints; // ͣ����
    public GameObject start;
    private List<Vector3> waypoints_pos = new List<Vector3>();
    private List<Vector3> stoppoints_pos = new List<Vector3>();
    public PlayerController playerController;
    public float speed = 4f; // ��ʼ�ƶ��ٶ�
    public float rotationSpeed = 4f; // ��ת�ٶ�
    public float stopDuration = 5f; // ��ͣ����ͣ����ʱ��
    public float acceleration = 1.5f; // ���ٶ�
    public float deceleration = 1.5f; // ���ٶ�

    private bool isPlayerInCar = false; // ����Ƿ��ڳ���
    private bool isStopping = false; // ��ǰ�Ƿ���ͣ����ͣ��
    private bool isAccelerating = false; // �Ƿ��ڼ���
    private bool isStopped = false;
    private int currentWaypointIndex = 0; // ��ǰĿ��յ�����
    private float currentSpeed; // ��ǰ�ٶ�
    private float stopTimer = 0f; // ͣ����ʱ��
    private bool isReach;
    private bool isBacking;
    private bool isBack = true;
    public float backSpeed = 20f;

    private void Start()
    {
        currentSpeed = speed;

        // ��ʼ��·����λ��
        foreach (GameObject wp in waypoints)
        {
            waypoints_pos.Add(wp.transform.position);
        }

        // ��ʼ��ͣ����λ��
        foreach (GameObject sp in stoppoints)
        {
            stoppoints_pos.Add(sp.transform.position);
        }
    }

    void Update()
    {
        if (isPlayerInCar)
        {
            player.position = transform.Find("seat").position;
        }

        // Debug.Log("isback:" + isBack + "isbacking" + isBacking);
        if (isPlayerInCar)
        {
            //player.rotation = Quaternion.Euler(0, 0, 0);
            if (!isStopped)
            {
                MoveAlongWaypoints();
            }
            if (isStopping)
            {
                HandleStop(); // ͣ���߼�
            }
            if (isAccelerating && !isStopped)
            {
                HandleAcceleration(); // �����߼�
            }
        }

        if (isReach == true && isPlayerInCar && Input.GetKeyDown(KeyCode.E))
        {
            ExitCar();

        }

        if (isBacking)
        {
            MoveBackAlongWaypoints();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player in range! Press E to enter the car.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !isBacking && isBack)
        {
            EnterCar(); // ��Ұ���E�����복��
        }
    }

    // ����ϳ�
    private void EnterCar()
    {
        Debug.Log("Player entered the car!");
        isPlayerInCar = true;
        isBack = false;
        player.GetComponent<CharacterController>().enabled = false;
        // player.SetParent(this.transform, true); // ����Ұ󶨵�����
        Transform driverSeat = transform.Find("seat");
        if (driverSeat != null)
        {
            player.position = driverSeat.position;
        }
        playerController.ResetGame();

    }

    private void ExitCar()
    {
        Debug.Log("Player exit the car!");
        isPlayerInCar = false;
        player.SetParent(null); // ����Ұ󶨵�����
        player.GetComponent<CharacterController>().enabled = true;
        Transform driverSeat = transform.Find("seat");
        if (driverSeat != null)
        {
            player.position = driverSeat.position;
        }
        isBacking = true;
        isReach = false;
        playerController.ResetGame();


    }

    // �����عյ��ƶ�
    private void MoveAlongWaypoints()
    {
        Debug.Log("move with speed " + currentSpeed);
        if (currentWaypointIndex >= waypoints.Count)
        {
            currentWaypointIndex = waypoints.Count - 1;
            isReach = true;
            Debug.Log("Reached the final waypoint!");
            return;
        }

        Vector3 targetPosition;
        // ��ǰĿ��λ��
        targetPosition = waypoints_pos[currentWaypointIndex];


        // ����Ƿ�ӽ�ͣ����
        if (IsNearStopPoint(transform.position, stoppoints_pos) && !isStopped && !isAccelerating && !isStopping)
        {
            StartStop(); // ����ͣ���߼�
            return;
        }

        // �ƶ���Ŀ���
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

        // ��ת��ͷ����Ŀ��
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        // ����Ƿ񵽴�Ŀ���
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex++; // �л�����һ��Ŀ���
        }
    }

    private void MoveBackAlongWaypoints()
    {
        Debug.Log("move with speed " + backSpeed);
        Debug.Log(currentWaypointIndex);
        if (currentWaypointIndex <= 0)
        {
            currentWaypointIndex = 0;
            isBack = true;
            isBacking = false;
            Debug.Log("Reached the first waypoint!");
            transform.position = start.transform.position;
            transform.rotation = start.transform.rotation;
            return;
        }

        // ��ǰĿ��λ��
        Vector3 targetPosition = waypoints_pos[currentWaypointIndex - 1];

        // �ƶ���Ŀ���
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, backSpeed * Time.deltaTime);

        // ��ת��ͷ����Ŀ��
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // ����Ƿ񵽴�Ŀ���
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex--; // �л�����һ��Ŀ���
        }
    }


    // ����Ƿ�ӽ�ͣ����
    private bool IsNearStopPoint(Vector3 position, List<Vector3> stopPoints)
    {
        foreach (Vector3 stopPoint in stopPoints)
        {
            if (Vector3.Distance(position, stopPoint) < 1f) // �ӽ�������ֵ
            {
                return true;
            }
        }
        return false;
    }

    // ����ͣ���߼�
    private void StartStop()
    {
        Debug.Log("Stopping at stop point...");
        isStopping = true; // ���Ϊͣ��
    }

    // ͣ���߼�
    private void HandleStop()
    {
        if (!isStopped)
        {
            // ���ٹ���
            currentSpeed -= deceleration * Time.deltaTime;
            Debug.Log($"Stop {currentSpeed}");

            if (currentSpeed <= 0.2)
            {
                currentSpeed = 0; // ֹͣ�ƶ�
                isStopped = true;

            }
        }
        else
        {
            // ��ʼ��ʱ���ȴ�ͣ��
            stopTimer += Time.deltaTime;
            Debug.Log(stopTimer);
            if (stopTimer >= stopDuration)
            {
                stopTimer = 0f;
                isStopping = false; // ͣ������
                isAccelerating = true; // ��ʼ����
                isStopped = false;
            }
        }
    }

    // �����߼�
    private void HandleAcceleration()
    {
        // ���ٹ���
        currentSpeed += acceleration * Time.deltaTime;
        Debug.Log($"Acceleration: {currentSpeed}");
        if (currentSpeed >= speed)
        {
            currentSpeed = speed; // �ﵽ����ٶ�
            isAccelerating = false; // ���ٽ���
        }
    }
}