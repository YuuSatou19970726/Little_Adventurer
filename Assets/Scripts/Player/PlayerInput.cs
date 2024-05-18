using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector]
    public float horizontalInput;
    [HideInInspector]
    public float verticalInput;
    [HideInInspector]
    public bool MouseButtonDown;

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (!MouseButtonDown && Time.timeScale != 0f)
        {
            MouseButtonDown = Input.GetMouseButtonDown(0);
        }

        horizontalInput = Input.GetAxisRaw(Axis.HORIZONTAL);
        verticalInput = Input.GetAxisRaw(Axis.VERTICAL);
    }

    void OnDisable()
    {
        MouseButtonDown = false;
        horizontalInput = 0f;
        verticalInput = 0f;
    }
}
