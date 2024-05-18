using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    private CharacterController _characterController;

    [SerializeField]
    private float moveSpeed = 5f;

    private Vector3 _movementVelocity;
    private PlayerInput _playerInput;

    private float _verticalVelocity;
    public float Gravity = -9.8f;

    private Animator _animator;


    //enemy
    [SerializeField]
    private bool isPlayer = true;

    private NavMeshAgent _navMeshAgent;
    private Transform targetPlayer;

    //state machine
    public enum CharacterState
    {
        NORMAL,
        ATTACKING
    }
    public CharacterState currentState;

    //player slides
    private float attackStartTime;
    private float attackSlideDuration = 0.4f;
    private float attackSlideSpeed = 1.5f;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _animator = GetComponent<Animator>();

        if (!isPlayer)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            targetPlayer = GameObject.FindWithTag(Tags.PLAYER).transform;
            _navMeshAgent.speed = moveSpeed;
        }
        else
        {
            _playerInput = GetComponent<PlayerInput>();
        }
    }

    void CalculatePlayerMovement()
    {
        if (_playerInput.MouseButtonDown && _characterController.isGrounded)
        {
            SwitchStateTo(CharacterState.ATTACKING);
            return;
        }

        _movementVelocity.Set(_playerInput.horizontalInput, 0f, _playerInput.verticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0, -45f, 0) * _movementVelocity;

        _animator.SetFloat(AnimationTags.SPEED_FLOAT, _movementVelocity.magnitude);

        _movementVelocity *= moveSpeed * Time.deltaTime;

        if (_movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementVelocity);

        _animator.SetBool(AnimationTags.AIRBORNE_BOLEAN, !_characterController.isGrounded);
    }

    void CalculateEnemyMovement()
    {
        if (Vector3.Distance(targetPlayer.position, transform.position) >= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.SetDestination(targetPlayer.position);
            _animator.SetFloat(AnimationTags.SPEED_FLOAT, 0.2f);
        }
        else
        {
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetFloat(AnimationTags.SPEED_FLOAT, 0f);
        }
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case CharacterState.NORMAL:
                if (isPlayer)
                    CalculatePlayerMovement();
                else
                    CalculateEnemyMovement();
                break;
            case CharacterState.ATTACKING:
                if (isPlayer)
                {
                    _movementVelocity = Vector3.zero;

                    if (Time.time < attackStartTime + attackSlideDuration)
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / attackSlideDuration;
                        _movementVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero, lerpTime);
                    }
                }
                break;
        }

        if (isPlayer)
        {
            if (_characterController.isGrounded == false)
                _verticalVelocity = Gravity;
            else
                _verticalVelocity = Gravity * 0.3f;

            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;

            _characterController.Move(_movementVelocity);
        }
    }

    private void SwitchStateTo(CharacterState characterState)
    {
        //clear cache
        _playerInput.MouseButtonDown = false;

        //exiting state
        switch (currentState)
        {
            case CharacterState.NORMAL:
                break;
            case CharacterState.ATTACKING:
                break;
        }

        //entering state
        switch (characterState)
        {
            case CharacterState.NORMAL:
                break;
            case CharacterState.ATTACKING:
                _animator.SetTrigger(AnimationTags.ATTACK_TRIGGER);
                if (isPlayer)
                    attackStartTime = Time.time;
                break;
        }

        currentState = characterState;
    }

    void AttackAnimationEnds()
    {
        SwitchStateTo(CharacterState.NORMAL);
    }
}
