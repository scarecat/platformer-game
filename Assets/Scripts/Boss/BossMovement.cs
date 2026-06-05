using System;
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

    private SpriteRenderer spriteRenderer;
    private EntityHealth health;

    protected Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<EntityHealth>();
        playerTransform = GameObject.Find("Player").transform;
    }
    
    protected void SpawnProjectile()
    {
        var projectileObj = Instantiate(projectileToSpawn);
        var projectile = projectileObj.GetComponent<Projectile>();
        projectile.direction = GetDirectionToPlayer();

    }

    private Vector3 GetDirectionToPlayer()
    {
        throw new NotImplementedException();
    }

    public enum BossBehaviour
    {
        RunningAtPlayer,
        Wait,
        ProjectileBursts
    } 

    protected BossBehaviour currentBehaviour;
    
    private bool _leftFacing;
    protected bool LeftFacing {
        get => _leftFacing;
        set
        {
            _leftFacing = value;
        }
    } 


    // Update is called once per frame
    void Update()
    {
        
    }
}
