///
///This script moves the projectile forward at a set speed
///
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private float speed = 5f;

    //this should change to a setup function
    private void Start()
    {
        if (TryGetComponent(out Rigidbody temp))
        {
            rb = temp;
        }
        else
            Debug.LogError(gameObject.name + " is missing a rigidbody component.");
    }


    private void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        rb.velocity = transform.forward * speed;
    }
}
