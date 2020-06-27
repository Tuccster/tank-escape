using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private byte _frameCount = 0;
    private Transform _transform;

    private void Awake()
    {
        _transform = gameObject.GetComponent<Transform>();
    }

    private void OnCollisionEnter(Collision col)
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        _frameCount++;
        if (_frameCount % 10 == 0 && _transform.position.y < -64f) Destroy(gameObject);
    }
}
