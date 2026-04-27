using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class FlyingMonster : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed = 3f;
    private int destinationIndex = 0;

    public float attackRange = 7f;
    public float shotCooldown = 1.5f;
    public GameObject projectilePrefab;

    private Transform player;
    private Animator animator;
    private SpriteRenderer sprite;

    private bool canShoot = true;

    enum MonsterState
    {
        Searching,
        Attacking
    }
    private MonsterState currentState = MonsterState.Searching;

    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case MonsterState.Searching:
                HandleSearching();

                if (distanceToPlayer <= attackRange)
                {
                    currentState = MonsterState.Attacking;
                    animator.SetBool("isAttacking", true);
                }
                break;
            case MonsterState.Attacking:
                HandleAttacking();

                if (distanceToPlayer > attackRange)
                {
                    currentState = MonsterState.Searching;
                    animator.SetBool("isAttacking", false);
                }
                break;
        }
    }

    private void HandleSearching()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[destinationIndex];

        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        bool isMovingLeft = targetPoint.position.x - transform.position.x < 0;
        sprite.flipX = isMovingLeft;

        if (Vector2.Distance(transform.position, targetPoint.position) <= 0.2f)
        {
            destinationIndex = (destinationIndex + 1) % patrolPoints.Length;
        }
    }

    private void HandleAttacking()
    {
        bool isPlayerOnLeft = player.position.x - transform.position.x < 0;
        sprite.flipX = isPlayerOnLeft;

        if (canShoot)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        canShoot = false;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        var projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var projectile = projectileObj.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.direction = directionToPlayer;
        }

        yield return new WaitForSeconds(shotCooldown);
        canShoot = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}