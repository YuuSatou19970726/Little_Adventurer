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
    [HideInInspector]
    public bool SpaceKeyDown;

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

        if (!SpaceKeyDown && Time.timeScale != 0f)
        {
            SpaceKeyDown = Input.GetKeyDown(KeyCode.Space);
        }

        horizontalInput = Input.GetAxisRaw(Axis.HORIZONTAL);
        verticalInput = Input.GetAxisRaw(Axis.VERTICAL);
    }

    void OnDisable()
    {
        ClearCache();
    }

    public void ClearCache()
    {
        MouseButtonDown = false;
        SpaceKeyDown = false;
        horizontalInput = 0f;
        verticalInput = 0f;
    }
}
