using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector2[] patrolPoints;
    public float moveSpeed;
    public int destinationIndex = 0;
    private Transform attachedPlayerTransform = null;

    void Start()
    {
        patrolPoints[0] = transform.Find("PlatformPoint1").transform.position;
        patrolPoints[1] = transform.Find("PlatformPoint2").transform.position;
    }

    
    public bool IsFacingLeft()
    {
        return patrolPoints[destinationIndex].x - transform.position.x < 0;
    }

    void Update()
    {

        var oldPos = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[destinationIndex], moveSpeed * Time.deltaTime);
        
        if (attachedPlayerTransform != null)
        {
            attachedPlayerTransform.position -= Vector3.right * (oldPos.x - transform.position.x);
        }

        if (Vector2.Distance(transform.position, patrolPoints[destinationIndex]) <= 0.2f)
        {
            destinationIndex = (destinationIndex + 1) % patrolPoints.Length;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attachedPlayerTransform = collision.gameObject.transform;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.transform == attachedPlayerTransform)
        {
            attachedPlayerTransform = null;
        }
    }
}
