using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    public Material[] mats; // �����պв�������
    private int index = 0;  // ��ǰ��պ�����

    // Start is called before the first frame update
    void Start()
    {
        if (mats.Length > 0)
        {
            RenderSettings.skybox = mats[index]; // ��ʼ�����õ�һ����պ�
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ������ּ� 0 �� 9 ������
        for (int i = 1; i <= 3; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                index = i;
                ChangeSky(); // �л���պ�
            }
        }
    }

    private void ChangeSky()
    {
        if (index >= 1 && index <= mats.Length)
        {
            RenderSettings.skybox = mats[index-1]; // �����µ���պ�
            Debug.Log("Skybox changed to: " + mats[index].name);
        }
    }
}
