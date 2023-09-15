using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float maxAngle = 20.0f;

    private void Update()
    {
        float angle = maxAngle * Mathf.Sin(Time.time * speed);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    public void StopSwinging()
    {
        maxAngle = 0;
    }
}

