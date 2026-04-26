using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapCollider2D))]
public class SpikesTilemap : MonoBehaviour
{
    [SerializeField] private float damage = 10.0f;

    private void Start()
    {
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerHealth health)) return;



        health.TrapDamage(damage);



    }
}
