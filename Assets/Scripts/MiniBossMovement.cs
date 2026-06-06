using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(EntityHealth))]
public class MiniBossMovement : MonoBehaviour
{
    private static readonly int HitHash = Animator.StringToHash("hit");
    private static readonly int WalkingHash = Animator.StringToHash("walking");
    private static readonly int AttackHash = Animator.StringToHash("attack");
    [Header("Configuration")]
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float aggroRange = 6.0f;
    [SerializeField] private float attackRange = 1.2f;

    [Header("Behaviour Timing")]
    [SerializeField] private float attackCooldown = 1.2f;

    private SpriteRenderer spriteRenderer;
    private EntityHealth health;
    private Transform playerTransform;
    private Animator animator;

    private float attackTimer = 0f;

    public enum MiniBossBehaviours
    {
        Idle,
        WalkTowardsPlayer,
        Hit,
        Attack
    }

    private float hitStunTimer = 0f;
    private const float HitStunDuration = 0.5f;

    private MiniBossBehaviours currentBehaviour;


    private bool _leftFacing;

    private void SetLeftFacing(bool value)
    {
        _leftFacing = value;
        spriteRenderer.flipX = _leftFacing;
    }


    public GameObject protectedObject;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<EntityHealth>();
        playerTransform = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        currentBehaviour = MiniBossBehaviours.Idle;
        health.OnHealthChanged.AddListener(OnHit);
        health.OnDeath.AddListener(OnDeath);
        protectedObject.SetActive(false);
    }

    private void OnDeath()
    {
        protectedObject.SetActive(true);
    }

    private void OnHit(float health, float maxHealth)
    {
        animator.SetTrigger(HitHash);
        currentBehaviour = MiniBossBehaviours.Hit;
        hitStunTimer = HitStunDuration;
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        float distanceToPlayer = GetDistanceToPlayer();


        UpdateBehaviour(distanceToPlayer);

        UpdateFacing();

        switch (currentBehaviour)
        {
            case MiniBossBehaviours.Idle:
                HandleIdle();
                break;
            case MiniBossBehaviours.Hit:
                HandleHit();
                break;
            case MiniBossBehaviours.WalkTowardsPlayer:
                HandleWalkTowardsPlayer();
                break;
            case MiniBossBehaviours.Attack:
                //HandleAttack();
                break;
        }
    }

    private void HandleHit()
    {
        hitStunTimer -= Time.deltaTime;
        if (hitStunTimer <= 0f)
            currentBehaviour = MiniBossBehaviours.Idle; // UpdateBehaviour will correct it next frame
    }
    private void UpdateBehaviour(float distanceToPlayer)
    {
        if (currentBehaviour == MiniBossBehaviours.Hit) return;

        if (currentBehaviour == MiniBossBehaviours.Attack)
        {
            if (attackTimer <= 0f)
                currentBehaviour = MiniBossBehaviours.WalkTowardsPlayer;
            return;
        }

        if (distanceToPlayer <= attackRange && attackTimer <= 0f)
        {
            animator.SetTrigger(AttackHash);
            currentBehaviour = MiniBossBehaviours.Attack;
            attackTimer = attackCooldown;  // arm here, not in HandleAttack
            animator.SetBool(WalkingHash, false);
            return;
        }

        if (distanceToPlayer <= aggroRange)
            currentBehaviour = MiniBossBehaviours.WalkTowardsPlayer;
        else
            currentBehaviour = MiniBossBehaviours.Idle;

        animator.SetBool(WalkingHash, currentBehaviour == MiniBossBehaviours.WalkTowardsPlayer);
    }

    private void HandleIdle()
    {
    }

    private void HandleWalkTowardsPlayer()
    {
        Vector3 direction = GetDirectionToPlayer();
        transform.position += new Vector3(direction.x * speed * Time.deltaTime, 0, 0);
    }


    private void UpdateFacing()
    {
        if (playerTransform == null) return;
        SetLeftFacing(playerTransform.position.x < transform.position.x);
    }

    private Vector3 GetDirectionToPlayer()
    {
        return (playerTransform.position - transform.position).normalized;
    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, playerTransform.position);
    }
}