using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector]
    public float horizontalInput;
    [HideInInspector]
    public float verticalInput;

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        horizontalInput = Input.GetAxisRaw(Axis.HORIZONTAL);
        verticalInput = Input.GetAxisRaw(Axis.VERTICAL);
    }
}
