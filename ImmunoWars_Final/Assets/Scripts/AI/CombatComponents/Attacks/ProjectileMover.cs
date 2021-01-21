///
///This script moves the projectile forward at a set speed
///
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float speed = 5f;



    private void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        rb.velocity = transform.forward * speed;
    }
}
