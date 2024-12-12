using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class bow_controller : MonoBehaviour
{
    public Transform shoot_point;
    public Arrow arrow_prefab;
    public float charge_strength;
    public AnimationCurve charge_curve;
    public Vector3 last_mouse_pos;
    public Transform bow;
    public Transform bow_platform;
    public float platform_speed = .2f;
    public float bow_speed = .2f;

    public Transform min_pos;
    public Transform max_pos;
    public Transform string_pos;

    public GameObject bow_model;

    public LineRenderer line_render;
    public Transform point1, point3;

    public LineRenderer trail_line;
    public float shakeAmount = .05f;  // 抖动幅度
    public float shakeSpeed = 0.001f;     // 抖动速度

    // 重力加速度
    public float gravity = -9.81f;
    public float timeStep = .0001f; // 每次更新轨迹点的时间间隔
    private List<Vector3> trajectoryPoints;

    private Vector3 initialPosition;

    private float chargeTime;
    void Start()
    {
        trajectoryPoints = new List<Vector3>();
        initialPosition = bow.position;
    }

  
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            initialPosition = bow.localPosition;
            charge_strength = 0;
            chargeTime = Time.time;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            charge_strength += Time.deltaTime;
            //  SimulateTrajectory(); // 计算轨迹并渲染
            // 使用 PingPong 函数来实现上下抖动
            bow.localPosition = new Vector3(bow.localPosition.x, initialPosition.y + Mathf.PingPong(Time.time * shakeSpeed * 0.25f, shakeAmount*0.3f) - shakeAmount/2,bow.localPosition.z);
        }
        string_pos.position = Vector3.Lerp(min_pos.position, max_pos.position, charge_strength / charge_curve.keys[charge_curve.length - 1].time);
        bow_model.gameObject.SetActive(Input.GetKey(KeyCode.Mouse0));
        line_render.SetPosition(0, point1.position);
        line_render.SetPosition(1, string_pos.position);
        line_render.SetPosition(2, point3.position);

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            float chargeDuration = Time.time - chargeTime;
       
            if (chargeDuration > 0.3f)
            {
                Vector3 a = shoot_point.position;
                Quaternion r = shoot_point.rotation;
    
                Arrow temp = Instantiate(arrow_prefab,shoot_point.position,shoot_point.rotation,null);
             //   temp.transform.localRotation = Quaternion.identity;
                //  Arrow t = Instantiate(arrow_prefab, a, Quaternion.LookRotation(shoot_point.transform.up));
                Debug.Log(shoot_point.rotation.eulerAngles+" "+shoot_point.localRotation.eulerAngles);
                Debug.Log(temp.name + temp.transform.rotation.eulerAngles + " " + temp.transform.localRotation.eulerAngles + temp.transform.parent);
               // temp.transform.SetParent(null);
                //   Time.timeScale = 0;
                temp.shoot_force = charge_curve.Evaluate(charge_strength);
            }
            charge_strength = 0;
            bow.localPosition = initialPosition;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            last_mouse_pos = Input.mousePosition;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            bow_platform.Rotate(Vector3.up * (last_mouse_pos - Input.mousePosition).x * platform_speed);
            bow.Rotate(Vector3.forward * (Input.mousePosition - last_mouse_pos).y * bow_speed);
        }
        last_mouse_pos = Input.mousePosition;
    }
}
