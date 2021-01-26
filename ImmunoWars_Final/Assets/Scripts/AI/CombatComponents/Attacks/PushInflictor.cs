///
///This script allows attacks to add push back force to enemies
///
using UnityEngine;
using UnityEngine.AI;

public class PushInflictor : MonoBehaviour
{
    [SerializeField]
    private float PushBackForce = 5f;
    private Vector3 tempForceDirection;

    public void ApplyPushBack(NavMeshAgent navi, Vector3 forceOriginPoint)
    {
            tempForceDirection = (navi.transform.position - forceOriginPoint).normalized;
            navi.velocity = PushBackForce * tempForceDirection;
    }

    public float ReadPushForce()
    {
        return PushBackForce;
    }
}
