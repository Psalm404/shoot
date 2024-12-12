using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    public Material[] mats; // 存放天空盒材质数组
    private int index = 0;  // 当前天空盒索引

    // Start is called before the first frame update
    void Start()
    {
        if (mats.Length > 0)
        {
            RenderSettings.skybox = mats[index]; // 初始化设置第一个天空盒
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 检测数字键 0 到 9 的输入
        for (int i = 1; i <= 3; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                index = i;
                ChangeSky(); // 切换天空盒
            }
        }
    }

    private void ChangeSky()
    {
        if (index >= 1 && index <= mats.Length)
        {
            RenderSettings.skybox = mats[index-1]; // 设置新的天空盒
            Debug.Log("Skybox changed to: " + mats[index].name);
        }
    }
}
