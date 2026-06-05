using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(EntityHealth))]
public class BossMovement : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected float speed = 3.0f;
    [SerializeField] GameObject projectileToSpawn;

    [Header("Behaviour Timing")]
    [SerializeField] protected float runTime = 6.0f;
    [SerializeField] protected float waitTime = 3.0f;
    [SerializeField] protected float projectileBurstsTime = 3.0f;

    [Header("Projectile Burst Settings")]
    [SerializeField] private float timeBetweenBursts = 0.4f;

    private SpriteRenderer spriteRenderer;
    private EntityHealth health;
    protected Transform playerTransform;

    private float behaviourTimer = 0f;
    private float burstFireTimer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<EntityHealth>();
        playerTransform = GameObject.Find("Player").transform;

        currentBehaviour = BossBehaviour.RunningAtPlayer;
        behaviourTimer = runTime;
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
        ProjectileBursts
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
        UpdateFacing();

        behaviourTimer -= Time.deltaTime;

        switch (currentBehaviour)
        {
            case BossBehaviour.RunningAtPlayer:
                HandleRunningAtPlayer();
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

    private void UpdateFacing()
    {
        if (playerTransform == null) return;
        LeftFacing = playerTransform.position.x < transform.position.x;
    }

    private void TransitionTo(BossBehaviour next, float duration)
    {
        currentBehaviour = next;
        behaviourTimer = duration;

        // Reset burst timer when entering burst phase
        if (next == BossBehaviour.ProjectileBursts)
            burstFireTimer = 0f; // fire immediately on entry
    }
}