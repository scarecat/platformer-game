using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(EntityHealth))]

[RequireComponent(typeof(Animator))]
public class BossMovement : MonoBehaviour
{
    public void Delete()
    {
        Destroy(gameObject);
    }

    private static readonly int DeathHash = Animator.StringToHash("death");
    private static readonly int AttackHash = Animator.StringToHash("attack");
    private static readonly int RunningHash = Animator.StringToHash("running");
    private static readonly int HitHash = Animator.StringToHash("hit");
    [Header("Configuration")]
    [SerializeField] protected float speed = 6.0f;
    [SerializeField] GameObject projectileToSpawn;

    [Header("Behaviour Timing")]
    [SerializeField] protected float runTime = 6.0f;
    [SerializeField] protected float waitTime = 3.0f;
    [SerializeField] protected float projectileBurstsTime = 3.0f;

    [Header("Projectile Burst Settings")]
    [SerializeField] private float timeBetweenBursts = 0.4f;

    private SpriteRenderer spriteRenderer;
    private EntityHealth health;
    private Animator animator;

    private Vector3 stopPosA;
    private Vector3 stopPosB;

    protected Transform playerTransform;

    private float behaviourTimer = 0f;
    private float burstFireTimer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<EntityHealth>();
        animator = GetComponent<Animator>();

        health.OnHealthChanged.AddListener(OnHealthChanged);
        health.OnDeath.AddListener(OnDeath);

        playerTransform = GameObject.Find("Player").transform;

        currentBehaviour = BossBehaviour.RunningAtPlayer;
        behaviourTimer = runTime;

        stopPosA = transform.Find("BossStopPointA").position;
        stopPosB = transform.Find("BossStopPointB").position;
    }

    private void OnDeath()
    {
        animator.SetTrigger(DeathHash);
    }

    private void OnHealthChanged(float health, float maxHealth)
    {
        animator.SetTrigger(HitHash);
        TransitionTo(BossBehaviour.RunningAwayFromPlayer, 2f);
    }

    private bool CloseToStopPoint()
    {
        return
        Mathf.Abs(stopPosA.x - transform.position.x) < 0.1f
        || Mathf.Abs(stopPosB.x - transform.position.x) < 0.1f;
    }

    protected void SpawnProjectile()
    {
        var projectileObj = Instantiate(projectileToSpawn, transform.position, Quaternion.identity);
        var projectile = projectileObj.GetComponent<Projectile>();
        projectile.direction = GetDirectionToPlayer();
    }

    private Vector3 GetDirectionToPlayer()
    {
        return (playerTransform.position - transform.position).normalized;
    }

    public enum BossBehaviour
    {
        RunningAtPlayer,
        Wait,
        ProjectileBursts,
        RunningAwayFromPlayer
    }

    protected BossBehaviour currentBehaviour;

    private bool _leftFacing;
    protected bool LeftFacing
    {
        get => _leftFacing;
        set
        {
            _leftFacing = value;
            spriteRenderer.flipX = _leftFacing;
        }
    }

    void Update()
    {
        if (!health.IsAlive)  return;
        UpdateFacing();

        behaviourTimer -= Time.deltaTime;

        switch (currentBehaviour)
        {
            case BossBehaviour.RunningAtPlayer:
                HandleRunningAtPlayer();
                break;

            case BossBehaviour.RunningAwayFromPlayer:
                HandleRunningAwayFromPlayer();
                break;

            case BossBehaviour.Wait:
                HandleWait();
                break;

            case BossBehaviour.ProjectileBursts:
                HandleProjectileBursts();
                break;
        }
    }


    private void HandleRunningAtPlayer()
    {
        MoveTowardsPlayer();

        if (behaviourTimer <= 0f)
            TransitionTo(BossBehaviour.ProjectileBursts, projectileBurstsTime);
    }
    private void HandleRunningAwayFromPlayer()
    {
        MoveAwayFromPlayer();
        if (behaviourTimer <= 0f)
            TransitionTo(BossBehaviour.ProjectileBursts, projectileBurstsTime);
        else if (CloseToStopPoint())
        {
            TransitionTo(BossBehaviour.ProjectileBursts, 2.0f);
        }
    }

    private void HandleWait()
    {
        if (behaviourTimer <= 0f)
            TransitionTo(BossBehaviour.RunningAtPlayer, runTime);
    }

    private void HandleProjectileBursts()
    {
        burstFireTimer -= Time.deltaTime;

        if (burstFireTimer <= 0f)
        {
            SpawnProjectile();
            burstFireTimer = timeBetweenBursts;
        }

        if (behaviourTimer <= 0f)
            TransitionTo(BossBehaviour.Wait, waitTime);
    }


    private void MoveTowardsPlayer()
    {
        Vector3 direction = GetDirectionToPlayer();
        transform.position += new Vector3(direction.x * speed * Time.deltaTime, 0, 0);
    }
    private void MoveAwayFromPlayer()
    {
        Vector3 direction = GetDirectionToPlayer();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + -direction.x * speed * Time.deltaTime, stopPosA.x, stopPosB.x), transform.position.y, transform.position.z);
    }


    private void UpdateFacing()
    {
        if (playerTransform == null) return;

        switch (currentBehaviour)
        {
            case BossBehaviour.RunningAwayFromPlayer:
                LeftFacing = playerTransform.position.x > transform.position.x;
                return;
            default:
                LeftFacing = playerTransform.position.x < transform.position.x;
                return;
        }
    }

    private void TransitionTo(BossBehaviour next, float duration)
    {
        currentBehaviour = next;
        behaviourTimer = duration;


        animator.SetBool(RunningHash, next == BossBehaviour.RunningAtPlayer || next == BossBehaviour.RunningAwayFromPlayer);


        // Reset burst timer when entering burst phase
        if (next == BossBehaviour.ProjectileBursts)
            animator.SetTrigger(AttackHash);
            burstFireTimer = 0f; // fire immediately on entry
    }
}