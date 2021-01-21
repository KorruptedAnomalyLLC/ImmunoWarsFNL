///
///This script allows an attack to inflict the stun effect
///
using UnityEngine;

public class StunInflictor : MonoBehaviour
{
    [SerializeField]
    private float stunTime = 0.5f;

    public void InflictStun(StatusManager targetStatus)
    {
        targetStatus.ApplyStunEffect(stunTime);
    }
}
