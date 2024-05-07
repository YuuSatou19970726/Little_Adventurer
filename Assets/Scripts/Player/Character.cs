using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController _characterController;

    [SerializeField]
    private float moveSpeed = 5f;

    private Vector3 _movementVelocity;
    private PlayerInput _playerInput;

    private float _verticalVelocity;
    public float Gravity = -9.8f;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    void CalculatePlayerMovement()
    {
        _movementVelocity.Set(_playerInput.horizontalInput, 0f, _playerInput.verticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0, -45f, 0) * _movementVelocity;
        _movementVelocity *= moveSpeed * Time.deltaTime;

        if (_movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
    }

    void FixedUpdate()
    {
        CalculatePlayerMovement();

        if (_characterController.isGrounded == false)
            _verticalVelocity = Gravity;
        else
            _verticalVelocity = Gravity * 0.3f;

        _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;

        _characterController.Move(_movementVelocity);
    }
}
