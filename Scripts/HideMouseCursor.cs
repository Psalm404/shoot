using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMouseCursor : MonoBehaviour
{

    public bool isLocked = true;
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            isLocked = !isLocked;

        }
        Cursor.visible = !isLocked;
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
