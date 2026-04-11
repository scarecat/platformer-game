using System;
using System.Collections;
using System.Runtime.Serialization;
using UnityEditor.Rendering;
using UnityEngine;


public class MonsterShooting : MonoBehaviour

{
    [SerializeField]
    private float shotCooldown = 0.25f;

    [SerializeField]
    private int shotCount = 1;

    
    private int shotsFired = 0;

    [SerializeField]
    private float visibilityRange = 5f;

    private bool canShoot = true;

    enum ShootingState
    {
        Searching,
        Shooting
    }

    private MonsterMovement monsterMovement;

    private ShootingState state = ShootingState.Searching;

    private void FoundTarget()
    {
        monsterMovement.enabled = false;
        state = ShootingState.Shooting;
    }

    private void BackToSearch()
    {
        state = ShootingState.Searching;
        monsterMovement.enabled = true;
        target = null;
    }

    private Transform target;

    private void CheckForPlayer()
    {
       Vector2 facingDir = monsterMovement.IsFacingLeft() ? Vector2.left : Vector2.right;


        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y), facingDir, visibilityRange);
        Debug.DrawRay(transform.position, facingDir * visibilityRange, Color.magenta);
        for (int i = 0; i < hits.Length; i++)
        {
            var obj = hits[i].transform;
            if (obj.CompareTag("Player"))
            {
                target = obj;
                FoundTarget();
                break;
            }
        }

    }

    private IEnumerator Shoot()
    {
        Debug.Log("Shooting!");
        canShoot = false;
        yield return new WaitForSeconds(shotCooldown);
        canShoot = true;
        shotsFired++;
    }

    void Start()
    {
        monsterMovement = GetComponent<MonsterMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case ShootingState.Searching:
                CheckForPlayer();
                break;
            case ShootingState.Shooting:
                if (shotsFired >= shotCount)
                {
                    shotsFired = 0;
                    BackToSearch();
                    break;
                }

                if (canShoot)
                {
                    StartCoroutine(Shoot());
                }
                break;
        }
    }


}
