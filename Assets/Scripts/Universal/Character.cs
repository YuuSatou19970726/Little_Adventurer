using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

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

    private int Coin;

    //enemy
    [SerializeField]
    private bool isPlayer = true;

    private NavMeshAgent _navMeshAgent;
    private Transform targetPlayer;

    //state machine
    public enum CharacterState
    {
        NORMAL,
        ATTACKING,
        DEAD,
        BEINGHIT
    }
    public CharacterState currentState;

    //player slides
    private float attackStartTime;
    private float attackSlideDuration = 0.4f;
    private float attackSlideSpeed = 0.5f;

    //health
    private Health _health;

    //damage caster
    private DamageCaster _damageCaster;

    //material animation
    private MaterialPropertyBlock _materialPropertyBlock;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    [SerializeField]
    private GameObject ItemToDrop;

    private Vector3 impactOnCharacter;
    private bool IsInvincible;
    private float invincibleDuration = 2f;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _health = GetComponent<Health>();
        _damageCaster = GetComponentInChildren<DamageCaster>();

        _animator = GetComponent<Animator>();

        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        _skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);

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

            SwitchStateTo(CharacterState.ATTACKING);
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
                    // _movementVelocity = Vector3.zero;

                    if (Time.time < attackStartTime + attackSlideDuration)
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / attackSlideDuration;
                        _movementVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero, lerpTime);
                    }
                }
                break;
            case CharacterState.DEAD:
                return;
            case CharacterState.BEINGHIT:
                if (impactOnCharacter.magnitude > 0.2f)
                    _movementVelocity = impactOnCharacter * Time.deltaTime;

                impactOnCharacter = Vector3.Lerp(impactOnCharacter, Vector3.zero, Time.deltaTime * 5);
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
            _movementVelocity = Vector3.zero;
        }
    }

    public void SwitchStateTo(CharacterState characterState)
    {
        //clear cache
        if (isPlayer)
            _playerInput.MouseButtonDown = false;

        //exiting state
        switch (currentState)
        {
            case CharacterState.NORMAL:
                break;
            case CharacterState.ATTACKING:
                if (_damageCaster != null)
                    DisableDamageCaster();
                break;
            case CharacterState.DEAD:
                return;
            case CharacterState.BEINGHIT:
                break;
        }

        //entering state
        switch (characterState)
        {
            case CharacterState.NORMAL:
                break;
            case CharacterState.ATTACKING:
                if (!isPlayer)
                {
                    Quaternion newRotation = Quaternion.LookRotation(targetPlayer.position - transform.position);
                    transform.rotation = newRotation;
                }

                _animator.SetTrigger(AnimationTags.ATTACK_TRIGGER);
                if (isPlayer)
                    attackStartTime = Time.time;
                break;
            case CharacterState.DEAD:
                _characterController.enabled = false;
                _animator.SetTrigger(AnimationTags.DEAD_TRIGGER);
                StartCoroutine(MaterialDissolve());
                break;
            case CharacterState.BEINGHIT:
                _animator.SetTrigger(AnimationTags.BEING_HIT_TRIGGER);

                if (isPlayer)
                {
                    IsInvincible = true;
                    StartCoroutine(DelayCancelInvincible());
                }
                break;
        }

        currentState = characterState;
    }

    void AttackAnimationEnds()
    {
        SwitchStateTo(CharacterState.NORMAL);
    }

    void BeingHitAnimationEnds()
    {
        SwitchStateTo(CharacterState.NORMAL);
    }

    public void ApplyDamage(int damage, Vector3 attackerPos = new Vector3())
    {
        if (IsInvincible)
            return;

        if (_health != null)
        {
            _health.ApplyDamage(damage);
        }

        if (!isPlayer)
        {
            GetComponent<EnemyVFXManager>().PlayBeingHitVFX(attackerPos);
        }

        StartCoroutine(MaterialBlink());

        if (isPlayer)
        {
            SwitchStateTo(CharacterState.BEINGHIT);
            AddImpact(attackerPos, 10f);
        }
    }

    IEnumerator DelayCancelInvincible()
    {
        yield return new WaitForSeconds(invincibleDuration);
        IsInvincible = false;
    }

    private void AddImpact(Vector3 attackerPos, float force)
    {
        Vector3 impactDir = transform.position - attackerPos;
        impactDir.Normalize();
        impactDir.y = 0;
        impactOnCharacter = impactDir * force;
    }

    public void EnableDamageCaster()
    {
        _damageCaster.EnableDamageCaster();
    }

    public void DisableDamageCaster()
    {
        _damageCaster.DisableDamageCaster();
    }

    IEnumerator MaterialBlink()
    {
        _materialPropertyBlock.SetFloat("_blink", 0.4f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        yield return new WaitForSeconds(0.2f);

        _materialPropertyBlock.SetFloat("_blink", 0f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(2f);

        float dissolveTimeDuration = 2f;
        float currentDissolveTime = 0f;
        float dissolveHeight_start = 20f;
        float dissolveHeight_target = -10f;
        float dissolveHeight;

        if (_materialPropertyBlock != null)
        {
            _materialPropertyBlock.SetFloat("_enableDissolve", 1f);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

            while (currentDissolveTime < dissolveTimeDuration)
            {
                currentDissolveTime += Time.deltaTime;
                dissolveHeight = Mathf.Lerp(dissolveHeight_start, dissolveHeight_target, currentDissolveTime / dissolveTimeDuration);
                _materialPropertyBlock.SetFloat("_dissolve_height", dissolveHeight);
                _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
                yield return null;
            }
        }

        DropItem();
    }

    public void DropItem()
    {
        if (ItemToDrop != null)
            Instantiate(ItemToDrop, transform.position, Quaternion.identity);
    }

    public void PickUpItem(PickUp item)
    {
        switch (item.Type)
        {
            case PickUp.PickUpType.HEAL:
                AddHealth(item.value);
                break;
            case PickUp.PickUpType.COIN:
                AddCoin(item.value);
                break;
        }
    }

    private void AddHealth(int health)
    {
        _health.AddHealth(health);
        GetComponent<PlayerVFXManager>().PlayHealth();
    }

    private void AddCoin(int coint)
    {
        Coin += coint;
    }
}
