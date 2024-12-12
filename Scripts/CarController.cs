using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform player; // 玩家对象
    public List<GameObject> waypoints; // 路线上的拐点
    public List<GameObject> stoppoints; // 停车点
    public GameObject start;
    private List<Vector3> waypoints_pos = new List<Vector3>();
    private List<Vector3> stoppoints_pos = new List<Vector3>();
    public PlayerController playerController;
    public float speed = 4f; // 初始移动速度
    public float rotationSpeed = 4f; // 旋转速度
    public float stopDuration = 5f; // 在停车点停留的时间
    public float acceleration = 1.5f; // 加速度
    public float deceleration = 1.5f; // 减速度

    private bool isPlayerInCar = false; // 玩家是否在车上
    private bool isStopping = false; // 当前是否在停车点停留
    private bool isAccelerating = false; // 是否在加速
    private bool isStopped = false;
    private int currentWaypointIndex = 0; // 当前目标拐点索引
    private float currentSpeed; // 当前速度
    private float stopTimer = 0f; // 停留计时器
    private bool isReach;
    private bool isBacking;
    private bool isBack = true;
    public float backSpeed = 20f;

    private void Start()
    {
        currentSpeed = speed;

        // 初始化路径点位置
        foreach (GameObject wp in waypoints)
        {
            waypoints_pos.Add(wp.transform.position);
        }

        // 初始化停车点位置
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
                HandleStop(); // 停车逻辑
            }
            if (isAccelerating && !isStopped)
            {
                HandleAcceleration(); // 加速逻辑
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
            EnterCar(); // 玩家按下E键进入车辆
        }
    }

    // 玩家上车
    private void EnterCar()
    {
        Debug.Log("Player entered the car!");
        isPlayerInCar = true;
        isBack = false;
        player.GetComponent<CharacterController>().enabled = false;
        // player.SetParent(this.transform, true); // 将玩家绑定到车辆
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
        player.SetParent(null); // 将玩家绑定到车辆
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

    // 车辆沿拐点移动
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
        // 当前目标位置
        targetPosition = waypoints_pos[currentWaypointIndex];


        // 检查是否接近停车点
        if (IsNearStopPoint(transform.position, stoppoints_pos) && !isStopped && !isAccelerating && !isStopping)
        {
            StartStop(); // 触发停车逻辑
            return;
        }

        // 移动到目标点
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

        // 旋转车头朝向目标
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        // 检查是否到达目标点
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex++; // 切换到下一个目标点
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

        // 当前目标位置
        Vector3 targetPosition = waypoints_pos[currentWaypointIndex - 1];

        // 移动到目标点
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, backSpeed * Time.deltaTime);

        // 旋转车头朝向目标
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 检查是否到达目标点
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex--; // 切换到下一个目标点
        }
    }


    // 检测是否接近停车点
    private bool IsNearStopPoint(Vector3 position, List<Vector3> stopPoints)
    {
        foreach (Vector3 stopPoint in stopPoints)
        {
            if (Vector3.Distance(position, stopPoint) < 1f) // 接近距离阈值
            {
                return true;
            }
        }
        return false;
    }

    // 启动停车逻辑
    private void StartStop()
    {
        Debug.Log("Stopping at stop point...");
        isStopping = true; // 标记为停车
    }

    // 停车逻辑
    private void HandleStop()
    {
        if (!isStopped)
        {
            // 减速过程
            currentSpeed -= deceleration * Time.deltaTime;
            Debug.Log($"Stop {currentSpeed}");

            if (currentSpeed <= 0.2)
            {
                currentSpeed = 0; // 停止移动
                isStopped = true;

            }
        }
        else
        {
            // 开始计时，等待停车
            stopTimer += Time.deltaTime;
            Debug.Log(stopTimer);
            if (stopTimer >= stopDuration)
            {
                stopTimer = 0f;
                isStopping = false; // 停车结束
                isAccelerating = true; // 开始加速
                isStopped = false;
            }
        }
    }

    // 加速逻辑
    private void HandleAcceleration()
    {
        // 加速过程
        currentSpeed += acceleration * Time.deltaTime;
        Debug.Log($"Acceleration: {currentSpeed}");
        if (currentSpeed >= speed)
        {
            currentSpeed = speed; // 达到最大速度
            isAccelerating = false; // 加速结束
        }
    }
}