///
///This script allows attacks to destroy themselves once they've hit something
///

using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    public bool killUnit = false;


    public void DestroyAttack(GameObject destroyTarget)
    {
        Destroy(destroyTarget);
    }
}
