using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 5f; // You can adjust this value to make the bullet move faster or slower
    

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        
    }
    public void SetDirection(Vector3 direction)
    {
        speed *= direction.x;
        
    }
}


