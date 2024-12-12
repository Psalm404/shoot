using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Bow;
    public GameObject Cross;
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            index++;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            index--;
        }
        if (index % 2 == 1)
        {
            Bow.SetActive(true);
            Cross.SetActive(false);
        }
        else {
            Bow.SetActive(false);
            Cross.SetActive(true);
        }
    }
   
}
