using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 5f;  // How long the projectile lives before it is destroyed

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}

