using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;

    public int destinationIndex = 0;

    public SpriteRenderer sprite;


    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    
    public bool IsFacingLeft()
    {
        return transform.position.x - patrolPoints[destinationIndex].position.x < 0;
    }


    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[destinationIndex].position, moveSpeed * Time.deltaTime);
        sprite.flipX = IsFacingLeft();

        if (Vector2.Distance(transform.position, patrolPoints[destinationIndex].position) <= 0.2f)
        {
            destinationIndex = (destinationIndex + 1) % patrolPoints.Length;
        }
        

    }
}
