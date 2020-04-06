using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public bool lockCursor = false;

    // Start is called before the first frame update
    void Start()
    {
        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
    }
}