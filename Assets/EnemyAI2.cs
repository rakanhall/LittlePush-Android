using UnityEngine;

public class EnemyAI2 : MonoBehaviour
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
        if (Mathf.Abs(transform.position.y - targetPoint.y) <= 0.1f)
        {
            if (targetPoint == pointB.position)
            {
                targetPoint = pointA.position;
            }
            else
            {
                targetPoint = pointB.position;
            }
        }
    }

    private void MoveTowardsTarget()
    {
        float direction = (targetPoint.y - transform.position.y) > 0 ? 1 : -1;
        transform.position += new Vector3(0, direction * moveSpeed * Time.deltaTime, 0);
    }
}

