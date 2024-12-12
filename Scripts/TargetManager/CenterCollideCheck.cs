using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCollisionCheck : MonoBehaviour
{
    // Start is called before the first frame update
    private bool HitCenter { get; set; }
    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log("cylinder is collided");
        if  (collision.gameObject.CompareTag("Arrow")) {
            HitCenter = true;
        }
    }

    // 公共的 getter 方法
    public bool IsHitCenter()
    {
        return HitCenter;
    }
}
