using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 5.0f;

    private Animator animator;
    private Vector3 targetPoint;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isMoving", true);

        targetPoint = pointB.position;
    }

    private void Update()
    {
        MoveTowardsTarget();

        // Check if reached the target point
        if (Mathf.Abs(transform.position.x - targetPoint.x) <= 0.1f)
        {
            if (targetPoint == pointB.position)
            {
                targetPoint = pointA.position;
            }
            else
            {
                targetPoint = pointB.position;
            }

            // Flip the sprite
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void MoveTowardsTarget()
    {
        float direction = (targetPoint.x - transform.position.x) > 0 ? 1 : -1;
        transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);
    }
}



