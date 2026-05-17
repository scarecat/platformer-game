using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int destinationIndex = 0;
    private Transform attachedPlayerTransform = null;

    void Start()
    {
    }

    
    public bool IsFacingLeft()
    {
        return patrolPoints[destinationIndex].position.x - transform.position.x < 0;
    }

    void Update()
    {

        var oldPos = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[destinationIndex].position, moveSpeed * Time.deltaTime);
        
        if (attachedPlayerTransform != null)
        {
            attachedPlayerTransform.position += Vector3.right * (oldPos.x - transform.position.x);
        }

        if (Vector2.Distance(transform.position, patrolPoints[destinationIndex].position) <= 0.2f)
        {
            destinationIndex = (destinationIndex + 1) % patrolPoints.Length;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attachedPlayerTransform = collision.gameObject.transform;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform == attachedPlayerTransform)
        {
            attachedPlayerTransform = null;
        }
    }
}
